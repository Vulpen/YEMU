using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using YLib;

namespace YAS
{
    class Assembler
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
            bool FirstPassSuccessful = true;
            bool SecondPassSuccessful = true;

            PreprocessorSuccessful = Preprocessor();

            using (StreamReader readStream = new StreamReader(sourceFilePath))
            {
                FirstPassSuccessful = FirstPass(readStream);
            }

            using (BinaryWriter binaryStream = new BinaryWriter(File.Open(destFilePath, FileMode.OpenOrCreate)))
            {
                SecondPassSuccessful = SecondPass(binaryStream);
            }

            if (PreprocessorSuccessful && FirstPassSuccessful && SecondPassSuccessful)
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

            return PreprocessorSuccessful && FirstPassSuccessful && SecondPassSuccessful;
        }

        private bool Preprocessor() { return true; }

        /// <summary>
        /// Lexing aka Tokenizing and parsing the assembly.
        /// </summary>
        private bool FirstPass(StreamReader readStream) 
        {
            bool BuildSuccessful = true;
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

                try
                {
                    //FIRST PASS
                    currentTokens = YParser.ParseString(CurrentLine);
                    if (currentTokens == null || currentTokens.Length == 0)
                        continue;
                    YFile.AddLine(currentTokens);

                }
                catch (FoundUnexpectedToken e)
                {
                    //Console.WriteLine("ERROR : Unexpected Token " + e.token1.Text + " On Line : " + LineNumber + "|" + CurrentLine);
                    if (e.token1 != null)
                    {
                        Console.WriteLine($"ERROR : Unexpected Token {e.token1.Text} On Line : {LineNumber} | {CurrentLine}");
                    }
                    else
                    {
                        Console.WriteLine($"ERROR : Unexpected Token On Line : {LineNumber} | {CurrentLine}");
                    }
                    BuildSuccessful = false;
                }
                catch (AssemblerException e)
                {

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ERROR at stage : " + Enum.GetName(typeof(EnumAssemblerStages), e._stage) + " " + e.Message);
                    BuildSuccessful = false;
                }
                catch (TokenAccessException e)
                {
                    BuildSuccessful = false;

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
                }
                catch (Exception e)
                {
                    BuildSuccessful = false;

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    Console.ResetColor();
                }
            }

            return BuildSuccessful;
        }

        /// <summary>
        /// Resolving labels in file and writing to binary file.
        /// </summary>
        /// <param name="destFilePath"></param>
        /// <returns></returns>
        private bool SecondPass(BinaryWriter binaryStream) 
        {
            bool BuildSuccessful = true;
            try
            {
                //SECOND PASS
                Console.WriteLine("Second pass...");
                YFile.ResolveLabels();
                Console.WriteLine("Writing to file...");
                YFileWriter.WriteToFile(YFile, binaryStream);
            }
            catch (TokenAccessException e)
            {
                BuildSuccessful = false;

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
            }
            catch (FoundUnexpectedToken e)
            {
                BuildSuccessful = false;

                Console.ForegroundColor = ConsoleColor.Red;

                if(e.token1 != null)
                {
                    Console.WriteLine($"Found unexpected token: {e.Message}");
                    Console.WriteLine(e.token1.TokenInfoString());
                }
                else
                {
                    Console.WriteLine($"Found unexpected token: {e.Message}");
                }
            }
            finally
            {
                Console.ResetColor();
            }

            return BuildSuccessful;
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
