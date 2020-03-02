using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace YAS
{
    /// <summary>
    /// The class which converts TokenFile objects into binary files.
    /// </summary>
    class BinaryFileWriter
    {
        public BinaryFileWriter()
        {

        }

        /// <summary>
        /// Writes a given TokenFile to a binary file.
        /// </summary>
        public void WriteToFile(TokenFile tokenFile, string destPath)
        {
            //TODO Create Wrapper functions for movs, jmps, etc.
            using (BinaryWriter filestream = new BinaryWriter(File.Open(destPath, FileMode.Create)))
            {
                for(int line = 0; line < tokenFile.NumberOfLines; line++)
                {
                    TokenLine currentTokenLine = tokenFile.GetLine(line);
                    Token firstToken = currentTokenLine.Tokens[0];
                    EnumInstructions first_instruction;
                    if(!firstToken.GetInstruction(out first_instruction))
                    {
                        return;
                    }

                    byte[] someBytes = new byte[0];
                    byte aByte;
                    long someProperty;
                    long otherProperty;
                    int rA;
                    int rB;
                    int tmpInt;
                    switch (first_instruction)
                    {
                        case EnumInstructions.halt:
                            filestream.Write((byte)0);
                            break;
                        case EnumInstructions.nop:
                            filestream.Write((byte)16);
                            break;
                        case EnumInstructions.rrmov:
                            if (currentTokenLine.Tokens.Length != 3)
                                throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Not enough tokens in RRMOV instruction.");

                            filestream.Write((byte)32);

                            if (!currentTokenLine.Tokens[1].GetProperty(EnumTokenProperties.RegisterNumber, out rA))
                                throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Token does not contain register code.");

                            if (!currentTokenLine.Tokens[2].GetProperty(EnumTokenProperties.RegisterNumber, out rB) || currentTokenLine.Tokens[2].TokenType != EnumTokenTypes.AddressRegister)
                                throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Token does not contain register code.");

                            tmpInt = rA << 4;
                            tmpInt = tmpInt | rB;
                            filestream.Write((byte)tmpInt);
                            tmpInt = 0;
                            break;
                        case EnumInstructions.irmov:
                            if (currentTokenLine.Tokens.Length != 3)
                                throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Not enough tokens in RRMOV instruction.");

                            filestream.Write((byte)48);

                            if (!currentTokenLine.Tokens[1].GetProperty(EnumTokenProperties.ImmediateValue, out someProperty) || currentTokenLine.Tokens[1].TokenType != EnumTokenTypes.Immediate)
                                throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Token is not expected immediate value.");

                            if (!currentTokenLine.Tokens[2].GetProperty(EnumTokenProperties.RegisterNumber, out rB) || currentTokenLine.Tokens[2].TokenType != EnumTokenTypes.Register)
                                throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Token is not expected address register.");

                            filestream.Write((byte)(rB + 240));//Write byte F,rB
                            someBytes = BitConverter.GetBytes(someProperty);
                            WriteBytes(someBytes, filestream);
                            break;
                        case EnumInstructions.rmmov:
                            if (currentTokenLine.Tokens.Length != 3)
                                throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Not enough tokens in RRMOV instruction.");

                            filestream.Write((byte)64);

                            if (!currentTokenLine.Tokens[1].GetProperty(EnumTokenProperties.RegisterNumber, out rA) || currentTokenLine.Tokens[1].TokenType != EnumTokenTypes.Register)
                                throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Token is not expected immediate value.");

                            if (!currentTokenLine.Tokens[2].GetProperty(EnumTokenProperties.RegisterNumber, out rB) || currentTokenLine.Tokens[2].TokenType != EnumTokenTypes.AddressRegister)
                                throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Token is not expected immediate value.");

                            //someProperty represents the offset of destination memory address
                            if (!currentTokenLine.Tokens[2].GetProperty(EnumTokenProperties.ImmediateValue, out someProperty))
                                throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Token is not expected immediate value.");

                            tmpInt = rA << 4;
                            tmpInt = tmpInt | rB;
                            filestream.Write((byte)tmpInt);
                            tmpInt = 0;

                            someBytes = BitConverter.GetBytes(someProperty);
                            WriteBytes(someBytes, filestream);
                            break;
                        case EnumInstructions.mrmov:
                            if (currentTokenLine.Tokens.Length != 3)
                                throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Not enough tokens in RRMOV instruction.");

                            filestream.Write((byte)80);

                            if (!currentTokenLine.Tokens[1].GetProperty(EnumTokenProperties.RegisterNumber, out rA) || currentTokenLine.Tokens[1].TokenType != EnumTokenTypes.AddressRegister)
                                throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Token is not expected immediate value.");

                            if (!currentTokenLine.Tokens[2].GetProperty(EnumTokenProperties.RegisterNumber, out rB) || currentTokenLine.Tokens[2].TokenType != EnumTokenTypes.Register)
                                throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Token is not expected immediate value.");

                            //someProperty represents the offset of destination memory address
                            if (!currentTokenLine.Tokens[1].GetProperty(EnumTokenProperties.ImmediateValue, out someProperty))
                                throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Token is not expected immediate value.");

                            tmpInt = rA << 4;
                            tmpInt = tmpInt | rB;
                            filestream.Write((byte)tmpInt);
                            tmpInt = 0;

                            someBytes = BitConverter.GetBytes(someProperty);
                            WriteBytes(someBytes, filestream);
                            break;
                        case EnumInstructions.add:
                            if (currentTokenLine.Tokens.Length != 3)
                                throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Not enough tokens in RRMOV instruction.");

                            filestream.Write((byte)96);

                            if (!currentTokenLine.Tokens[1].GetProperty(EnumTokenProperties.RegisterNumber, out rA) || currentTokenLine.Tokens[1].TokenType != EnumTokenTypes.Register)
                                throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Token is not expected immediate value.");

                            if (!currentTokenLine.Tokens[2].GetProperty(EnumTokenProperties.RegisterNumber, out rB) || currentTokenLine.Tokens[2].TokenType != EnumTokenTypes.Register)
                                throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Token is not expected immediate value.");

                            tmpInt = rA << 4;
                            tmpInt = tmpInt | rB;
                            filestream.Write((byte)tmpInt);
                            tmpInt = 0;
                            break;
                        case EnumInstructions.sub:
                            if (currentTokenLine.Tokens.Length != 3)
                                throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Not enough tokens in RRMOV instruction.");

                            filestream.Write((byte)(96 + 1));

                            if (!currentTokenLine.Tokens[1].GetProperty(EnumTokenProperties.RegisterNumber, out rA) || currentTokenLine.Tokens[1].TokenType != EnumTokenTypes.Register)
                                throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Token is not expected immediate value.");

                            if (!currentTokenLine.Tokens[2].GetProperty(EnumTokenProperties.RegisterNumber, out rB) || currentTokenLine.Tokens[2].TokenType != EnumTokenTypes.Register)
                                throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Token is not expected immediate value.");

                            tmpInt = rA << 4;
                            tmpInt = tmpInt | rB;
                            filestream.Write((byte)tmpInt);
                            tmpInt = 0;
                            break;
                        case EnumInstructions.and:
                            if (currentTokenLine.Tokens.Length != 3)
                                throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Not enough tokens in RRMOV instruction.");

                            filestream.Write((byte)(96 + 2));

                            if (!currentTokenLine.Tokens[1].GetProperty(EnumTokenProperties.RegisterNumber, out rA) || currentTokenLine.Tokens[1].TokenType != EnumTokenTypes.Register)
                                throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Token is not expected immediate value.");

                            if (!currentTokenLine.Tokens[2].GetProperty(EnumTokenProperties.RegisterNumber, out rB) || currentTokenLine.Tokens[2].TokenType != EnumTokenTypes.Register)
                                throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Token is not expected immediate value.");

                            tmpInt = rA << 4;
                            tmpInt = tmpInt | rB;
                            filestream.Write((byte)tmpInt);
                            tmpInt = 0;
                            break;
                        case EnumInstructions.xor:
                            if (currentTokenLine.Tokens.Length != 3)
                                throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Not enough tokens in RRMOV instruction.");

                            filestream.Write((byte)(96 + 3));

                            if (!currentTokenLine.Tokens[1].GetProperty(EnumTokenProperties.RegisterNumber, out rA) || currentTokenLine.Tokens[1].TokenType != EnumTokenTypes.Register)
                                throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Token is not expected immediate value.");

                            if (!currentTokenLine.Tokens[2].GetProperty(EnumTokenProperties.RegisterNumber, out rB) || currentTokenLine.Tokens[2].TokenType != EnumTokenTypes.Register)
                                throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Token is not expected immediate value.");

                            tmpInt = rA << 4;
                            tmpInt = tmpInt | rB;
                            filestream.Write((byte)tmpInt);
                            tmpInt = 0;
                            break;
                        case EnumInstructions.imul:
                            if (currentTokenLine.Tokens.Length != 3)
                                throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Not enough tokens in RRMOV instruction.");

                            filestream.Write((byte)(96 + 4));

                            if (!currentTokenLine.Tokens[1].GetProperty(EnumTokenProperties.RegisterNumber, out rA) || currentTokenLine.Tokens[1].TokenType != EnumTokenTypes.Register)
                                throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Token is not expected immediate value.");

                            if (!currentTokenLine.Tokens[2].GetProperty(EnumTokenProperties.RegisterNumber, out rB) || currentTokenLine.Tokens[2].TokenType != EnumTokenTypes.Register)
                                throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Token is not expected immediate value.");

                            tmpInt = rA << 4;
                            tmpInt = tmpInt | rB;
                            filestream.Write((byte)tmpInt);
                            tmpInt = 0;
                            break;
                        case EnumInstructions.jmp:
                            if (currentTokenLine.Tokens.Length != 2)
                                throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Not enough tokens in RRMOV instruction.");

                            filestream.Write((byte)112);

                            if (!currentTokenLine.Tokens[1].GetProperty(EnumTokenProperties.ImmediateValue, out someProperty) || currentTokenLine.Tokens[1].TokenType != EnumTokenTypes.Immediate)
                                throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Token is not expected immediate value.");

                            someBytes = BitConverter.GetBytes(someProperty);
                            WriteBytes(someBytes, filestream);
                            break;
                        case EnumInstructions.jle:
                        case EnumInstructions.jl:
                        case EnumInstructions.je:
                        case EnumInstructions.call:
                        case EnumInstructions.ret:
                        case EnumInstructions.push:
                        case EnumInstructions.pop:
                        default:
#if DEBUG

#else
                            throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "File writer does not support this instruction yet.");
#endif
                            break;
                    }

                }
            }
        }

        private void WriteBytes(byte[] bytes, BinaryWriter bw)
        {
            for(int i = bytes.Length - 1; i >= 0 ; i--)
            {
                //Add file access exception
                bw.Write(bytes[i]);
            }
        }


        ~BinaryFileWriter()
        {
            
        }
    }
}
