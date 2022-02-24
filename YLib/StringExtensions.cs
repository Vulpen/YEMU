using System;
using System.Collections.Generic;
using System.Text;

namespace YLib
{
    public static class StringExtensions
    {
        public static string TrimToFirst(this string value, string key)
        {
            if (!value.Contains(key)) return value;
            return value.Substring(0, value.IndexOf(key));
        }
    }
}
