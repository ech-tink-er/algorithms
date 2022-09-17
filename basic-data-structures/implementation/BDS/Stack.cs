namespace BDS
{
    using System;
    using System.Linq;
    using System.Collections;
    using System.Collections.Generic;

    public sealed class Stack<T> : IEnumerable<T>
    {
        private const int InitCapacity = 4;

        private List<T> items;

        public Stack(int capacity = InitCapacity)
        {
            this.items = new List<T>(capacity);
        }

        public Stack(IEnumerable<T> items)
            : this()
        {
            this.Push(items);
        }

        public int Count
        {
            get
            {
                return this.items.Count;
                     
            }
        }

        public int Capacity
        {
            get
            {
                return this.items.Capacity;
            }

            set
            {
                this.items.Capacity = value;
            }
        }

        public void Push(T item)
        {
            this.items.Add(item);
        }

        public void Push(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                this.Push(item);
            }
        }

        public T Pop()
        {
            this.ValidateNotEmpty();

            T last = this.items[this.items.Count - 1];

            this.items.RemoveAt(this.items.Count - 1);

            return last;
        }

        public T Peek()
        {
            this.ValidateNotEmpty();

            return this.items[this.items.Count - 1];
        }

        public void Clear()
        {
            this.items.Clear();
        }

        public bool Contains(T value)
        {
            return this.items.Contains(value);
        }

        public void Trim()
        {
            this.items.Trim();
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = this.items.Count - 1; i >= 0; i--)
            {
                yield return this.items[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private void ValidateNotEmpty()
        {
            if (this.items.Any())
            {
                return;
            }

            throw new InvalidOperationException("Stack is empty!");
        }
    }
}