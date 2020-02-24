using System;
using System.Collections.Generic;
using System.Text;

namespace YAS
{

    class InstructionSize
    {
        private Dictionary<EnumInstructions, int> Sizes;

        public InstructionSize()
        {
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
                        LabelTable.Add(tokens[0].Text, FileSize);
                    }
                }
                EnumInstructions inst;
                Int64 InstructionLength = 0;
                if(tokens[0].GetInstruction(out inst))
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
                File.Add(tempLine);
                return;
            }
            throw new Exception("Tried to add empty line to token file");
        }

        public bool ResolveLabels()
        {
            //Resolves labels on the Token File.
            throw new NotImplementedException();
        }

        public Token[] GetLine(int index)
        {
            if(index >=0 && index < File.Count)
            {
                return File[index].Tokens;
            }
            throw new Exception("Attempt to access invalid index of token file");
            return File[index].Tokens;
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
