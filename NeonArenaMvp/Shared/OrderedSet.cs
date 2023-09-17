namespace NeonArenaMvp.Shared
{
    using System.Collections;

    public class OrderedSet<T> : ICollection<T> where T: notnull
    {
        private readonly IDictionary<T, LinkedListNode<T>> dictionary;
        private readonly LinkedList<T> linkedList;

        public OrderedSet()
            : this(EqualityComparer<T>.Default)
        {
        }

        public OrderedSet(IEqualityComparer<T> comparer)
        {
            dictionary = new Dictionary<T, LinkedListNode<T>>(comparer);
            linkedList = new LinkedList<T>();
        }

        public int Count => dictionary.Count;

        public virtual bool IsReadOnly => dictionary.IsReadOnly;

        void ICollection<T>.Add(T item)
        {
            Add(item);
        }

        public void Clear()
        {
            linkedList.Clear();
            dictionary.Clear();
        }

        public bool Remove(T item)
        {
            bool found = dictionary.TryGetValue(item, out LinkedListNode<T>? node);

            if (!found || node is null)
            {
                return false;
            }

            dictionary.Remove(item);
            linkedList.Remove(node);

            return true;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return linkedList.GetEnumerator();
        }

        public bool Contains(T item) => dictionary.ContainsKey(item);

        public void CopyTo(T[] array, int arrayIndex)
        {
            linkedList.CopyTo(array, arrayIndex);
        }

        public bool Add(T item)
        {
            if (dictionary.ContainsKey(item))
            {
                return false;
            }

            LinkedListNode<T> node = linkedList.AddLast(item);
            dictionary.Add(item, node);

            return true;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)linkedList).GetEnumerator();
        }
    }
}
