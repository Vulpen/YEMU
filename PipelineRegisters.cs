using System;
using System.Collections.Generic;
using System.Text;

namespace Y86Emulator
{
    interface PipelineRegisters
    {
        //InitializeRegisters
        //ClearRegisters
        //ReadRegister
        //WriteRegister
        public abstract void ClearRegisters();
        public abstract Int64 ReadRegister(int reg);
        public abstract bool WriteRegister(Int64 val, int reg);
    }

    enum RegisterFileRegisters
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
    class RegisterFile: PipelineRegisters
    {
        private static int NumberRegisters = 15;
        private Int64[] Registers;

        public void ClearRegisters()
        {
            throw new NotImplementedException();
        }

        public long ReadRegister(int reg)
        {
            throw new NotImplementedException();
        }

        public bool WriteRegister(long val, int reg)
        {
            throw new NotImplementedException();
        }
    }

    enum FRegisters
    {
    }
    class FRegister : PipelineRegisters
    {
        private static int NumberRegisters = 0;
        private Int64[] Registers;
        public void ClearRegisters()
        {
            throw new NotImplementedException();
        }

        public long ReadRegister(int reg)
        {
            if(reg < NumberRegisters)
            {
                return Registers[reg];
            }
            else
            {
                throw new Exception("Attempt to read invalid register");
            }
        }

        public bool WriteRegister(long val, int reg)
        {
            throw new NotImplementedException();
        }
    }



    enum DRegisters
    {
        stat = 0,
        icode,
        ifun,
        rA,
        rB,
        valC,
        valP
    }
    class DRegister : PipelineRegisters
    {
        private static int NumberRegisters = 7;
        private Int64[] Registers;

        public DRegister()
        {
            Registers = new long[NumberRegisters];
        }

        public void ClearRegisters()
        {
            for(int i = 0; i < NumberRegisters; i++)
            {
                Registers[i] = 0;
            }
        }

        public long ReadRegister(int reg)
        {
            if(reg < NumberRegisters)
            {
                return Registers[reg];
            }
            else
            {
                throw new Exception("Attempt to read invalid register on Decode Register");
            }
        }

        public bool WriteRegister(long val, int reg)
        {
            if (reg < NumberRegisters)
            {
                Registers[reg] = val;
                return true;
            }
            else
            {
                throw new Exception("Attempt to read invalid register on Decode Register");
            }
        }
    }

    enum ERegisters
    {
        stat = 0,
        icode,
        ifun,
        valC,
        valA,
        valB,
        dstE,
        dstM,
        srcA,
        srcB
    }
    class ERegister : PipelineRegisters
    {
        private static int NumberRegisters = 9;
        private Int64[] Registers;

        public ERegister()
        {
            Registers = new long[NumberRegisters];
        }

        public void ClearRegisters()
        {
            throw new NotImplementedException();
        }

        public long ReadRegister(int reg)
        {
            throw new NotImplementedException();
        }

        public bool WriteRegister(long val, int reg)
        {
            throw new NotImplementedException();
        }
    }
}
