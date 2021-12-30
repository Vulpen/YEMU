using System;
using System.IO;
using YLib;

namespace YAS
{
    public class Assembler
    {
        public Assembler()
        {
            InitializeAssembler();
        }

        /// <summary>
        /// Compiles lines of assembly from the source file into binary at the destfile.
        /// </summary>
        /// <returns></returns>
        public bool AssembleFile(string sourceFilePath, string destFilePath)
        {
            bool PreprocessorSuccessful = true;
            bool ParsingSuccessful = true;

            PreprocessorSuccessful = Preprocessor();
            try
            {
                using (StreamReader readStream = new StreamReader(sourceFilePath))
                {
                    FirstPass(readStream);
                }

                using (BinaryWriter binaryStream = new BinaryWriter(File.Open(destFilePath, FileMode.OpenOrCreate)))
                {
                    SecondPass(binaryStream);
                }
            }
            catch (TokenAccessException e)
            {

                Console.ForegroundColor = ConsoleColor.Red;
                if (e._Tkn != null)
                {
                    Console.WriteLine("ERROR accessing token: " + e.Message);
                    Console.WriteLine(e._Tkn.TokenInfoString());
                }
                else
                {
                    Console.WriteLine("ERROR accessing token: " + e.Message);
                }

                ParsingSuccessful = false;
            }
            catch (FoundUnexpectedToken e)
            {

                Console.ForegroundColor = ConsoleColor.Red;

                if (e.token1 != null)
                {
                    Console.WriteLine($"Found unexpected token: {e.Message}");
                    Console.WriteLine(e.token1.TokenInfoString());
                }
                else
                {
                    Console.WriteLine($"Found unexpected token: {e.Message}");
                }

                ParsingSuccessful = false;
            }
            catch (AssemblerException e)
            {

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR at stage : " + Enum.GetName(typeof(EnumAssemblerStages), e._stage) + " " + e.Message);

                ParsingSuccessful = false;
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(e.Message);

                ParsingSuccessful = false;
            }
            finally
            {
                Console.ResetColor();
            }

            if (PreprocessorSuccessful && ParsingSuccessful)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine(destFilePath + " successfully built");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("FAILED building: " + destFilePath);
                Console.ResetColor();
            }

            return PreprocessorSuccessful && ParsingSuccessful;
        }

        private bool Preprocessor() { return true; }

        /// <summary>
        /// Lexing aka Tokenizing and parsing the assembly.
        /// </summary>
        private void FirstPass(StreamReader readStream)
        {
            int LineNumber = 0;
            string CurrentLine;
            Token[] currentTokens;
            Console.WriteLine("Lexical analysis and parsing...");
            while (!readStream.EndOfStream)
            {
                CurrentLine = String.Empty;
                CurrentLine = readStream.ReadLine();
                CurrentLine.Trim();
                LineNumber += 1;

                if (CurrentLine == String.Empty)
                    continue;

                //FIRST PASS
                currentTokens = YParser.ParseString(CurrentLine);
                if (currentTokens == null || currentTokens.Length == 0)
                    continue;
                YFile.AddLine(currentTokens);

            }
        }

        /// <summary>
        /// Resolving labels in file and writing to binary file.
        /// </summary>
        /// <param name="destFilePath"></param>
        /// <returns></returns>
        private void SecondPass(BinaryWriter binaryStream)
        {
            //SECOND PASS
            Console.WriteLine("Second pass...");
            YFile.ResolveLabels();
            Console.WriteLine("Writing to file...");
            YFileWriter.WriteToFile(YFile, binaryStream);
        }

        private void InitializeAssembler()
        {
            YParser = new Parser(EnumVerboseLevels.All);
            YFile = new TokenFile();
            YFileWriter = new BinaryFileWriter();
        }

        private Parser YParser;
        private TokenFile YFile;
        private BinaryFileWriter YFileWriter;
    }
}
