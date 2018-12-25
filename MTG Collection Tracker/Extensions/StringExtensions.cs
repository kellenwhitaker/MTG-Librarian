using System;
using System.Linq;

namespace MTG_Librarian
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

        public static string OnlyDigits(this string input)
        {
            return new string(input.Where(c => char.IsDigit(c)).ToArray());
        }
    }
}
