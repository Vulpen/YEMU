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
            AddToken(TokenTypes.Instruction, "irmovq");
            AddToken(TokenTypes.Instruction, "rrmovq");
            AddToken(TokenTypes.Instruction, "mrmovq");
            AddToken(TokenTypes.Instruction, "rmmovq");
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

        Parser()
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
            Token[] ParsedTokens = new Token[line.Length];

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
            }
            tokens = null;
            return false;
        }

        private bool ContextParse(Token[] tokens_in, out Token[] tokens_out)
        {
            tokens_out = null;
            return false;
        }
    }
}
