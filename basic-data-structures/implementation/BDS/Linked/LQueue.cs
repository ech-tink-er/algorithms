namespace BDS
{
    using System;
    using System.Linq;
    using System.Collections;
    using System.Collections.Generic;

    public sealed class LQueue<T> : IEnumerable<T>
    {
        private LinkedList<T> items;

        public LQueue()
        {
            this.items = new LinkedList<T>();
        }

        public LQueue(IEnumerable<T> items)
            : this()
        {
            this.Enqueue(items);
        }

        public int Count
        {
            get
            {
                return this.items.Count;
            }
        }

        public void Enqueue(T item)
        {
            this.items.AddLast(item);
        }

        public void Enqueue(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                this.Enqueue(item);
            }
        }

        public T Dequeue()
        {
            this.ValidateNotEmpty();

            T item = this.items.First.Value;
            this.items.RemoveFirst();

            return item;
        }

        public T Peek()
        {
            this.ValidateNotEmpty();

            return this.items.First.Value;
        }

        public bool Contains(T item)
        {
            return this.items.Contains(item);
        }

        public void Clear()
        {
            this.items.Clear();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.items.GetEnumerator();
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

            throw new InvalidOperationException("Queue is empty!");
        }
    }
}