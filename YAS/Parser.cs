using System;
using System.Collections.Generic;
using System.Text;
using YLib;

namespace YAS
{

    /// <summary>
    /// Handles parsing and 'lexing' an input string into a series of tokens.
    /// </summary>
    class Parser
    {
        private Keywords YKeywords;
        private EnumVerboseLevels _verbosityLevel;

        public Parser(EnumVerboseLevels lvl)
        {
            YKeywords = new Keywords();
            _verbosityLevel = lvl;
        }

        /// <summary>
        /// Driver of all necessary private functions and splits string into a list of strings.
        /// </summary>
        /// <returns></returns>
        public Token[] ParseString(string str, out bool success)
        {
            //Trim string----
            str = str.Trim();
            string[] line = str.Split(' ');
            if(line.Length < 1)
            {
                success = false;
                return null;
            }

            if(line == null || line.Length == 0)
            {
                success = false;
                return null;
            }
            //End trim------


            //Check if should be ignored: comments, etc.
            for(int i = 0; i < line.Length; i++)
            {
                if(line[i].StartsWith("//") || line[i].StartsWith("#"))
                {
                    if(i == 0)
                    {
                        Console.WriteLine("Ignored: " + str);
                        success = false;
                        return null;
                    }
                    else
                    {
                        //Allows putting comments on the same line as instructions
                        string[] replace = new string[i];
                        for(int j = 0; j < i; j++)
                        {
                            replace[j] = line[j];
                        }
                        line = replace;
                        break;
                    }
                }
                line[i] = line[i].TrimEnd(',');
            }
            
            Token[] ParsedTokens = new Token[line.Length];

            //Lexer - Turn string into array of tokens
            if(!SimpleKeywordParse(line, ref ParsedTokens))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Failed line on lexing |" + str);
                success = false;
                return null;
            }
            //Parser - Finish populating array of tokens based on their context, and check if the array of tokens makes sense
            if (!ContextParse(ParsedTokens))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Failed on parsing |" + str);
                success = false;
                return null;
            }
            Console.ResetColor();
            Console.WriteLine("Successfully parsed " + str);

            if (_verbosityLevel == EnumVerboseLevels.All)
            {
                for (int i = 0; i < ParsedTokens.Length; i++)
                {
                    Console.Write(ParsedTokens[i].TokenInfoString());
                }
            }
            Console.WriteLine("Yup, everything is good here");
            success = true;
            return ParsedTokens;
        }

        /// <summary>
        /// This loops through every unit in the string and determines if each one is a label, immediate, instruction etc.
        /// </summary>
        /// <returns></returns>
        private bool SimpleKeywordParse(string[] units, ref Token[] tokens)
        {
            tokens = new Token[units.Length];
            for(int i = 0; i < units.Length; i++)
            {
                Token temp = null;
                if (YKeywords.IsKeyword(units[i], ref temp)){
                    //First, check if the string is an existing keyword.
                    //If not, it will be a label, immediate, or an address register with offset
                    tokens[i] = temp;
                }
                else
                {
                    //The first instruction can also be a label
                    if (units[i].EndsWith(':'))
                    {
                        //label
                        tokens[i] = new Token((int)EnumTokenTypes.Label, units[i]);
                        continue;
                    }

                    if (units[i].StartsWith('$'))
                    {
                        //Can be an immediate, or an address register with offset
                        Int64 ImmediateVal = 0;
                        if (units[i].Contains("("))
                        {
                            //Address register with offset, error if no immediate!
                            int j = units[i].IndexOf("(");
                            ImmediateVal = MathConversion.ParseImmediate(units[i].Substring(0, j-1));
                            units[i] = units[i].Substring(j);
                            if (YKeywords.IsKeyword(units[i], ref temp))
                            {
                                temp.AddProperty(EnumTokenProperties.ImmediateValue, ImmediateVal); //ImmediateValue holds the offset!
                                tokens[i] = temp;
                                continue;
                            }
                        }

                        ImmediateVal = MathConversion.ParseImmediate(units[i]);
                        tokens[i] = new Token((int)EnumTokenTypes.Immediate, units[i]);
                        tokens[i].AddProperty(EnumTokenProperties.ImmediateValue, ImmediateVal);
                        continue;
                    }

                    tokens[i] = new Token((int)EnumTokenTypes.Unkown, units[i]);
                }
            }
            return true;
        }

        private bool CheckArithmeticOperation(Token[] tkns)
        {
            if (tkns.Length == 3)
            {
                EnumTokenTypes token1Type;
                EnumTokenTypes token2Type;
                if (tkns[1].GetTokenType(out token1Type) && tkns[2].GetTokenType(out token2Type))
                {
                    if(token1Type == EnumTokenTypes.Immediate)
                    {
                        throw new FoundUnexpectedToken("Illegal immediate used in arithmetic operation");
                    }
                    if ((token1Type == EnumTokenTypes.Register) && token2Type == EnumTokenTypes.Register)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool CheckJMP(Token[] tkns)
        {
            if(tkns.Length == 2)
            {
                EnumTokenTypes token1Type;
                if (tkns[1].GetTokenType(out token1Type))
                {
                    if (token1Type == EnumTokenTypes.Immediate || token1Type == EnumTokenTypes.Label || token1Type == EnumTokenTypes.Unkown)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool CheckMov(Token[] tkns, EnumTokenTypes type1, EnumTokenTypes type2)
        {
            if (tkns.Length == 3)
            {
                EnumTokenTypes token1Type;
                EnumTokenTypes token2Type;
                if (tkns[1].GetTokenType(out token1Type) && tkns[2].GetTokenType(out token2Type))
                {
                    if (token1Type == type1 && token2Type == type2)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool CheckInterrupt(Token[] tkns)
        {
            if (tkns.Length != 2) return false;

            EnumTokenTypes secondTokenType;
            if(tkns[1].GetTokenType(out secondTokenType))
            {
                if(secondTokenType == EnumTokenTypes.Immediate)
                {
                    //Add a check to see if interrupt is valid.
                    return true;
                }
            }

            return false;
        }

        private bool CheckInstruction(Token[] tokens)
        {
            Token firstToken = tokens[0];
            EnumInstructions CurrentTokenInstruction;
            if(!firstToken.GetInstruction(out CurrentTokenInstruction))
            {
                return false;
            }
            switch (CurrentTokenInstruction)
            {
                case EnumInstructions.jle:
                case EnumInstructions.jge:
                case EnumInstructions.jl:
                case EnumInstructions.jg:
                case EnumInstructions.je:
                case EnumInstructions.jne:
                case EnumInstructions.jmp:
                case EnumInstructions.call:
                    return CheckJMP(tokens);
                    break;
                case EnumInstructions.add:
                case EnumInstructions.sub:
                case EnumInstructions.imul:
                case EnumInstructions.xor:
                    return CheckArithmeticOperation(tokens);
                case EnumInstructions.irmov:
                    return CheckMov(tokens, EnumTokenTypes.Immediate, EnumTokenTypes.Register);
                case EnumInstructions.mrmov:
                    return CheckMov(tokens, EnumTokenTypes.AddressRegister, EnumTokenTypes.Register);
                case EnumInstructions.rmmov:
                    return CheckMov(tokens, EnumTokenTypes.Register, EnumTokenTypes.AddressRegister);
                case EnumInstructions.rrmov:
                    return CheckMov(tokens, EnumTokenTypes.Register, EnumTokenTypes.Register);
                case EnumInstructions.interrupt:
                    return CheckInterrupt(tokens);
                case EnumInstructions.ret:
                case EnumInstructions.halt:
                    break;
            }
            return false;
        }

        /// <summary>
        /// This function is what truely parses the tokens. This looks at the first instruction, and checks if arguments are correct.
        /// </summary>
        /// <returns></returns>
        private bool ContextParse(Token[] tokens_in)
        {
            Token firstToken = tokens_in[0];
            EnumTokenTypes CurrentTokenType;
            if(!firstToken.GetTokenType(out CurrentTokenType))
            {
                return false;
            }
            switch (CurrentTokenType)
            {
                case (EnumTokenTypes.Instruction):
                    return CheckInstruction(tokens_in);
                    break;
                case (EnumTokenTypes.Label):
                    //Handle label table
                    return true;
                    break;
                default:
                case (EnumTokenTypes.Immediate):
                case (EnumTokenTypes.Register):
                case (EnumTokenTypes.Unkown):
                    throw new FoundUnexpectedToken(firstToken.DeepCopy());
                    return false;
                    break;
            }
            return false;
        }
    }
}