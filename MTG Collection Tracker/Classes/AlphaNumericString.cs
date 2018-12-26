using System;
using System.Text.RegularExpressions;

namespace MTG_Librarian
{
    public class AlphaNumericString : IComparable<AlphaNumericString>
    {
        public string String { get; private set; }
        private string PaddedString;

        public AlphaNumericString(string str)
        {
            String = str;
            PaddedString = PadNumbers(String);
        }

        public int CompareTo(AlphaNumericString other)
        {
            return PaddedString.CompareTo(other.PaddedString);
        }

        private string PadNumbers(string inputString)
        {
            return Regex.Replace(inputString, "[0-9]+", match => match.Value.PadLeft(3, '0'));
        }

        public override string ToString()
        {
            return String;
        }
    }
}
