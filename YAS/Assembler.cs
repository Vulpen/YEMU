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
        /// <param name="sourceFilePath"></param>
        /// <param name="destFilePath"></param>
        /// <returns></returns>
        public void AssembleFile(string sourceFilePath, string destFilePath)
        {
            var firstPassSuccess = false;
            var secondPassSuccess = false;

            if (!File.Exists(sourceFilePath))
            {
                Console.WriteLine($"Could not find source file at {sourceFilePath}");
                return;
            }

            using (var readStream = new StreamReader(sourceFilePath))
            {
                FirstPass(readStream, out firstPassSuccess);
            }

            using (var binaryStream = new BinaryWriter(File.Open(destFilePath, FileMode.OpenOrCreate)))
            {
                SecondPass(binaryStream, out secondPassSuccess);
            }

            if (firstPassSuccess && secondPassSuccess)
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
        }

        private bool Preprocessor() { return true; }

        /// <summary>
        /// Lexing aka Tokenizing and parsing the assembly.
        /// </summary>
        private void FirstPass(StreamReader readStream, out bool success) 
        {
            var lineNumber = 0;
            var currentLine = "";
            Token[] currentTokens;
            Console.WriteLine("Lexical analysis and parsing...");
            
            while (!readStream.EndOfStream)
            {
                success = false;
                currentLine = readStream.ReadLine();
                currentLine = currentLine.Trim();
                lineNumber += 1;

                if(string.IsNullOrEmpty(currentLine)) continue;
                
                try
                {
                    currentTokens = YParser.ParseString(currentLine, out success);
                    if (currentTokens?.Length == 0) continue;
                    YFile.AddLine(currentTokens);
                    return;
                }
                catch (FoundUnexpectedToken e)
                {
                    if (e.token1 != null)
                    {
                        Console.WriteLine($"ERROR : Unexpected Token {e.token1.Text} On Line : {lineNumber} | {currentLine = ""}");
                    }
                    else
                    {
                        Console.WriteLine($"ERROR : Unexpected Token On Line : {lineNumber} | {currentLine = ""}");
                    }
                }
                catch (AssemblerException e)
                {
                    success = false;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ERROR at stage : " + Enum.GetName(typeof(EnumAssemblerStages), e._stage) + " " + e.Message);
                    return;
                }
                catch (TokenAccessException e)
                {
                    success = false;
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
                    return;
                }
                catch (Exception e)
                {
                    success = false;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(e.Message);
                    return;
                }
                finally
                {
                    Console.ResetColor();
                }
            }
            success = true;
        }

        /// <summary>
        /// Resolving labels in file and writing to binary file.
        /// </summary>
        /// <param name="destFilePath"></param>
        /// <returns></returns>
        private void SecondPass(BinaryWriter binaryStream, out bool success) 
        {
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
                success = false;
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

                return;
            }
            catch (FoundUnexpectedToken e)
            {
                success = false;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
            }
            finally
            {
                Console.ResetColor();
            }

            success = true;
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
