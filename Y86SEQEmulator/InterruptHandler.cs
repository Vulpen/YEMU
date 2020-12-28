using System;
using System.Collections.Generic;
using System.Text;

namespace Y86SEQEmulator
{

    public delegate void InterruptDelegate();
    public static class InterruptHandler
    {
        public static Dictionary<UInt32, Action> InterruptMapping
                = new Dictionary<UInt32, Action>
        {
            { 0, () => { Test_Fail(); } },
            { 1, () => { Test_Success(); } }
        };


        private static void Test_Fail()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Interrupt: TEST_FAIL");
            Console.ResetColor();
        }

        private static void Test_Success()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Interrupt: TEST_SUCCESS");
            Console.ResetColor();
        }
    }
}
