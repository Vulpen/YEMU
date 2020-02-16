using System;

namespace Y86SEQEmulator
{
    class SEQInternalRegisters : IRegister
    {
        public long ReadRegister(long address)
        {
            throw new NotImplementedException();
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
            }
            throw new NotImplementedException();
        }

        public bool WriteRegister(long address, long value)
        {
            throw new NotImplementedException();
        }
    }
}
