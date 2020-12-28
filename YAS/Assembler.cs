using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using YLib;

namespace YAS
{
    class Assembler
    {

        /// <summary>
        /// Assembles lines of assembly from the source file to the destination binary file.
        /// </summary>
        /// <returns></returns>
        public static bool AssembleFile(string sourceFilePath, string destFilePath)
        {
            Parser YParser = new Parser(EnumVerboseLevels.All);
            TokenFile YFile = new TokenFile();
            BinaryFileWriter YFileWriter = new BinaryFileWriter();

            bool BuildSuccessful = true;

            if (!File.Exists(sourceFilePath))
            {
                Console.WriteLine("Could not find file at  " + sourceFilePath);
                return false;
            }

            int LineNumber = 0;

            using (StreamReader readStream = new StreamReader(sourceFilePath))
            {
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

                    //if verbose:
                    //Console.WriteLine("Parsing line: " + CurrentLine);
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

                try
                {
                    //SECOND PASS
                    Console.WriteLine("Second pass...");
                    YFile.ResolveLabels();
                    Console.WriteLine("Writing to file...");
                    YFileWriter.WriteToFile(YFile, destFilePath);
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
                finally
                {
                    Console.ResetColor();
                }

                if (BuildSuccessful)
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

            return true;
        }

    }
}
