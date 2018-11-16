using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTG_Collection_Tracker
{
    public static class PredicateExtensions
    {
        public static Predicate<T> And<T>(this Predicate<T> left, Predicate<T> right)
        {
            return x => left(x) && right(x);
        }
    }
}
