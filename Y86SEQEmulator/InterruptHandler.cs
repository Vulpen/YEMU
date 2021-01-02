using System;
using System.Collections.Generic;
using System.Text;

namespace Y86SEQEmulator
{

    public delegate void InterruptDelegate(Processor processor);
    public static class InterruptHandler
    {
        public static Dictionary<UInt32, InterruptDelegate> InterruptMapping
                = new Dictionary<UInt32, InterruptDelegate>
        {
            { 0, (Processor processor) => { Test_Fail(processor); } },
            { 1, (Processor processor) => { Test_Success(processor); } }
        };


        private static void Test_Fail(Processor processor)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Interrupt: TEST_FAIL");
            Console.ResetColor();
        }

        private static void Test_Success(Processor processor)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Interrupt: TEST_SUCCESS");
            Console.ResetColor();
        }
    }
}
