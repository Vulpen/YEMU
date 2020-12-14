using System;
using System.Collections.Generic;
using System.IO;
using YLib;

namespace YAS
{
    class Program
    {
        //static string PATH = @"E:\[]ProgrammingProjects\C#\YEMU\YAS\Examples\ex1.yas";
        static string PATH = @"D:\[KEEP]ProgrammingProjects\C#\Y86Emulator\YAS\Examples\ex1.yas";
        static string BIN_PATH = @"D:\[KEEP]ProgrammingProjects\C#\Y86Emulator\YAS\Examples\ex1.yin";
        //static string BIN_PATH = @"E:\[]ProgrammingProjects\C#\YEMU\YAS\Examples\ex1.yin";
        static void Main(string[] args)
        {
            //When made into 

            Assembler.AssembleFile(PATH, BIN_PATH);

        }
    }
}
