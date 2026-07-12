using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

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

        public static IEnumerable<T[]> Chunk<T>(this IEnumerable<T> source, int size)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (size <= 0) throw new ArgumentException("Size must be greater than 0.", nameof(size));

            using (var enumerator = source.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    var chunk = new T[size];
                    chunk[0] = enumerator.Current;
                    int count = 1;

                    while (count < size && enumerator.MoveNext())
                    {
                        chunk[count] = enumerator.Current;
                        count++;
                    }

                    if (count < size)
                    {
                        Array.Resize(ref chunk, count);
                    }

                    yield return chunk;
                }
            }
        }
    }
}
