using System;
using System.Collections.Generic;
using System.Text;

namespace YAS
{
    enum TokenTypes
    {
        Instruction = 0,
        OpInstruction,
        JumpInstruction,
        MoveInstruction,
        Register,
        Immediate,
        Label,
        Uknown,
    }

    /// <summary>
    /// Tokens represent atomic units of text in assembly.
    /// </summary>
    class Token
    {
        private int _type;
        private string _text;

        public int Type
        {
            get { return _type; }
        }

        public string Text
        {
            get { return _text; }
        }

        public Token(int type, string str)
        {
            _type = type;
            _text = str;

        }
        public Token(TokenTypes type, string str)
        {
            _type = (int)type;
            _text = str;
        }

        public Token(Token other)
        {
            _type = other._type;
            _text = other._text;
        }
    }

    /// <summary>
    /// A Registry of Keyword tokens in the Y86 language.
    /// </summary>
    class Keywords
    {
        private List<Token> keys;

        private void AddToken(TokenTypes type, string str)
        {
            keys.Add(new Token(type, str));
        }

        public Keywords()
        {
            keys = new List<Token>();
            AddToken(TokenTypes.OpInstruction, "addq");
            AddToken(TokenTypes.OpInstruction, "subq");
            AddToken(TokenTypes.OpInstruction, "andq");
            AddToken(TokenTypes.OpInstruction, "xorq");
            AddToken(TokenTypes.OpInstruction, "imulq");
            AddToken(TokenTypes.JumpInstruction, "jmp");
            AddToken(TokenTypes.JumpInstruction, "jle");
            AddToken(TokenTypes.JumpInstruction, "jl");
            AddToken(TokenTypes.JumpInstruction, "je");
            AddToken(TokenTypes.JumpInstruction, "jne");
            AddToken(TokenTypes.JumpInstruction, "jge");
            AddToken(TokenTypes.JumpInstruction, "jg");
            AddToken(TokenTypes.MoveInstruction, "irmovq");
            AddToken(TokenTypes.MoveInstruction, "rrmovq");
            AddToken(TokenTypes.MoveInstruction, "mrmovq");
            AddToken(TokenTypes.MoveInstruction, "rmmovq");
            AddToken(TokenTypes.Register, "%rax");
            AddToken(TokenTypes.Register, "%rbx");
            AddToken(TokenTypes.Register, "%rcx");
            AddToken(TokenTypes.Register, "%rdx");
            AddToken(TokenTypes.Register, "%rsp");
            AddToken(TokenTypes.Register, "%rbp");
            AddToken(TokenTypes.Register, "%rsi");
            AddToken(TokenTypes.Register, "%rdi");
            AddToken(TokenTypes.Register, "%r8");
            AddToken(TokenTypes.Register, "%r9");
            AddToken(TokenTypes.Register, "%r10");
            AddToken(TokenTypes.Register, "%r11");
            AddToken(TokenTypes.Register, "%r12");
            AddToken(TokenTypes.Register, "%r13");
            AddToken(TokenTypes.Register, "%r14");
        }

        public bool IsKeyword(string val, out Token tkn)
        {
            tkn = null;
            for(int i = 0; i < keys.Count; i++)
            {
                if(keys[i].Text == val)
                {
                    tkn = new Token(keys[i].Type, val); 
                    return true;
                }
            }
            return false;
        }

        public bool IsKeyword(string val)
        {
            for (int i = 0; i < keys.Count; i++)
            {
                if (keys[i].Text == val)
                {
                    return true;
                }
            }
            return false;
        }
    }

    /// <summary>
    /// Handles parsing an input string into a series of tokens.
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
            string[] line = str.Split(' ');
            if(line == null || line.Length == 0)
            {
                return null;
            }

            for(int i = 0; i < line.Length; i++)
            {
                line[i] = line[i].TrimEnd(',');
            }

            Token[] ParsedTokens = new Token[line.Length];

            SimpleKeywordParse(line, out ParsedTokens);

            Token[] ContextTokens = new Token[ParsedTokens.Length];
            ContextParse(ParsedTokens);
            Console.WriteLine("Successfully parsed " + str);
            return null;
        }

        private bool SimpleKeywordParse(string[] units, out Token[] tokens)
        {

            tokens = new Token[units.Length];

            for(int i = 0; i < units.Length; i++)
            {
                Token temp;
                if (YKeywords.IsKeyword(units[i], out temp)){
                    tokens[i] = temp;
                }
                else
                {
                    //The first instruction can also be a label
                    if (units[i].EndsWith(':'))
                    {
                        //label
                        tokens[i] = new Token((int)TokenTypes.Label, units[i]);
                    }
                    //Find if it is an 'Unknown', we can't always tell if immediate or an 'unknown' variable or label
                    //$ indicates immediate, 0x after that indicates HEX
                    if (units[i].StartsWith('$'))
                    {
                        //immediate
                        tokens[i] = new Token((int)TokenTypes.Immediate, units[i]);
                    }

                    //throw parse exception here!
                    return false;
                }
            }
            return true;
        }

        private bool ContextParse(Token[] tokens_in)
        {
            Token firstToken = tokens_in[0];
            switch (firstToken.Type)
            {
                case ((int)TokenTypes.OpInstruction):
                    //Tokens must be of length 3 and have 2 register arguments
                    if(tokens_in.Length == 3)
                    {
                        if(tokens_in[1].Type == (int)TokenTypes.Register && tokens_in[2].Type == (int)TokenTypes.Register)
                        {
                            return true;
                        }
                    }
                    //Throw Exception Here!
                    break;
                case ((int)TokenTypes.MoveInstruction):
                    if(firstToken.Text.ToLower() == "irmovq")
                    {
                        if (tokens_in[1].Type == (int)TokenTypes.Immediate && tokens_in[2].Type == (int)TokenTypes.Register)
                        {
                            return true;
                        }
                    }
                    if (firstToken.Text.ToLower() == "rrmovq")
                    {
                        if (tokens_in[1].Type == (int)TokenTypes.Register && tokens_in[2].Type == (int)TokenTypes.Register)
                        {
                            return true;
                        }
                    }
                    if (firstToken.Text.ToLower() == "rmmovq")
                    {
                        if (tokens_in[1].Type == (int)TokenTypes.Register && tokens_in[2].Type == (int)TokenTypes.Immediate)
                        {
                            return true;
                        }
                    }
                    if (firstToken.Text.ToLower() == "mrmovq")
                    {
                        if (tokens_in[1].Type == (int)TokenTypes.Immediate && tokens_in[2].Type == (int)TokenTypes.Register)
                        {
                            return true;
                        }
                    }
                    break;
                case ((int)TokenTypes.JumpInstruction):

                    break;
                case ((int)TokenTypes.Label):
                    //Must be length 1
                    if(tokens_in.Length == 1)
                    {
                        return true;
                    }
                    break;

                default:
                    //Throw Exception here!
                    break;
            }
            string exstring = "";
            for(int i = 0; i < tokens_in.Length; i++)
            {
                exstring += tokens_in[i].Text + " ";
            }
            throw new Exception("Could not context parse line " + exstring);
            return false;
        }
    }
}
