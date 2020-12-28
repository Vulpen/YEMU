using System;
using System.Runtime.InteropServices;
using YLib;

namespace Y86SEQEmulator
{
    [StructLayout(LayoutKind.Explicit)]
    public struct Int32Union
    {
        [FieldOffset(0)]
        public Int32 signed;
        [FieldOffset(0)]
        public UInt32 unsigned;
    }
    class Program
    {
        static void Main(string[] args)
        {
            Processor YProcessor = new Processor();


            for(int i = 0; i < 50; i++)
            {
                YProcessor.Tick();
                Console.WriteLine(Enum.GetName(typeof(EnumInstructions), YProcessor.GetCurrentInstruction()));
            }
            return;
        }
    }
}
