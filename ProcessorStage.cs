using System;
using System.Collections.Generic;
using System.Text;

namespace Y86Emulator
{
    interface ProcessorStage
    {
        public abstract bool Execute();

        public abstract bool Reset();
    }


    class DecodeStage : ProcessorStage
    {
        //DRegister
        DRegister InputRegisters;
        //ERegister
        ERegister OutputRegisters;
        RegisterFile registerFile;

        public DecodeStage(DRegister input, ERegister output, RegisterFile regFile)
        {
            InputRegisters = input;
            OutputRegisters = output;
            registerFile = regFile;
        }
        public bool Execute()
        {
            OutputRegisters.ClearRegisters();
            switch (InputRegisters.ReadRegister((int)DRegisters.icode))
            {
                case (0):
                    //halt
                    throw new Exception("Halt Op");
                    break;
                case (1):
                    //nop
                    OutputRegisters.WriteRegister(1, (int)ERegisters.icode);
                    return true;
                    break;
                case (2):
                    //rrmovq rA, rB
                    OutputRegisters.WriteRegister(2, (int)ERegisters.icode);
                    OutputRegisters.WriteRegister(0, (int)ERegisters.ifun);
                    
                    OutputRegisters.WriteRegister(registerFile.ReadRegister((int)InputRegisters.ReadRegister((int)DRegisters.rA)), (int)ERegisters.valA);
                    OutputRegisters.WriteRegister((int)InputRegisters.ReadRegister((int)DRegisters.rA), (int)ERegisters.srcA);
                    return false;
                case (3):
                    //irmovq
                    return false;
                case (4):
                    //rmmovq
                    return false;
                case (5):
                    //mrmovq
                    return false;
                case (6):
                    //OPq
                    return false;
                case (7):
                    //jXX 
                    return false;
                case (8):
                    //call Dest
                    return false;
                case (9):
                    //ret
                    return false;
                case (10):
                    //pushq rA
                    return false;
                case (11):
                    //popq rA
                    return false;
            }
            return false;
        }

        public bool Reset()
        {
            throw new NotImplementedException();
        }
    }
}
