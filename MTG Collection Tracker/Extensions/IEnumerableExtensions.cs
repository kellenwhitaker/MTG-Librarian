using System.Collections;

namespace MTG_Librarian
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
