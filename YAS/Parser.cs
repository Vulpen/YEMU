using System;
using System.Collections.Generic;
using System.Text;

namespace YAS
{
    enum EnumTokenTypes
    {
        Instruction = 0,
        Register,
        Immediate,
        Label,
        Unkown,
    }

    enum EnumTokenProperties{
        TokenType = 0,   //Corresponds to EnumTokenTypes
        LabelId,  //Used for label table
        OpSizeBytes,  //stores 1, 2, 4, 8 if size is added
        ImmediateValue, //If is immediate, stores the value
        RealInstruction, //Corresponds to EnumInstructions
        RegisterNumber   //Corresponds to EnumRegisters
    }

    enum EnumInstructions
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

    enum EnumRegisters
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
    /// A Registry of Keyword tokens in the Y86 language.
    /// </summary>
    class Keywords
    {
        private List<Token> keys;

        public Keywords()
        {
            keys = new List<Token>();
            GenerateTokens(ref keys);
            //AddToken(EnumTokenTypes.Instruction, "addq");
            //AddToken(EnumTokenTypes.Instruction, "subq");
            //AddToken(EnumTokenTypes.Instruction, "andq");
            //AddToken(EnumTokenTypes.Instruction, "xorq");
            //AddToken(EnumTokenTypes.Instruction, "imulq");
            //AddToken(EnumTokenTypes.Instruction, "jmp");
            //AddToken(EnumTokenTypes.Instruction, "jle");
            //AddToken(EnumTokenTypes.Instruction, "jl");
            //AddToken(EnumTokenTypes.Instruction, "je");
            //AddToken(EnumTokenTypes.Instruction, "jne");
            //AddToken(EnumTokenTypes.Instruction, "jge");
            //AddToken(EnumTokenTypes.Instruction, "jg");
            //AddToken(EnumTokenTypes.Instruction, "irmovq");
            //AddToken(EnumTokenTypes.Instruction, "rrmovq");
            //AddToken(EnumTokenTypes.Instruction, "mrmovq");
            //AddToken(EnumTokenTypes.Instruction, "rmmovq");
            //AddToken(EnumTokenTypes.Register, "%rax");
            //AddToken(EnumTokenTypes.Register, "%rbx");
            //AddToken(EnumTokenTypes.Register, "%rcx");
            //AddToken(EnumTokenTypes.Register, "%rdx");
            //AddToken(EnumTokenTypes.Register, "%rsp");
            //AddToken(EnumTokenTypes.Register, "%rbp");
            //AddToken(EnumTokenTypes.Register, "%rsi");
            //AddToken(EnumTokenTypes.Register, "%rdi");
            //AddToken(EnumTokenTypes.Register, "%r8");
            //AddToken(EnumTokenTypes.Register, "%r9");
            //AddToken(EnumTokenTypes.Register, "%r10");
            //AddToken(EnumTokenTypes.Register, "%r11");
            //AddToken(EnumTokenTypes.Register, "%r12");
            //AddToken(EnumTokenTypes.Register, "%r13");
            //AddToken(EnumTokenTypes.Register, "%r14");
        }

        public void AddInstructionToken(List<Token> tokens, string str, EnumInstructions instructionEnum)
        {
            Token tkn = new Token(str);
            tkn.AddProperty(EnumTokenProperties.TokenType, (Int64)EnumTokenTypes.Instruction);
            tkn.AddProperty(EnumTokenProperties.RealInstruction, (Int64)instructionEnum);
        }

        private void AddRegisterToken(List<Token> tokens, string str, EnumRegisters registerEnum)
        {
            Token tkn = new Token(str);
            tkn.AddProperty(EnumTokenProperties.TokenType, (Int64)EnumTokenTypes.Instruction);
            tkn.AddProperty(EnumTokenProperties.RegisterNumber, (Int64)registerEnum);
            tokens.Add(tkn);
        }

        public void GenerateTokens(ref List<Token> tknList)
        {
            //Token tkn = new Token()
            //-----Instructions
            tknList.Add(new Token((Int64)EnumTokenTypes.Instruction, (Int64)EnumInstructions.add, "addq"));
            tknList.Add(new Token((Int64)EnumTokenTypes.Instruction, (Int64)EnumInstructions.sub, "subq"));
            tknList.Add(new Token((Int64)EnumTokenTypes.Instruction, (Int64)EnumInstructions.xor, "xorq"));

            //-----Registers
            AddRegisterToken(tknList, "%rax", EnumRegisters.rax);
            AddRegisterToken(tknList, "%rbx", EnumRegisters.rbx);
            AddRegisterToken(tknList, "%rcx", EnumRegisters.rcx);
            AddRegisterToken(tknList, "%rdx", EnumRegisters.rdx);
        }

        public bool IsKeyword(string val, ref Token tkn)
        {
            for (int i = 0; i < keys.Count; i++)
            {
                if (keys[i].Text == val)
                {
                    tkn = keys[i].DeepCopy();
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
    /// Tokens represent atomic units of text in assembly and contains properties associated with the token.
    /// </summary>
    class Token
    {
        private string _text;
        private Dictionary<EnumTokenProperties, Int64> Properties;

        public Token()
        {
            Properties = new Dictionary<EnumTokenProperties, long>();
        }

        public Token(string str)
        {
            Properties = new Dictionary<EnumTokenProperties, long>();
            _text = str;
        }

        /// <summary>
        /// Creates a token and automatically adds TokenType.
        /// </summary>
        public Token(Int64 TokenTypeVal, string str)
        {
            _text = str;
            Properties = new Dictionary<EnumTokenProperties, long>();
            AddProperty(EnumTokenProperties.TokenType, TokenTypeVal);
        }

        /// <summary>
        /// Creates a token and automatically adds TokenType and RealInstruction Properties.
        /// </summary>
        public Token(Int64 TokenTypeVal, Int64 RealInstructionVal, string str)
        {
            Properties = new Dictionary<EnumTokenProperties, long>();
            _text = str;
            AddProperty(EnumTokenProperties.TokenType, TokenTypeVal);
            AddProperty(EnumTokenProperties.RealInstruction, RealInstructionVal);
        }

        public Token(Int64 TokenTypeVal, Int64 RealInstructionVal, int InstructionSize, string str)
        {
            Properties = new Dictionary<EnumTokenProperties, long>();
            _text = str;
            AddProperty(EnumTokenProperties.TokenType, TokenTypeVal);
            AddProperty(EnumTokenProperties.RealInstruction, RealInstructionVal);
            AddProperty(EnumTokenProperties.OpSizeBytes, (Int64)InstructionSize);
        }

        public string Text
        {
            get { return _text; }
        }

        public bool AddProperty(EnumTokenProperties key, Int64 val)
        {
            if (!Properties.ContainsKey(key))
            {
                //Perhaps add some key/value checking here
                Properties.Add(key, val);
                return true;
            }
            //Invalid Property Exception
            return false;
        }

        public bool GetProperty(EnumTokenProperties key, out Int64 val)
        {
            val = -1;
            if (Properties.ContainsKey(key))
            {
                val = Properties[key];
                return true;
            }
            return false;
        }

        public bool GetProperty(EnumTokenProperties key, out int val)
        {
            val = -1;
            if (Properties.ContainsKey(key))
            {
                val = (int)Properties[key];
                return true;
            }
            return false;
        }

        public bool GetInstruction(out EnumInstructions val)
        {
            val = EnumInstructions.add;
            if (Properties.ContainsKey(EnumTokenProperties.RealInstruction))
            {
                val = (EnumInstructions)Properties[EnumTokenProperties.RealInstruction];
                return true;
            }
            return false;
        }

        public bool GetTokenType(out EnumTokenTypes val)
        {
            val = EnumTokenTypes.Unkown;
            if (Properties.ContainsKey(EnumTokenProperties.TokenType))
            {
                val = (EnumTokenTypes)Properties[EnumTokenProperties.TokenType];
                return true;
            }
            return false;
        }

        public Token DeepCopy()
        {
            //Ensure a new instance of _text is created
            char[] s = _text.ToCharArray();
            Token temp = new Token(s.ToString());
            foreach (var prop in Properties)
            {
                temp.AddProperty(prop.Key, prop.Value);
            }
            return temp;
        }
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

            SimpleKeywordParse(line, ref ParsedTokens);

            ContextParse(ParsedTokens);
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
                    }
                    //Find if it is an 'Unknown', we can't always tell if immediate or an 'unknown' variable or label
                    //$ indicates immediate, 0x after that indicates HEX
                    if (units[i].StartsWith('$'))
                    {
                        //immediate
                        tokens[i] = new Token((int)EnumTokenTypes.Immediate, units[i]);
                    }
                    //throw parse exception here!
                    return false;
                }
            }
            return true;
        }

        private bool CheckArithmeticOperation(Token[] tkns)
        {
            if(tkns.Length == 3)
            {
                EnumTokenTypes token1Type;
                EnumTokenTypes token2Type;
                if(tkns[1].GetTokenType(out token1Type) && tkns[2].GetTokenType(out token2Type))
                {
                    if(token1Type == EnumTokenTypes.Register && token2Type == EnumTokenTypes.Register)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool CheckJMP(Token[] tkns)
        {
            throw new NotImplementedException();
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
                    break;
                case EnumInstructions.add:
                case EnumInstructions.sub:
                case EnumInstructions.imul:
                case EnumInstructions.xor:
                    return CheckArithmeticOperation(tokens);
                    break;
                case EnumInstructions.irmov:
                    break;
                case EnumInstructions.mrmov:
                    break;
                case EnumInstructions.rmmov:
                    break;
                case EnumInstructions.rrmov:
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
                case (EnumTokenTypes.Immediate):
                    break;
                case (EnumTokenTypes.Label):
                    break;
                default:
                case (EnumTokenTypes.Register):
                case (EnumTokenTypes.Unkown):
                    return false;
                    break;
            }
            return false;
        }
    }
}
