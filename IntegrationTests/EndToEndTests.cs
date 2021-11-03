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
        public void TestExampleOne()
        {
            Assembler myAssembler = new Assembler();
            string workingDirectory = Directory.GetCurrentDirectory();
            Console.WriteLine(workingDirectory);

            Assert.IsTrue(myAssembler.AssembleFile(workingDirectory + @"\Examples\ex1.yas", workingDirectory + @"\Examples\ex1.yin"));
        }

    }
}
