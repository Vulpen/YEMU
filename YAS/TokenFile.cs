using System;
using System.Collections.Generic;
using System.Text;

namespace YAS
{

    class InstructionSize
    {
        private Dictionary<EnumInstructions, int> Sizes;

        public static Dictionary<EnumInstructions, int> AllSizes = new Dictionary<EnumInstructions, int>
        {
            {EnumInstructions.halt,  1},
            {EnumInstructions.nop,   1},
            {EnumInstructions.rrmov, 2},
            {EnumInstructions.irmov, 10},
            {EnumInstructions.rmmov, 10},
            {EnumInstructions.mrmov, 10},
            {EnumInstructions.ret,   1},
            {EnumInstructions.push,  2},
            {EnumInstructions.pop,   2},
            {EnumInstructions.add,   2},
            {EnumInstructions.sub,   2},
            {EnumInstructions.imul,  2},
            {EnumInstructions.xor,   2},
            {EnumInstructions.jmp,   9},
            {EnumInstructions.je,    9},
            {EnumInstructions.jle,   9},
            {EnumInstructions.jge,   9},
            {EnumInstructions.jg,    9},
            {EnumInstructions.jl,    9},
        };

        public InstructionSize()
        {
            Sizes = new Dictionary<EnumInstructions, int>();
            Sizes.Add(EnumInstructions.halt, 1);
            Sizes.Add(EnumInstructions.nop, 1);
            Sizes.Add(EnumInstructions.rrmov, 2);
            Sizes.Add(EnumInstructions.irmov, 10);
            Sizes.Add(EnumInstructions.rmmov, 10);
            Sizes.Add(EnumInstructions.mrmov, 10);
            Sizes.Add(EnumInstructions.ret, 1);
            Sizes.Add(EnumInstructions.push, 2);
            Sizes.Add(EnumInstructions.pop, 2);
            Sizes.Add(EnumInstructions.add, 2);
            Sizes.Add(EnumInstructions.sub, 2);
            Sizes.Add(EnumInstructions.imul, 2);
            Sizes.Add(EnumInstructions.xor, 2);
            Sizes.Add(EnumInstructions.jmp, 9);
            Sizes.Add(EnumInstructions.je, 9);
            Sizes.Add(EnumInstructions.jle, 9);
            Sizes.Add(EnumInstructions.jge, 9);
            Sizes.Add(EnumInstructions.jg, 9);
            Sizes.Add(EnumInstructions.jl, 9);
        }

        public int GetInstructionSize(EnumInstructions inst)
        {
            if (Sizes.ContainsKey(inst))
            {
                return Sizes[inst];
            }
            throw new Exception("Tried to get an unsupported instruction size.");
        }
    }

    class TokenLine
    {
        public Token[] Tokens;
        public Int64 BeginAddress;
        public Int64 EndAddress;

        public bool ContainsLabel()
        {
            if(Tokens != null)
            {
                for(int i = 0; i < Tokens.Length; i++)
                {
                    EnumTokenTypes temp;
                    Tokens[i].GetTokenType(out temp);
                    if (temp == EnumTokenTypes.Label)
                    {
                        return true;
                    }
                }
                return false;
            }
            return false;
        }

        /// <summary>
        /// Returns index in the line of a label, -1 if there is no label in the line.
        /// </summary>
        /// <returns></returns>
        public int ReturnLabelIndex()
        {
            if (Tokens != null)
            {
                for (int i = 0; i < Tokens.Length; i++)
                {
                    EnumTokenTypes temp;
                    Tokens[i].GetTokenType(out temp);
                    if (temp == EnumTokenTypes.Label || temp == EnumTokenTypes.Unkown)
                    {
                        return i;
                    }
                }
                return -1;
            }
            return -1;
        }
    }

    /// <summary>
    /// A representation of our file in arrays of tokens.
    /// </summary>
    class TokenFile
    {
        private Dictionary<string, Int64> LabelTable;
        //private List<Token[]> File;
        private List<TokenLine> File;
        private Int64 FileSize;
        private InstructionSize Y86Sizes;
        public int NumberOfLines
        {
            get { return File.Count; }
        }

        public TokenFile()
        {
            File = new List<TokenLine>();
            LabelTable = new Dictionary<string, long>();
            Y86Sizes = new InstructionSize();
            FileSize = 0;
        }

        /// <summary>
        /// Adds a line to the file and also adds labels to the label table as necessary.
        /// </summary>
        /// <param name="tokens"></param>
        public void AddLine(Token[] tokens)
        {
            if(tokens.Length > 0)
            {
                EnumTokenTypes type;
                if(tokens[0].GetTokenType(out type))
                {
                    if(type == EnumTokenTypes.Label)
                    {
                        if (!LabelTable.ContainsKey(tokens[0].Text))
                        {
                            //tokens[0].Text = tokens[0].Text.Trim(':');
                            LabelTable.Add(tokens[0].Text.Trim(':'), FileSize);
                        }
                        return;
                    }

                    if(type == EnumTokenTypes.Instruction)
                    {
                        EnumInstructions inst;
                        Int64 InstructionLength = 0;
                        if (tokens[0].GetInstruction(out inst))
                        {
                            InstructionLength = Y86Sizes.GetInstructionSize(inst);
                        }
                        else
                        {
                            throw new AssemblerException(EnumAssemblerStages.TokenFile, "Could not get instruction length");
                        }
                        TokenLine tempLine = new TokenLine();
                        tempLine.Tokens = tokens;
                        tempLine.BeginAddress = FileSize;
                        tempLine.EndAddress = tempLine.BeginAddress + InstructionLength;
                        FileSize = tempLine.EndAddress;
                        File.Add(tempLine);
                        return;
                    }
                }
            }
            throw new Exception("Tried to add empty line to token file");
        }

        public bool ResolveLabels()
        {
            //Resolves labels on the Token File.
            //Loop through all token lines and replace instances of label tokens with respective immediate tokens

            for(int i = 0; i < File.Count; i++)
            {
                int index = File[i].ReturnLabelIndex();
                if (index >= 0)
                {
                    string labelText = File[i].Tokens[index].Text;
                    Int64 replaceLiteral = 0;
                    if (LabelTable.ContainsKey(labelText)) {
                        replaceLiteral = LabelTable[labelText];
                    }
                    else
                    {
                        throw new AssemblerException(EnumAssemblerStages.TokenFile, "LABEL RESOLVE - Could not find label in label table");
                        return false;
                    }
                    Token replacement = new Token((int)EnumTokenTypes.Immediate, "$" + replaceLiteral.ToString());
                    replacement.AddProperty(EnumTokenProperties.ImmediateValue, replaceLiteral);
                    File[i].Tokens[index] = replacement;
                }
                continue;
            }

            return true;
        }

        public TokenLine GetLine(int index)
        {
            if(index >=0 && index < File.Count)
            {
                return File[index];
            }
            throw new Exception("Attempt to access invalid index of token file");
            return File[index];
        }

        public TokenLine GetInstructionLine(Int64 Address)
        {
            for(int i = 0; i < File.Count; i++)
            {
                if(File[i].BeginAddress == Address)
                {
                    return File[i];
                }
            }
            throw new AssemblerException(EnumAssemblerStages.TokenFile, "Could not retreive instruction line at address + " + Address.ToString());
        }
    }
}
