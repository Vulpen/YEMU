using System;

namespace Y86SEQEmulator
{
    interface IRegister
    {
        Int64 ReadRegister(Int64 address);
        bool WriteRegister(Int64 address, Int64 value);
        Int64 ResolveRegisterAddress(string regName);
        bool Reset();
    }

    interface IStage
    {
        bool Execute();
        bool Reset();
    }

}
