using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTG_Collection_Tracker
{
    public static class StringExtensions
    {
        public static string StripPunctuation(this string input)
        {
            return new string(input.Where(c => !char.IsPunctuation(c)).ToArray());
        }

        public static string SanitizeFilename(this string input)
        {
            char[] forbidden = new char[] { '*', '.', '"', '/', '\\', '[', ']', ':', ';', '|', '='};
            return new string(input.Where(c => !forbidden.Contains(c)).ToArray());
        }
    }
}
