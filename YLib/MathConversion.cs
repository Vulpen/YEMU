using System;
using System.Collections.Generic;
using System.Text;

namespace YLib
{
    public class MathConversion
    {
        public static bool ConvertHexToInt(string HexString, out Int64 returnNumber)
        {
            throw new NotImplementedException("Hex Conversion Not Supported Yet.");
        }

        public static bool ConvertHexToInt(string HexString, out int returnNumber)
        {
            throw new NotImplementedException("Hex Conversion Not Supported Yet.");
        }

        public static bool ConvertIntToHex(Int64 Number, out string HexString)
        {
            throw new NotImplementedException("Hex Conversion Not Supported Yet.");
        }

        public static bool ConvertIntToHex(int Number, out string HexString)
        {
            throw new NotImplementedException("Hex Conversion Not Supported Yet.");
        }

        /// <summary>
        /// Parse an immediate in Y86 language to an Int64
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static Int64 ParseImmediate(string Text)
        {
            string Temp = new string(Text);
            if (Text.StartsWith('$'))
            {
                //Check if it is an immediate, might be redundant.
                Temp = Temp.Substring(1);
                if (Temp.StartsWith("0x") || Temp.StartsWith("0X"))
                {
                    //Treat as Hex
                    Temp = Temp.Substring(2);
                    Temp = Temp.PadLeft(4, '0');
                    Int64 number;
                    //MathConversion.ConvertHexToInt(Temp, out number);
                    if (Int64.TryParse(Temp, System.Globalization.NumberStyles.HexNumber, null, out number))
                    {
                        return number;
                    }
                }
                else
                {
                    //Treat as base 10
                    Int64 number;
                    if (Int64.TryParse(Temp, out number))
                    {
                        return number;
                    }
                }
            }
            throw new AssemblerException(EnumAssemblerStages.Utility, "Could not parse " + Text + " to immediate");
            return -1;
        }

        public static bool Int64ToHexString(Int64 number, int padToLength, out string hexString)
        {
            hexString = String.Empty;
            hexString = number.ToString("X");
            hexString = hexString.PadLeft(padToLength, '0');
            return true;
        }
    }
}
