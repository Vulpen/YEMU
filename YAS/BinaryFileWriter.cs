using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using YLib;

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
        public void WriteToFile(TokenFile tokenFile, BinaryWriter filestream)
        {
            //TODO Create Wrapper functions for movs, jmps, etc.

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
                        //0x00
                        filestream.Write((byte)0);
                        break;
                    case EnumInstructions.nop:
                        //0x10
                        filestream.Write((byte)16);
                        break;
                    case EnumInstructions.rrmov:
                        //0x20<rA><rB>
                        if (currentTokenLine.Tokens.Length != 3)
                            throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Not enough tokens in RRMOV instruction.");

                        filestream.Write((byte)32);//0x20

                        if (!currentTokenLine.Tokens[1].GetProperty(EnumTokenProperties.RegisterNumber, out rA))
                            throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Token does not contain register code.");

                        if (!currentTokenLine.Tokens[2].GetProperty(EnumTokenProperties.RegisterNumber, out rB) || currentTokenLine.Tokens[2].TokenType != EnumTokenTypes.Register)
                            throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Token does not contain register code.");

                        tmpInt = rA << 4;
                        tmpInt = tmpInt | rB;
                        filestream.Write((byte)tmpInt);//0x<rA><rB>
                        tmpInt = 0;
                        break;
                    case EnumInstructions.irmov:
                        //0x30<F><rB><V>
                        if (currentTokenLine.Tokens.Length != 3)
                            throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Not enough tokens in IRMOV instruction.");

                        filestream.Write((byte)48);

                        if (!currentTokenLine.Tokens[1].GetProperty(EnumTokenProperties.ImmediateValue, out someProperty) || currentTokenLine.Tokens[1].TokenType != EnumTokenTypes.Immediate)
                            throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Token is not expected immediate value.");

                        if (!currentTokenLine.Tokens[2].GetProperty(EnumTokenProperties.RegisterNumber, out rB) || currentTokenLine.Tokens[2].TokenType != EnumTokenTypes.Register)
                            throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Token is not expected address register.");

                        filestream.Write((byte)(rB + 240));//Write byte F,rB
                        someBytes = BitConverter.GetBytes((uint)someProperty);
                        WriteBytes(someBytes, filestream);
                        break;
                    case EnumInstructions.rmmov:
                        //0x40<rA><rB><D> Where D is offset to address in register B (rB).
                        if (currentTokenLine.Tokens.Length != 3)
                            throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Not enough tokens in RMMOV instruction.");

                        filestream.Write((byte)64);

                        if (!currentTokenLine.Tokens[1].GetProperty(EnumTokenProperties.RegisterNumber, out rA) || currentTokenLine.Tokens[1].TokenType != EnumTokenTypes.Register)
                            throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Token is not expected register value.");

                        if (!currentTokenLine.Tokens[2].GetProperty(EnumTokenProperties.RegisterNumber, out rB) || currentTokenLine.Tokens[2].TokenType != EnumTokenTypes.AddressRegister)
                            throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Token is not expected address register value.");

                        //someProperty represents the offset of destination memory address
                        if (!currentTokenLine.Tokens[2].GetProperty(EnumTokenProperties.ImmediateValue, out someProperty))
                            someProperty = 0;

                        tmpInt = rA << 4;
                        tmpInt = tmpInt | rB;
                        filestream.Write((byte)tmpInt);
                        tmpInt = 0;

                        someBytes = BitConverter.GetBytes((uint)someProperty);
                        WriteBytes(someBytes, filestream);
                        break;
                    case EnumInstructions.mrmov:
                        //0x50<rA><rB><D> Where D is offset to address in register A (rA).
                        if (currentTokenLine.Tokens.Length != 3)
                            throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Not enough tokens in MRMOV instruction.");

                        filestream.Write((byte)80);

                        if (!currentTokenLine.Tokens[1].GetProperty(EnumTokenProperties.RegisterNumber, out rA) || currentTokenLine.Tokens[1].TokenType != EnumTokenTypes.AddressRegister)
                            throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Token is not expected address register value.");

                        if (!currentTokenLine.Tokens[2].GetProperty(EnumTokenProperties.RegisterNumber, out rB) || currentTokenLine.Tokens[2].TokenType != EnumTokenTypes.Register)
                            throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Token is not expected immediate value.");

                        //someProperty represents the offset of destination memory address
                        if (!currentTokenLine.Tokens[1].GetProperty(EnumTokenProperties.ImmediateValue, out someProperty))
                            someProperty = 0;

                        tmpInt = rA << 4;
                        tmpInt = tmpInt | rB;
                        filestream.Write((byte)tmpInt);
                        tmpInt = 0;

                        someBytes = BitConverter.GetBytes((uint)someProperty);
                        WriteBytes(someBytes, filestream);
                        break;
                    case EnumInstructions.add:
                        if (currentTokenLine.Tokens.Length != 3)
                            throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Not enough tokens in ADD instruction.");

                        filestream.Write((byte)96);

                        if (!currentTokenLine.Tokens[1].GetProperty(EnumTokenProperties.RegisterNumber, out rA) || currentTokenLine.Tokens[1].TokenType != EnumTokenTypes.Register)
                            throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Token is not expected register value.");

                        if (!currentTokenLine.Tokens[2].GetProperty(EnumTokenProperties.RegisterNumber, out rB) || currentTokenLine.Tokens[2].TokenType != EnumTokenTypes.Register)
                            throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Token is not expected register value.");

                        tmpInt = rA << 4;
                        tmpInt = tmpInt | rB;
                        filestream.Write((byte)tmpInt);
                        tmpInt = 0;
                        break;
                    case EnumInstructions.sub:
                        if (currentTokenLine.Tokens.Length != 3)
                            throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Not enough tokens in SUB instruction.");

                        filestream.Write((byte)(96 + 1));

                        if (!currentTokenLine.Tokens[1].GetProperty(EnumTokenProperties.RegisterNumber, out rA) || currentTokenLine.Tokens[1].TokenType != EnumTokenTypes.Register)
                            throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Token is not expected register value.");

                        if (!currentTokenLine.Tokens[2].GetProperty(EnumTokenProperties.RegisterNumber, out rB) || currentTokenLine.Tokens[2].TokenType != EnumTokenTypes.Register)
                            throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Token is not expected register value.");

                        tmpInt = rA << 4;
                        tmpInt = tmpInt | rB;
                        filestream.Write((byte)tmpInt);
                        tmpInt = 0;
                        break;
                    case EnumInstructions.and:
                        if (currentTokenLine.Tokens.Length != 3)
                            throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Not enough tokens in AND instruction.");

                        filestream.Write((byte)(96 + 2));

                        if (!currentTokenLine.Tokens[1].GetProperty(EnumTokenProperties.RegisterNumber, out rA) || currentTokenLine.Tokens[1].TokenType != EnumTokenTypes.Register)
                            throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Token is not expected register value.");

                        if (!currentTokenLine.Tokens[2].GetProperty(EnumTokenProperties.RegisterNumber, out rB) || currentTokenLine.Tokens[2].TokenType != EnumTokenTypes.Register)
                            throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Token is not expected register value.");

                        tmpInt = rA << 4;
                        tmpInt = tmpInt | rB;
                        filestream.Write((byte)tmpInt);
                        tmpInt = 0;
                        break;
                    case EnumInstructions.xor:
                        if (currentTokenLine.Tokens.Length != 3)
                            throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Not enough tokens in XOR instruction.");

                        filestream.Write((byte)(96 + 3));

                        if (!currentTokenLine.Tokens[1].GetProperty(EnumTokenProperties.RegisterNumber, out rA) || currentTokenLine.Tokens[1].TokenType != EnumTokenTypes.Register)
                            throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Token is not expected register value.");

                        if (!currentTokenLine.Tokens[2].GetProperty(EnumTokenProperties.RegisterNumber, out rB) || currentTokenLine.Tokens[2].TokenType != EnumTokenTypes.Register)
                            throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Token is not expected register value.");

                        tmpInt = rA << 4;
                        tmpInt = tmpInt | rB;
                        filestream.Write((byte)tmpInt);
                        tmpInt = 0;
                        break;
                    case EnumInstructions.imul:
                        if (currentTokenLine.Tokens.Length != 3)
                            throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Not enough tokens in IMUL instruction.");

                        filestream.Write((byte)(96 + 4));

                        if (!currentTokenLine.Tokens[1].GetProperty(EnumTokenProperties.RegisterNumber, out rA) || currentTokenLine.Tokens[1].TokenType != EnumTokenTypes.Register)
                            throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Token is not expected register value.");

                        if (!currentTokenLine.Tokens[2].GetProperty(EnumTokenProperties.RegisterNumber, out rB) || currentTokenLine.Tokens[2].TokenType != EnumTokenTypes.Register)
                            throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Token is not expected register value.");

                        tmpInt = rA << 4;
                        tmpInt = tmpInt | rB;
                        filestream.Write((byte)tmpInt);
                        tmpInt = 0;
                        break;
                    case EnumInstructions.jmp:
                        //0x7<fn><Dest>
                        if (currentTokenLine.Tokens.Length != 2)
                            throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Not enough tokens in JMP instruction.");

                        filestream.Write((byte)112);

                        if (!currentTokenLine.Tokens[1].GetProperty(EnumTokenProperties.ImmediateValue, out someProperty) || currentTokenLine.Tokens[1].TokenType != EnumTokenTypes.Immediate)
                            throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Token is not expected immediate value.");

                        someBytes = BitConverter.GetBytes((uint)someProperty);
                        WriteBytes(someBytes, filestream);
                        break;
                    case EnumInstructions.jle:
                        if (currentTokenLine.Tokens.Length != 2)
                            throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Not enough tokens in JLE instruction.");

                        filestream.Write((byte)(112 + 1));

                        if (!currentTokenLine.Tokens[1].GetProperty(EnumTokenProperties.ImmediateValue, out someProperty) || currentTokenLine.Tokens[1].TokenType != EnumTokenTypes.Immediate)
                            throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Token is not expected immediate value.");

                        someBytes = BitConverter.GetBytes((uint)someProperty);
                        WriteBytes(someBytes, filestream);
                        break;
                    case EnumInstructions.jl:
                        if (currentTokenLine.Tokens.Length != 2)
                            throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Not enough tokens in JL instruction.");

                        filestream.Write((byte)(112 + 2));

                        if (!currentTokenLine.Tokens[1].GetProperty(EnumTokenProperties.ImmediateValue, out someProperty) || currentTokenLine.Tokens[1].TokenType != EnumTokenTypes.Immediate)
                            throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Token is not expected immediate value.");

                        someBytes = BitConverter.GetBytes((uint)someProperty);
                        WriteBytes(someBytes, filestream);
                        break;
                    case EnumInstructions.je:
                        if (currentTokenLine.Tokens.Length != 2)
                            throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Not enough tokens in JE instruction.");

                        filestream.Write((byte)(112 + 3));

                        if (!currentTokenLine.Tokens[1].GetProperty(EnumTokenProperties.ImmediateValue, out someProperty) || currentTokenLine.Tokens[1].TokenType != EnumTokenTypes.Immediate)
                            throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Token is not expected immediate value.");

                        someBytes = BitConverter.GetBytes((uint)someProperty);
                        WriteBytes(someBytes, filestream);
                        break;
                    case EnumInstructions.jne:
                        if (currentTokenLine.Tokens.Length != 2)
                            throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Not enough tokens in JNE instruction.");

                        filestream.Write((byte)(112 + 4));

                        if (!currentTokenLine.Tokens[1].GetProperty(EnumTokenProperties.ImmediateValue, out someProperty) || currentTokenLine.Tokens[1].TokenType != EnumTokenTypes.Immediate)
                            throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Token is not expected immediate value.");

                        someBytes = BitConverter.GetBytes((uint)someProperty);
                        WriteBytes(someBytes, filestream);
                        break;
                    case EnumInstructions.jge:
                        if (currentTokenLine.Tokens.Length != 2)
                            throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Not enough tokens in JGE instruction.");

                        filestream.Write((byte)(112 + 5));

                        if (!currentTokenLine.Tokens[1].GetProperty(EnumTokenProperties.ImmediateValue, out someProperty) || currentTokenLine.Tokens[1].TokenType != EnumTokenTypes.Immediate)
                            throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Token is not expected immediate value.");

                        someBytes = BitConverter.GetBytes((uint)someProperty);
                        WriteBytes(someBytes, filestream);
                        break;
                    case EnumInstructions.jg:
                        if (currentTokenLine.Tokens.Length != 2)
                            throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Not enough tokens in JG instruction.");

                        filestream.Write((byte)(112 + 6));

                        if (!currentTokenLine.Tokens[1].GetProperty(EnumTokenProperties.ImmediateValue, out someProperty) || currentTokenLine.Tokens[1].TokenType != EnumTokenTypes.Immediate)
                            throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Token is not expected immediate value.");

                        someBytes = BitConverter.GetBytes((uint)someProperty);
                        WriteBytes(someBytes, filestream);
                        break;
                    case EnumInstructions.call:
                        //0x80<Dest>
                        if (currentTokenLine.Tokens.Length != 2)
                            throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Not enough tokens in CALL instruction.");

                        filestream.Write((byte)128);

                        if (!currentTokenLine.Tokens[1].GetProperty(EnumTokenProperties.ImmediateValue, out someProperty) || currentTokenLine.Tokens[1].TokenType != EnumTokenTypes.Immediate)
                            throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Token is not expected immediate value.");

                        someBytes = BitConverter.GetBytes((uint)someProperty);
                        WriteBytes(someBytes, filestream);
                        break;
                    case EnumInstructions.ret:
                        if (currentTokenLine.Tokens.Length != 1)
                            throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Not enough tokens in RET instruction.");

                        filestream.Write((byte)144);
                        break;
                    case EnumInstructions.interrupt:
                        if (currentTokenLine.Tokens.Length != 2)
                            throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Not enough tokens in Interrupt instruction.");

                        filestream.Write((byte)240);

                        if (!currentTokenLine.Tokens[1].GetProperty(EnumTokenProperties.ImmediateValue, out someProperty) || currentTokenLine.Tokens[1].TokenType != EnumTokenTypes.Immediate)
                            throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Token is not expected immediate value.");

                        someBytes = BitConverter.GetBytes((uint)someProperty);
                        WriteBytes(someBytes, filestream);
                        break;
                    case EnumInstructions.push:
                    case EnumInstructions.pop:
                        throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "Push/Pop instructions not implemented!");
                    default:
#if DEBUG

#else
                        throw new AssemblerException(EnumAssemblerStages.BinaryWriter, "File writer does not support this instruction yet.");
#endif
                        break;
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
