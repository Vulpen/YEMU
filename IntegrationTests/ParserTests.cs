using Microsoft.VisualStudio.TestTools.UnitTesting;
using YLib;
using YAS;
using System.Collections.Generic;
using System;

namespace IntegrationTests
{
    /**
     * These tests cover the first pass at parsing individual lines of Y86 assembly.
     * Not able to resolve labels at this stage.
     */
    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        public void EmptyLine()
        {
            Parser testParser = new Parser(EnumVerboseLevels.All);
            Token[] actualTokens = testParser.ParseString("");

            Assert.IsTrue(actualTokens.Length == 0);
        }

        [TestMethod]
        public void SpacesLine()
        {
            Parser testParser = new Parser(EnumVerboseLevels.All);
            Token[] actualTokens = testParser.ParseString("         ");

            Assert.IsTrue(actualTokens.Length == 0);
        }

        [TestMethod]
        public void Interrupt()
        {
            Parser testParser = new Parser(EnumVerboseLevels.All);
            List<Token> expectedtokens = new List<Token>();
            expectedtokens.Add(new Token((Int64)EnumTokenTypes.Instruction, (Int64)EnumInstructions.interrupt, (Int64)EnumInstructionSizes.Long, "int"));
            Token immediateToken = new Token((Int64)EnumTokenTypes.Immediate, "$3");
            immediateToken.AddProperty(EnumTokenProperties.ImmediateValue, 3);
            expectedtokens.Add(immediateToken);
            Token[] actualTokens = testParser.ParseString("int $3");

            foreach (Token tkn in expectedtokens)
            {
                Assert.IsTrue(Array.IndexOf(actualTokens, tkn) > -1);
            }
        }

        [TestMethod]
        public void LabelDeclaration()
        {
            Parser testParser = new Parser(EnumVerboseLevels.All);

            List<Token> expectedtokens = new List<Token>();
            expectedtokens.Add(new Token((Int64)EnumTokenTypes.Label, "myLabel:"));

            Token[] actualTokens = testParser.ParseString("myLabel:");

            Assert.AreEqual(expectedtokens[0], actualTokens[0]);
        }
    }
}
