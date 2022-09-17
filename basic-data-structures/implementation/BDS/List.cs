namespace BDS
{
    using System;
    using System.Linq;
    using System.Collections;
    using System.Collections.Generic;

    public sealed class List<T> : IEnumerable<T>
    {
        private const int InitCapacity = 4;

        private const int GrowthFactor = 2;

        private T[] items;

        public List(int capacity = InitCapacity)
        {
            this.items = new T[capacity];
            this.Count = 0;
        }

        public List(IEnumerable<T> items)
            : this()
        {
            this.Add(items);
        }

        public int Capacity
        {
            get
            {
                return this.items.Length;
            }

            set
            {
                if (value < this.Count)
                {
                    throw new ArgumentException("Capacity can't be less than Count!");
                }

                T[] old = this.items;
                this.items = new T[value];

                int count = Math.Min(old.Length, this.items.Length);

                for (int i = 0; i < count; i++)
                {
                    this.items[i] = old[i];
                }
            }
        }

        public int Count { get; private set; }

        public T this[int index]
        {
            get
            {
                if (!this.IsValidIndex(index))
                {
                    throw new IndexOutOfRangeException(index.ToString());
                }

                return this.items[index];
            }

            set
            {
                if (!this.IsValidIndex(index))
                {
                    throw new IndexOutOfRangeException(index.ToString());
                }

                this.items[index] = value;
            }
        }

        public void Add(T item)
        {
            if (this.items.Length == this.Count)
            {
                this.Capacity = this.items.Any() ? this.Count * GrowthFactor : InitCapacity;
            }

            this.items[this.Count] = item;

            this.Count++;
        }

        public void Add(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                this.Add(item);
            }
        }

        public void InsertAt(T item, int index)
        {
            if (!this.IsValidIndex(index))
            {
                throw new IndexOutOfRangeException(index.ToString());
            }

            this.Add(this.items[this.Count - 1]);

            for (int i = this.Count - 2; i >= index + 1; i--)
            {
                this.items[i] = this.items[i - 1];
            }

            this.items[index] = item;
        }

        public void RemoveAt(int index)
        {
            if (!this.IsValidIndex(index))
            {
                throw new IndexOutOfRangeException(index.ToString());
            }

            for (int i = index; i < this.Count - 1; i++)
            {
                this.items[i] = this.items[i + 1];
            }

            this.Count--;
            this.items[this.Count] = default(T);
        }

        public int IndexOf(T item)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this.items[i].Equals(item))
                {
                    return i;
                }
            }

            return -1;
        }

        public bool Contains(T item)
        {
            return this.IndexOf(item) >= 0;
        }

        public bool Remove(T item)
        {
            int index = this.IndexOf(item);
            if (index == -1)
            {
                return false;
            }

            this.RemoveAt(index);

            return true;
        }

        public void Clear()
        {
            for (int i = this.Count - 1; i >= 0; i--)
            {
                this.items[i] = default(T);
            }

            this.Count = 0;
        }

        public void Trim()
        {
            this.Capacity = this.Count;
        }

        public bool IsValidIndex(int index)
        {
            return 0 <= index && index < this.Count; 
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < this.Count; i++)
            {
                yield return this.items[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}