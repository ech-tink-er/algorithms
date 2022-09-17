namespace BDS
{
    using System;
    using System.Linq;
    using System.Collections;
    using System.Collections.Generic;

    public sealed class LinkedStack<T> : IEnumerable<T>
    {
        private LinkedList<T> items;

        public LinkedStack()
        {
            this.items = new LinkedList<T>();
        }

        public LinkedStack(IEnumerable<T> items)
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

        public void Push(T item)
        {
            this.items.AddLast(item);
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

            T last = this.items.Last.Value;

            this.items.RemoveLast();

            return last;
        }

        public T Peek()
        {
            this.ValidateNotEmpty();

            return this.items.Last.Value;
        }

        public void Clear()
        {
            this.items.Clear();
        }

        public bool Contains(T value)
        {
            return this.items.Contains(value);
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (var current = this.items.Last; current != null; current = current.Previous)
            {
                yield return current.Value;
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