using System;

namespace YAS
{
    class Program
    {

        static void Main(string[] args)
        {
            Parser YParser = new Parser();
            YParser.ParseString("addq %rax, %rcx");
        }
    }
}
