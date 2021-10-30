using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YLib;

namespace YAS
{
    class Program
    {
        static string PATH = @".\Examples\drawPixel.yas";
        static string BIN_PATH = @".\Examples\drawPixel.yin";

        static void Main(string[] args)
        {
            //When made into a CLI, handle arguments here
            string assemblyPath = PATH;
            string binaryPath = BIN_PATH;
   
            if(args.Any(str => str == "-i"))
            {
                // Set the input file from this path.
                assemblyPath = args.SkipWhile(str => str != "-i").ElementAt(1);
            }

            if (args.Any(str => str == "-o"))
            {
                // Set the input file from this path.
                binaryPath = args.SkipWhile(str => str != "-o").ElementAt(1);
            }

            if(!File.Exists(assemblyPath))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Not able to open source file: {assemblyPath}");
                Console.ResetColor();
            }

            Assembler Y86Assembler = new Assembler();
            Y86Assembler.AssembleFile(assemblyPath, binaryPath);
            return;
        }
    }
}
