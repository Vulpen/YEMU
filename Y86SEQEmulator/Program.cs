using System;
using YLib;

namespace Y86SEQEmulator
{
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
        }
    }
}
