using System;
using System.IO;

namespace YAS
{
    class Program
    {
        //static string PATH = @"E:\[]ProgrammingProjects\C#\YEMU\YAS\Examples\ex1.yas";
        static string PATH = @"D:\[KEEP]ProgrammingProjects\C#\Y86Emulator\YAS\Examples\ex1.yas";
        static void Main(string[] args)
        {
            Parser YParser = new Parser();

            if (!File.Exists(PATH))
            {
                Console.WriteLine("Could not find file at  " + PATH);
                return;
            }

            int LineNumber = 0;

            using (StreamReader readStream = new StreamReader(PATH))
            {
                string CurrentLine;
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
                        YParser.ParseString(CurrentLine);
                    }catch (FoundUnexpectedToken e)
                    {
                        //Console.WriteLine("ERROR : Unexpected Token " + e.token1.Text + " On Line : " + LineNumber + "|" + CurrentLine);
                        Console.WriteLine($"ERROR : Unexpected Token {e.token1.Text} On Line : {LineNumber} | {CurrentLine}");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }

        }
    }
}
