using System;
using System.Collections.Generic;
using System.IO;
using YLib;

namespace YAS
{


    class Program
    {
        static string PATH = @"E:\[]ProgrammingProjects\C#\YEMU\YAS\Examples\ex1.yas";
        //static string PATH = @"D:\[KEEP]ProgrammingProjects\C#\Y86Emulator\YAS\Examples\ex1.yas";
        //static string BIN_PATH = @"D:\[KEEP]ProgrammingProjects\C#\Y86Emulator\YAS\Examples\ex1.yin";
        static string BIN_PATH = @"E:\[]ProgrammingProjects\C#\YEMU\YAS\Examples\ex1.yin";
        static void Main(string[] args)
        {
            Parser YParser = new Parser(EnumVerboseLevels.All);
            TokenFile YFile = new TokenFile();
            BinaryFileWriter YFileWriter = new BinaryFileWriter();

            bool BuildSuccessful = true;

            if (!File.Exists(PATH))
            {
                Console.WriteLine("Could not find file at  " + PATH);
                return;
            }

            int LineNumber = 0;

            using (StreamReader readStream = new StreamReader(PATH))
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
                        currentTokens = YParser.ParseString(CurrentLine);
                        if (currentTokens == null || currentTokens.Length == 0)
                            continue;
                        YFile.AddLine(currentTokens);
                    }catch (FoundUnexpectedToken e)
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
#if DEBUG
                    catch (AssemblerException e)
                    {

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("ERROR at stage : " + Enum.GetName(typeof(EnumAssemblerStages), e._stage) + " " + e.Message);
                        BuildSuccessful = false;
                    }
                    catch(TokenAccessException e)
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
#else
                    catch (Exception e)
                    {
                        BuildSuccessful = false;
                    }
#endif
                    finally
                    {
                        Console.ResetColor();
                    }
                }



                try
                {
                    Console.WriteLine("Second pass...");
                    YFile.ResolveLabels();
                    Console.WriteLine("Writing to file...");
                    YFileWriter.WriteToFile(YFile, BIN_PATH);
                }
                catch (AssemblerException e)
                {
                    BuildSuccessful = false;
                    Console.WriteLine("ERROR at stage : " + Enum.GetName(typeof(EnumAssemblerStages), e._stage) + " " + e.Message);
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

                    Console.WriteLine(e.Message);
                }
                finally
                {
                    Console.ResetColor();
                }

                if (BuildSuccessful)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine(BIN_PATH + " successfully built");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("FAILED building: " + BIN_PATH);
                    Console.ResetColor();
                }
                
            }

        }
    }
}
