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

    /// <summary>
    /// A representation of our file in arrays of tokens.
    /// </summary>
    class TokenFile
    {
        private Dictionary<string, Int64> LabelTable;
        private List<Token[]> File;
        private Int64 FileSize;
        private InstructionSize Y86Sizes;

        public TokenFile()
        {
            File = new List<Token[]>();
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
                if(tokens[0].GetInstruction(out inst))
                {
                    FileSize += Y86Sizes.GetInstructionSize(inst);
                }

                File.Add(tokens);
            }
            throw new Exception("Tried to add empty line to token file");
        }

        public Token[] GetLine(int index)
        {
            if(index >=0 && index < File.Count)
            {
                return File[index];
            }
            throw new Exception("Attempt to access invalid index of token file");
            return File[index];
        }
    }
}
