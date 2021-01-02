using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using YLib;

namespace YAS
{
    public class Preprocessor
    {
        Dictionary<string, string> Macros;

        public Preprocessor()
        {
            Macros = new Dictionary<string, string>();
        }

        private void PopulateMacroTable(StreamReader stream)
        {
            string CurrentLine;
            string MacroKey = String.Empty;
            while(!stream.EndOfStream)
            {
                CurrentLine = stream.ReadLine().Trim();

                if(CurrentLine.StartsWith("#beginmacro"))
                {
                    if(MacroKey == string.Empty)
                    {
                        MacroKey = CurrentLine.Substring(11).Trim();
                    }
                    else
                    {
                        //ERROR
                        throw new AssemblerException(EnumAssemblerStages.Preprocessor, "Reached a begin macro without ending another macro.");
                    }
                }

                if(CurrentLine.StartsWith("#endmacro"))
                {

                }

                if(MacroKey != String.Empty)
                {

                }

                
            }

        }
    }
}
