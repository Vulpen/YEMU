using System;
using System.Collections.Generic;
using System.IO;
using YLib;

namespace YAS
{
    class Program
    {
        private const string _path = @"./Examples/drawPixel.yas";
        private const string _binPath = @"./Examples/drawPixel.yin";

        /// <summary>
        /// Calls assemble file, will handle arguments in the future.
        /// </summary>
        /// <param name="args"></param>
        private static void Main(string[] args)
        {
            //When made into a CLI, handle arguments here
            var Y86Assembler = new Assembler();
            Y86Assembler.AssembleFile(_path, _binPath);
        }
    }
}
