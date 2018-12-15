using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTG_Librarian
{
    public class ConcurrentDeque<T> : IProducerConsumerCollection<T>
    {
        private readonly object lockObject = new object();
        private LinkedList<T> BackingList = null;

        public ConcurrentDeque()
        {
            BackingList = new LinkedList<T>();
        }

        public int Count => BackingList.Count;

        public object SyncRoot => lockObject;

        public bool IsSynchronized => true;

        public void CopyTo(T[] array, int index)
        {
            lock (lockObject) BackingList.CopyTo(array, index);
        }

        public void CopyTo(Array array, int index)
        {
            lock (lockObject) (BackingList as ICollection).CopyTo(array, index);
        }

        public IEnumerator<T> GetEnumerator()
        {
            LinkedList<T> copy = null;
            lock (lockObject) copy = new LinkedList<T>(BackingList);
            return copy.GetEnumerator();
        }

        public T[] ToArray()
        {
            T[] array = null;
            lock (lockObject) array = BackingList.ToArray();
            return array;
        }

        public bool TryAdd(T item)
        {
            lock (lockObject) BackingList.AddLast(item);
            return true;
        }

        public bool TryTake(out T item)
        {
            bool success = false;
            item = default(T);
            lock (lockObject)
            {
                if (BackingList.First != null)
                {
                    item = BackingList.First.Value;
                    BackingList.RemoveFirst();
                    success = true;
                }
            }
            return success;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (this as IEnumerable<T>).GetEnumerator();
        }

        public bool Enqueue(T item)
        {
            return TryAdd(item);
        }

        public bool TryDequeue(out T item)
        {
            return TryTake(out item);
        }

        public bool AddFirst(T item)
        {
            lock (lockObject) BackingList.AddFirst(item);
            return true;
        }

        public bool RemoveLast(out T item)
        {
            bool success = false;
            lock (lockObject)
            {
                item = default(T);
                if (BackingList.Last != null)
                {
                    item = BackingList.Last.Value;
                    BackingList.RemoveLast();
                    success = true;
                }
            }
            return success;
        }
    }
}
