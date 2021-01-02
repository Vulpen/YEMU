using System;
using System.Collections.Generic;
using System.IO;
using YLib;

namespace YAS
{
    class Program
    {
        //static string PATH = @"E:\[]ProgrammingProjects\C#\YEMU\YAS\Examples\ex1.yas";
        static string PATH = @"D:\[KEEP]ProgrammingProjects\C#\Y86Emulator\YAS\Examples\drawPixel.yas";
        static string BIN_PATH = @"D:\[KEEP]ProgrammingProjects\C#\Y86Emulator\YAS\Examples\drawPixel.yin";
        //static string BIN_PATH = @"E:\[]ProgrammingProjects\C#\YEMU\YAS\Examples\ex1.yin";
        static void Main(string[] args)
        {
            //When made into a CLI, handle arguments here
            Assembler Y86Assembler = new Assembler();
            Y86Assembler.AssembleFile(PATH, BIN_PATH);
            return;
        }
    }
}
