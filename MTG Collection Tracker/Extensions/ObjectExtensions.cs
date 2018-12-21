using System;
using System.ComponentModel;

namespace MTG_Librarian
{
    public static class ObjectExtensions
    {
        public static void DumpToConsole(this object o)
        {
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(o))
            {
                string name = descriptor.Name;
                object value = descriptor.GetValue(o);
                Console.WriteLine("{0}={1}", name, value);
            }
        }
    }
}
