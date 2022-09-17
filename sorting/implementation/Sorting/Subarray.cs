namespace Sorting
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class Subarray<T> : IList<T>
    {
        private T[] array;
        private int from;
        private int to;

        public Subarray(T[] array)
        {
            this.array = array;
            this.SetRange(0, this.array.Length - 1);
        }

        public Subarray(T[] array, int from, int x, bool length = false)
        {
            this.array = array;
            this.SetRange(from, x, length);
        }

        public T this[int i]
        {
            get
            {
                this.ValidateIndex(i);

                return this.array[this.AbsoluteIndex(i)];
            }

            set
            {
                this.ValidateIndex(i);

                this.array[this.AbsoluteIndex(i)] = value;
            }
        }

        public T[] Array
        {
            get
            {
                return this.array;
            }
        }

        public int From
        {
            get
            {
                return this.from;
            }
            set
            {
                this.ValidateRange(value, this.to);

                this.from = value;
            }
        }

        public int To
        {
            get
            {
                return this.to;
            }
            set
            {
                this.ValidateRange(this.from, value);

                this.to = value;
            }
        }

        public int Count
        {
            get
            {
                return this.ToLength(this.to);
            }

            set
            {
                this.to = this.FromLength(value);
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return ((IList<T>)this.Array).IsReadOnly;
            }
        }

        public void SetRange(int from, int x, bool length = false)
        {
            int to = length ? FromLength(x) : x;

            this.ValidateRange(from, to);

            this.from = from;
            this.to = to;
        }

        public bool IsValidRange(int from, int to)
        {
            return from <= to &&
                0 <= from && to < this.array.Length;
        }

        public bool IsValidIndex(int i)
        {
            return 0 <= i && i < this.Count;
        }

        // IList<T> Methods
        public int IndexOf(T item)
        {
            IList<T> list = this.Array;

            int i = list.IndexOf(item);

            if (!this.IsValidIndex(i))
                return -1;

            return i;
        }

        public void Insert(int index, T item)
        {
            this.ValidateIndex(index);

            IList<T> list = this.Array;

            list.Insert(this.AbsoluteIndex(index), item);

            this.Count++;
        }

        public void RemoveAt(int index)
        {
            this.ValidateIndex(index);

            IList<T> list = this.Array;

            list.RemoveAt(this.AbsoluteIndex(index));

            this.Count--;
        }

        public void Add(T item)
        {
            IList<T> list = this.Array;

            list.Add(item);

            this.Count--;
        }

        public void Clear()
        {
            IList<T> list = this.Array;

            list.Clear();

            this.Count = 0;
        }

        public bool Contains(T item)
        {
            return this.IndexOf(item) != -1;
        }

        public void CopyTo(T[] array, int start)
        {
            int length = array.Length - start;
            if (length < this.Count)
                throw new ArgumentException("Destination array was not long enough.");

            for (int i = 0; i < this.Count; i++)
                array[start + i] = this[i];
        }

        public bool Remove(T item)
        {
            IList<T> list = this.Array;

            var res = list.Remove(item);

            this.Count--;

            return res;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)this.Array).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Array.GetEnumerator();
        }

        private void ValidateRange(int from, int to)
        {
            if (!this.IsValidRange(from, to))
                throw new ArgumentException("Invalid range.");
        }

        private void ValidateIndex(int i)
        {
            if (!this.IsValidIndex(i))
                throw new IndexOutOfRangeException(i.ToString());
        }

        private int FromLength(int length)
        {
            return this.from + length - 1;
        }

        private int ToLength(int to)
        {
            return to - this.from + 1;
        }

        private int AbsoluteIndex(int i)
        {
            return this.from + i;
        }
    }
}