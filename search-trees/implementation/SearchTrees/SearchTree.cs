namespace SearchTrees
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using Utilities;

    public enum TreeStringFormat
    {
        List, Vertical, Horizontal
    }

    public abstract class SearchTree<T> : ICollection<T>, IReadOnlyCollection<T>
        where T : IComparable<T>
    {
        public int Count { get; protected set; }

        public virtual bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public abstract void Add(T item);

        public virtual void Add(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                this.Add(item);
            }
        }

        public abstract bool Remove(T item);

        public virtual bool Remove(IEnumerable<T> items)
        {
            bool result = true;

            foreach (var item in items)
            {
                result &= this.Remove(item);
            }

            return result;
        }

        public abstract void Clear();

        public abstract bool Contains(T item);

        public abstract T Min();

        public abstract T Max();

        public abstract IEnumerator<T> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public virtual void CopyTo(T[] array, int start)
        {
            int length = array.Length - start;
            if (length < this.Count)
            {
                throw new InvalidOperationException("Not enough length to copy in array!");
            }

            int i = start;
            foreach (var item in this)
            {
                array[i] = item;

                i++;
            }
        }

        public virtual T[] ToArray()
        {
            T[] array = new T[this.Count];

            this.CopyTo(array, 0);

            return array;
        }

        public override string ToString()
        {
            return this.ToString(TreeStringFormat.List);
        }

        public abstract string ToString(TreeStringFormat format, bool verbose = false);
    }
}