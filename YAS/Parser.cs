using System;
using System.Collections.Generic;
using System.Text;

namespace YAS
{
    public enum EnumTokenTypes
    {
        Instruction = 0,
        Register,
        AddressRegister,
        Immediate,
        Label,
        Unkown,
    }

    public enum EnumTokenProperties{
        TokenType = 0,   //Corresponds to EnumTokenTypes
        LabelId,  //Used for label table
        OpSizeBytes,  //stores 1, 2, 4, 8 if size is added
        ImmediateValue, //If is immediate, stores the value
        RealInstruction, //Corresponds to EnumInstructions
        RegisterNumber   //Corresponds to EnumRegisters
    }

    public enum EnumInstructions
    {
        //Todo: add cmov's
        add = 0,
        sub,
        xor,
        imul,
        jmp,
        jle,
        jl,
        je,
        jne,
        jge,
        jg,
        irmov,
        rrmov,
        mrmov,
        rmmov,
        halt,
        nop,
        call,
        ret,
        push,
        pop
    }

    /// <summary>
    /// Represents Y86 Registers. These are in order of their Identifying number.
    /// </summary>
    public enum EnumRegisters
    {
        rax = 0,
        rcx,
        rdx,
        rbx,
        rsp,
        rbp,
        rsi,
        rdi,
        r8,
        r9,
        r10,
        r11,
        r12,
        r13,
        r14
    }

    /// <summary>
    /// Handles parsing and 'lexing' an input string into a series of tokens.
    /// </summary>
    class Parser
    {
        private Keywords YKeywords;

        public Parser()
        {
            YKeywords = new Keywords();
        }

        /// <summary>
        /// Driver of all necessary private functions and splits string into a list of strings.
        /// </summary>
        /// <returns></returns>
        public List<Token> ParseString(string str)
        {
            str = str.Trim();
            string[] line = str.Split(' ');
            if(line.Length < 1)
            {
                return null;
            }

            if(line == null || line.Length == 0)
            {
                return null;
            }

            for(int i = 0; i < line.Length; i++)
            {
                if(line[i].StartsWith("//") || line[i].StartsWith("#"))
                {
                    if(i == 0)
                    {
                        Console.WriteLine("Successfully parsed " + str);
                        return new List<Token>();
                    }
                    else
                    {
                        string[] replace = new string[i];
                        for(int j = 0; j < replace.Length; j++)
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

            //Lexer
            if(!SimpleKeywordParse(line, ref ParsedTokens))
            {
                Console.WriteLine("Failed line on lexing |" + str);
                return null;
            }
            //Parser
            if (!ContextParse(ParsedTokens))
            {
                Console.WriteLine("Failed on parsing |" + str);
                return null;
            }
            //Second Pass
            Console.WriteLine("Successfully parsed " + str);
            return null;
        }

        /// <summary>
        /// This is a naive parse that loops through every unit in the string and determines if each one is a label, immediate, instruction etc.
        /// </summary>
        /// <returns></returns>
        private bool SimpleKeywordParse(string[] units, ref Token[] tokens)
        {
            tokens = new Token[units.Length];
            for(int i = 0; i < units.Length; i++)
            {
                Token temp = null;
                if (YKeywords.IsKeyword(units[i], ref temp)){
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
                        //Can be an immediate, or a memory address stored in a register WITH an offset, which would be an address register.
                        if (units[i].Contains("("))
                        {

                        }
                        tokens[i] = new Token((int)EnumTokenTypes.Immediate, units[i]);
                        continue;
                    }

                    tokens[i] = new Token((int)EnumTokenTypes.Unkown, units[i]);
                    continue;
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
                    if ((token1Type == EnumTokenTypes.Register || token1Type == EnumTokenTypes.Immediate) && token2Type == EnumTokenTypes.Register)
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
                    break;
                case EnumInstructions.irmov:
                    return CheckMov(tokens, EnumTokenTypes.Immediate, EnumTokenTypes.Register);
                    break;
                case EnumInstructions.mrmov:
                    return CheckMov(tokens, EnumTokenTypes.AddressRegister, EnumTokenTypes.Register);
                    break;
                case EnumInstructions.rmmov:
                    return CheckMov(tokens, EnumTokenTypes.Register, EnumTokenTypes.AddressRegister);
                    break;
                case EnumInstructions.rrmov:
                    return CheckMov(tokens, EnumTokenTypes.Register, EnumTokenTypes.Register);
                    break;
                case EnumInstructions.ret:
                case EnumInstructions.halt:
                    break;
            }
            return false;
        }

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
                    throw new FoundUnexpectedToken(firstToken);
                    return false;
                    break;
            }
            return false;
        }
    }
}