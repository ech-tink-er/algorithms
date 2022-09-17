namespace BDS
{
    using System;
    using System.Linq;
    using System.Collections;
    using System.Collections.Generic;

    public sealed class Queue<T> : IEnumerable<T>
    {
        private const int InitCapacity = 4;

        private const int GrowthFactor = 2;

        public T[] items;

        public int head;

        public int tail;

        public Queue(int capacity = InitCapacity)
        {
            this.items = new T[capacity];

            this.head = 0;
            this.tail = 0;

            this.Count = 0;
        }

        public Queue(IEnumerable<T> items)
            : this()
        {
            this.Enqueue(items);
        }

        public int Count { get; private set; }

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

                T[] @new = new T[value];

                int i = 0;
                foreach (var item in this)
                {
                    @new[i] = item;

                    i++;
                }

                this.items = @new;

                this.head = 0;
                this.tail = i;
            }
        }

        public void Enqueue(T item)
        {
            if (this.IsFull() || !this.items.Any())
            {
                this.Capacity = this.items.Any() ? this.items.Length * GrowthFactor : InitCapacity;
            }

            this.items[this.tail] = item;
            this.Count++;

            this.tail = this.GetNext(this.tail);
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

            T item = this.items[this.head];
            this.items[this.head] = default(T);

            this.Count--;

            this.head = this.GetNext(this.head);

            return item;
        }

        public T Peek()
        {
            this.ValidateNotEmpty();

            return this.items[this.head];
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (!this.items.Any())
            {
                yield break;
            }

            int start = this.head;

            if (this.IsFull())
            {
                yield return this.items[start];

                start = this.GetNext(start);
            }

            for (int i = start; i != this.tail; i = this.GetNext(i))
            {
                yield return this.items[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private int GetNext(int index)
        {
            return (index + 1) % this.items.Length;
        }

        private void ValidateNotEmpty()
        {
            if (this.Count > 0)
            {
                return;
            }

            throw new InvalidOperationException("Queue is Empty!");
        }

        private bool IsFull()
        {
            return this.head == this.tail && this.Count > 0;
        } 
    }
}