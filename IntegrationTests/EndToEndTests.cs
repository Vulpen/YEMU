using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using YAS;

namespace IntegrationTests
{
    [TestClass]
    public class EndToEndTests
    {
        [TestMethod]
        public void ExampleOne()
        {
            Assembler myAssembler = new Assembler();
            string workingDirectory = Directory.GetCurrentDirectory();
            Console.WriteLine(workingDirectory);

            Assert.IsTrue(myAssembler.AssembleFile(workingDirectory + @"\Examples\ex1.yas", workingDirectory + @"\Examples\ex1.yin"));
        }

        [TestMethod]
        public void ExampleTwo()
        {
            Assembler myAssembler = new Assembler();
            string workingDirectory = Directory.GetCurrentDirectory();
            Console.WriteLine(workingDirectory);

            Assert.IsTrue(myAssembler.AssembleFile(workingDirectory + @"\Examples\ex2.yas", workingDirectory + @"\Examples\ex2.yin"));
        }

        [TestMethod]
        public void JumpTest()
        {
            Assembler myAssembler = new Assembler();
            string workingDirectory = Directory.GetCurrentDirectory();
            Console.WriteLine(workingDirectory);

            Assert.IsTrue(myAssembler.AssembleFile(workingDirectory + @"\Examples\jumpTest.yas", workingDirectory + @"\Examples\jumpTest.yin"));
        }

        [TestMethod]
        public void DrawPixel()
        {
            Assembler myAssembler = new Assembler();
            string workingDirectory = Directory.GetCurrentDirectory();
            Console.WriteLine(workingDirectory);

            Assert.IsTrue(myAssembler.AssembleFile(workingDirectory + @"\Examples\drawPixel.yas", workingDirectory + @"\Examples\drawPixel.yin"));
        }
    }
}
