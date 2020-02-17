using System;

namespace Y86SEQEmulator
{
    class SEQInternalRegisters : IRegister
    {
        public enum Registers
        {
            iCode = 0,
            iFun,
            rA,
            rB,
            valC,
            valP,
            valA,
            valB,
            dstE,
            dstM,
            srcA,
            srcB,
            Cnd,
            valE,
            Stat,
            valM,
            newPC,
            PC
        }
        private Int64[] DataArray;
        private const int NumRegisters = 18;

        public SEQInternalRegisters()
        {
            DataArray = new Int64[NumRegisters];
        }
        public long ReadRegister(long address)
        {
            if(address < NumRegisters)
            {
                return DataArray[address];
            }
            else
            {
                throw new Exception();
            }
        }

        public bool Reset()
        {
            throw new NotImplementedException();
        }

        public long ResolveRegisterAddress(string regName)
        {
            switch (regName.ToUpper())
            {
                case ("ICODE"):
                    break;
                case ("IFUN"):
                    break;
                case ("RA"):
                    break;
                case ("RB"):
                    break;
                case ("VALC"):
                    break;
                case ("VALP"):
                    break;
                case ("VALA"):
                    break;
                case ("VALB"):
                    break;
                case ("DSTE"):
                    break;
                case ("DSTM"):
                    break;
                case ("SRCA"):
                    break;
                case ("SRCB"):
                    break;
                case ("CND"):
                    break;
                case ("VALE"):
                    break;
                case ("STAT"):
                    break;
                case ("VALM"):
                    break;
                case ("NEWPC"):
                    break;
                case ("PC"):
                    break;
            }
            throw new NotImplementedException();
        }

        public bool WriteRegister(long address, long value)
        {
            throw new NotImplementedException();
        }
    }
}
