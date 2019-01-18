using System;
using System.Text.RegularExpressions;

namespace MTG_Librarian
{
    public class AlphaNumericString : IComparable<AlphaNumericString>
    {
        public string String { get; private set; }
        private readonly string PaddedString;

        public AlphaNumericString(string str)
        {
            String = str;
            PaddedString = PadNumbers(String);
        }

        public int CompareTo(AlphaNumericString other)
        {
            return PaddedString.CompareTo(other.PaddedString);
        }

        private static string PadNumbers(string inputString)
        {
            return Regex.Replace(inputString, "[0-9]+", match => match.Value.PadLeft(10, '0'));
        }

        public override string ToString()
        {
            return String;
        }
    }
}