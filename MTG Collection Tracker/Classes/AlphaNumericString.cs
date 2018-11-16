using System;
//todo: change highlighting in treeview
namespace MTG_Collection_Tracker
{
    public class AlphaNumericString : IComparable<AlphaNumericString>
    {
        private Int32? _intvalue;
        public bool HasValue => _intvalue.HasValue;
        public int Value => _intvalue.Value;
        public string String { get; private set; }

        public AlphaNumericString(string str)
        {
            String = str;
            if (Int32.TryParse(str, out int value))
                _intvalue = value;
        }

        public int CompareTo(AlphaNumericString other)
        {
            if (HasValue && other.HasValue)
                return Value.CompareTo(other.Value);
            else
                return String.CompareTo(other.String);
        }
    }
}
