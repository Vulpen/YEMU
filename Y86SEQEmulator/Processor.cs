using System;
using System.Collections.Generic;
using System.Text;
using YLib;

namespace Y86SEQEmulator
{
    /// <summary>
    /// Representation of a Sequential Y86 Processor.
    /// </summary>
    public class Processor
    {
        // Properties -----
        public UInt64 R8 { get { return (registers[8]); } }
        public UInt64 R9 { get { return (registers[9]); } }
        public UInt64 R10 { get { return (registers[10]); } }

        public bool IsRunning { get; set; } = true;

        //Registers:
        UInt64[] registers;
        uint PC;

        byte icode, ifun, rA, rB;
        uint valC, valP, valM; // valC - Constant read in from Fetch. valP - PC guess. valE - result of Execute. valM - result of Memory.
        Int32Union valA, valB, valE; // Values inside register A and register B
        EnumInstructions instruction;
        bool[] flags;


        Memory MainMemory;

        byte[] buffer;

        public Processor()
        {
            registers = new UInt64[15];//Corresponds to the 15 registers of the 64-bit Y86 architecture
            MainMemory = new Memory();
            buffer = new byte[8];
            flags = new bool[4];
            PC = 0;
        }

        public EnumInstructions GetCurrentInstruction() { return instruction; }

        private void SetFlags(UInt32 value)
        {
            Int32 signed = BitConverter.ToInt32(BitConverter.GetBytes(value),0);


            flags[(int)EnumConditionCodes.ZF] = (value == 0) ? true : false;
            flags[(int)EnumConditionCodes.SF] = (signed < 0) ? true : false;
        }

        public void Tick()
        {
            //Fetch
            buffer[0] = MainMemory.ReadByte(PC);
            icode = (byte)(buffer[0] >> 4);
            ifun = (byte)(buffer[0] & 0x0F);
            instruction = EnumHelper.GetInstructions(icode, ifun);

            switch(instruction)
            {
                case EnumInstructions.halt:
                case EnumInstructions.nop:
                case EnumInstructions.ret:
                    valP = 1;
                    break;
                case EnumInstructions.irmov:
                case EnumInstructions.rmmov:
                case EnumInstructions.mrmov:
                    //Load rA,rB, and const val into valC
                    valP = 6;
                    buffer[0] = MainMemory.ReadByte(PC + 1);
                    rA = (byte)(buffer[0] >> 4);
                    rB = (byte)(buffer[0] & 0x0F);

                    valC = MainMemory.ReadLong(PC + 2);
                    break;
                case EnumInstructions.add:
                case EnumInstructions.sub:
                case EnumInstructions.imul:
                case EnumInstructions.and:
                case EnumInstructions.xor:
                case EnumInstructions.rrmov:
                case EnumInstructions.push:
                case EnumInstructions.pop:
                    //Load rA,rB
                    valP = 2;
                    buffer[0] = MainMemory.ReadByte(PC + 1);
                    rA = (byte)(buffer[0] >> 4);
                    rB = (byte)(buffer[0] & 0x0F);
                    break;
                case EnumInstructions.jmp:
                case EnumInstructions.jle:
                case EnumInstructions.jl:
                case EnumInstructions.je:
                case EnumInstructions.jne:
                case EnumInstructions.jge:
                case EnumInstructions.jg:
                case EnumInstructions.interrupt:
                    valP = 5;
                    valC = MainMemory.ReadLong(PC + 1);
                    break;
                
            }

            //Decode
            valA.unsigned = (rA <= 14) ? (UInt32)registers[rA] : 0;
            valB.unsigned = (rB <= 14) ? (UInt32)registers[rB] : 0;

            //Execute
            switch(instruction)
            {
                case EnumInstructions.add:
                    valE.unsigned = valB.unsigned + valA.unsigned;
                    flags[(int)EnumConditionCodes.CF] = (valB.unsigned > UInt32.MaxValue - valA.unsigned) ? true : false;

                    flags[(int)EnumConditionCodes.OF] = false;

                    if (valA.signed > 0 && valB.signed > 0 && valE.signed < 0) {
                        flags[(int)EnumConditionCodes.OF] = true;
                        break;
                    }

                    if (valA.signed < 0 && valB.signed < 0 && valE.signed > 0)
                    {
                        flags[(int)EnumConditionCodes.OF] = true;
                        break;
                    }


                    flags[(int)EnumConditionCodes.ZF] = (valE.unsigned == 0);
                    flags[(int)EnumConditionCodes.SF] = (valE.signed < 0);

                    break;
                case EnumInstructions.sub:
                    valE.unsigned = valA.unsigned - valB.unsigned;
                    flags[(int)EnumConditionCodes.CF] = (valA.unsigned < valB.unsigned) ? true : false;

                    flags[(int)EnumConditionCodes.OF] = false;

                    if (valB.signed < 0 && valA.signed > 0 && valE.signed < 0)
                    {
                        flags[(int)EnumConditionCodes.OF] = true;
                        break;
                    }

                    if (valB.signed > 0 && valA.signed < 0 && valE.signed > 0)
                    {
                        flags[(int)EnumConditionCodes.OF] = true;
                        break;
                    }

                    flags[(int)EnumConditionCodes.ZF] = (valE.unsigned == 0);
                    flags[(int)EnumConditionCodes.SF] = (valE.signed < 0);
                    break;
                case EnumInstructions.and:
                    valE.unsigned = valB.unsigned & valA.unsigned;
                    break;
                case EnumInstructions.xor:
                    break;
                case EnumInstructions.imul:
                    valE.unsigned = valB.unsigned * valA.unsigned;
                    break;
                case EnumInstructions.irmov:
                    valE.unsigned = valC;
                    break;
                case EnumInstructions.rrmov:
                    valE = valA;
                    break;
                case EnumInstructions.rmmov:
                    valE.unsigned = valB.unsigned + valC;     //valE holds the address
                    break;
                case EnumInstructions.mrmov:
                    valE.unsigned = valA.unsigned + valC;     //rA and rB reversed from example in book pg.389
                    break;
                case EnumInstructions.interrupt:
                    if (InterruptHandler.InterruptMapping.ContainsKey(valC)) { InterruptHandler.InterruptMapping[valC].Invoke(this); }
                    break;
                default:
                    break;
            }

            //Memory
            if(instruction == EnumInstructions.rmmov)
            {
                valM = MainMemory.ReadLong(valE.unsigned);
            }else if(instruction == EnumInstructions.mrmov)
            {
                MainMemory.WriteByte(valE.unsigned, (byte)valA.unsigned);
            }

            //Write-Back
            switch(instruction)
            {
                case EnumInstructions.add:
                case EnumInstructions.sub:
                case EnumInstructions.and:
                case EnumInstructions.xor:
                case EnumInstructions.imul:
                case EnumInstructions.irmov:
                case EnumInstructions.rrmov:
                    registers[rB] = valE.unsigned;
                    break;
                case EnumInstructions.mrmov:
                    registers[rB] = valM;   //rA and rB reversed from example in book pg.389
                    break;
            }

            //PC Update
            switch(instruction)
            {
                case EnumInstructions.jmp:
                    PC = valC;
                    break;
                case EnumInstructions.jle:
                    // (SF != OF) || ZF
                    PC = ((flags[(int)EnumConditionCodes.SF] != flags[(int)EnumConditionCodes.OF]) || flags[(int)EnumConditionCodes.ZF]) ? valC : PC+valP;
                    break;
                case EnumInstructions.jl:
                    // (SF != OF)
                    PC = (flags[(int)EnumConditionCodes.SF] != flags[(int)EnumConditionCodes.OF]) ? valC : PC + valP;
                    break;
                case EnumInstructions.je:
                    // ZF
                    PC = (flags[(int)EnumConditionCodes.ZF]) ? valC : PC + valP;
                    break;
                case EnumInstructions.jne:
                    // !ZF
                    PC = (!flags[(int)EnumConditionCodes.ZF]) ? valC : PC + valP;
                    break;
                case EnumInstructions.jge:
                    // !ZF && (SF = OF)
                    PC = ((flags[(int)EnumConditionCodes.SF] == flags[(int)EnumConditionCodes.OF])) ? valC : PC + valP;
                    break;
                case EnumInstructions.jg:
                    // !ZF && !SF
                    PC = (!flags[(int)EnumConditionCodes.ZF] && !flags[(int)EnumConditionCodes.SF]) ? valC : PC + valP;
                    break;
                default:
                    PC += valP;
                    valP = 0;
                    break;
            }
            
        }

    }
}
