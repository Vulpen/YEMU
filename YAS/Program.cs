using System;
using System.Collections.Generic;
using System.IO;

namespace YAS
{
    enum EnumVerboseLevels
    {
        None = 0,
        Little,
        All
    }

    class Program
    {
        //static string PATH = @"E:\[]ProgrammingProjects\C#\YEMU\YAS\Examples\ex1.yas";
        static string PATH = @"D:\[KEEP]ProgrammingProjects\C#\Y86Emulator\YAS\Examples\ex1.yas";
        static void Main(string[] args)
        {
            Parser YParser = new Parser(EnumVerboseLevels.All);
            TokenFile YFile = new TokenFile();

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
                        Console.WriteLine($"ERROR : Unexpected Token {e.token1.Text} On Line : {LineNumber} | {CurrentLine}");
                    }
                    catch (AssemblerException e)
                    {
                        Console.WriteLine("ERROR at stage : " + Enum.GetName(typeof(EnumAssemblerStages), e._stage) + " " + e.Message);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                int debug = 0;
            }

        }
    }
}
