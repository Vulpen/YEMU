using System;
using System.Collections.Generic;
using System.Text;
using YLib;

namespace Y86SEQEmulator
{
    class Processor
    {
        //Registers:
        uint eax, ecx, edx, ebx, esi, edi, esp, ebp;
        uint PC;

        byte icode, ifun, rA, rB;
        uint valC, valP, valE; // valC - Constant read in from Fetch. valP - PC guess. valE - result of Execute.
        uint valA, valB; // Values inside register A and register B
        EnumInstructions instruction;


        Memory MainMemory;

        byte[] buffer;

        public Processor()
        {
            MainMemory = new Memory();
            buffer = new byte[8];
            PC = 0;
        }

        public EnumInstructions GetCurrentInstruction() { return instruction; }

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
                case EnumInstructions.rrmov:
                case EnumInstructions.add:
                case EnumInstructions.sub:
                case EnumInstructions.imul:
                case EnumInstructions.and:
                case EnumInstructions.xor:
                case EnumInstructions.push:
                case EnumInstructions.pop:
                    //Load rA,rB
                    valP = 2;
                    break;
                case EnumInstructions.jmp:
                case EnumInstructions.jle:
                case EnumInstructions.jl:
                case EnumInstructions.je:
                case EnumInstructions.jne:
                case EnumInstructions.jge:
                case EnumInstructions.jg:
                    valP = 5;
                    break;
            }

            //Decode

            //Execute

            //Memory

            //Write-Back

            //PC Update
            PC += valP;
            valP = 0;
        }

    }
}
