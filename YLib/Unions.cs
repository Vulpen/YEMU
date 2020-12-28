using System;
using System.Runtime.InteropServices;

namespace YLib
{

    [StructLayout(LayoutKind.Explicit)]
    public struct Int64Union
    {
        [FieldOffset(0)]
        public Int64 signed;
        [FieldOffset(0)]
        public UInt64 unsigned;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct Int32Union
    {
        [FieldOffset(0)]
        public Int32 signed;
        [FieldOffset(0)]
        public UInt32 unsigned;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct Int16Union
    {
        [FieldOffset(0)]
        public Int16 signed;
        [FieldOffset(0)]
        public UInt16 unsigned;
    }

}