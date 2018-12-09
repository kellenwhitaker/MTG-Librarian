using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTG_Collection_Tracker
{
    public static class IEnumerableExtensions
    {
        public static int Count(this IEnumerable source)
        {
            int count = 0;
            var enumerator = source.GetEnumerator();
                while (enumerator.MoveNext())
                    count++;

            return count;
        }
    }
}
