using System;
using System.Collections.Generic;
using System.Text;
using YLib;

namespace Y86SEQEmulator
{
    class Processor
    {
        //Registers:
        UInt64[] registers;
        uint PC;

        byte icode, ifun, rA, rB;
        uint valC, valP, valE, valM; // valC - Constant read in from Fetch. valP - PC guess. valE - result of Execute. valM - result of Memory.
        uint valA, valB; // Values inside register A and register B
        EnumInstructions instruction;
        bool[] flags;


        Memory MainMemory;

        byte[] buffer;

        public Processor()
        {
            registers = new UInt64[15];//Corresponds to the 15 registers of the 64-bit Y86 architecture
            MainMemory = new Memory();
            buffer = new byte[8];
            flags = new bool[3];
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
                    valP = 5;
                    valC = MainMemory.ReadLong(PC + 1);
                    break;
            }

            //Decode
            valA = (rB <= 14) ? (UInt32)registers[rA] : 0;
            valB = (rB <= 14) ? (UInt32)registers[rB] : 0;

            //Execute
            switch(instruction)
            {
                case EnumInstructions.add:
                    valE = valB + valA;
                    flags[(int)EnumConditionCodes.CF] = (valB > UInt32.MaxValue - valA) ? true : false;
                    break;
                case EnumInstructions.sub:
                    valE = valB - valA;
                    flags[(int)EnumConditionCodes.CF] = (valB < valA) ? true : false;
                    break;
                case EnumInstructions.and:
                    valE = valB & valA;
                    break;
                case EnumInstructions.xor:
                    break;
                case EnumInstructions.imul:
                    valE = valB * valA;
                    break;
                case EnumInstructions.irmov:
                    valE = valC;
                    break;
                case EnumInstructions.rrmov:
                    valE = valA;
                    break;
                case EnumInstructions.rmmov:
                    valE = valB + valC;     //valE holds the address
                    break;
                case EnumInstructions.mrmov:
                    valE = valA + valC;     //rA and rB reversed from example in book pg.389
                    break;
                default:
                    break;
            }

            //Memory
            if(instruction == EnumInstructions.rmmov)
            {
                valM = MainMemory.ReadLong(valE);
            }else if(instruction == EnumInstructions.mrmov)
            {
                MainMemory.WriteByte(valE, (byte)valA);
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
                    registers[rB] = valE;
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
                default:
                    PC += valP;
                    valP = 0;
                    break;
            }
            
        }

    }
}
