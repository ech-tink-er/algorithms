namespace BDS.Heap
{
    using System;
    using System.Linq;
    using System.Collections;
    using System.Collections.Generic;

    using Utilities;
    using static Validations;

    public sealed class Heap<T> : Heap<T, T>
        where  T : IComparable<T>
    {
        private static T SelectValue(T value)
        {
            return value;
        }

        public Heap(bool reverse)
            : base(SelectValue, reverse)
        { }

        public Heap(bool reverse, IEnumerable<T> items)
            : base(SelectValue, reverse, items)
        { }

        public Heap()
            : base(SelectValue)
        { }

        public Heap(IEnumerable<T> items)
            : base(SelectValue, items)
        { }
    }

    public delegate K KeySelector<K, V>(V value);

    public class Heap<K, V> : IEnumerable<V>
        where K : IComparable<K>
    {
        internal static int Left(int i)
        {
            return i * 2 + 1;
        }

        internal static int Right(int i)
        {
            return i * 2 + 2;
        }

        internal static int Parent(int i)
        {
            return (i - 1) / 2;
        }

        private System.Collections.Generic.List<V> values;

        public Heap(KeySelector<K, V> selectKey, bool min)
        {
            this.SelectKey = selectKey;

            this.values = new System.Collections.Generic.List<V>();

            if (min)
            {
                this.Compare = Comparison.ReverseCompare;
            }
            else
            {
                this.Compare = Comparison.Compare;
            }
        }

        public Heap(KeySelector<K, V> selectKey, bool min, IEnumerable<V> values)
            : this(selectKey, min)
        {
            this.values = values.ToList();
            this.Heapify();
        }

        public Heap(KeySelector<K, V> selectKey)
            : this(selectKey, min: false)
        { }

        public Heap(KeySelector<K, V> selectKey, IEnumerable<V> values)
            : this(selectKey, min: false, values: values)
        { }

        public int Count
        {
            get
            {
                return this.values.Count;
            }
        }

        public V this[int i]
        {
            get
            {
                return this.values[i];
            }
        }

        internal Comparison<K> Compare { get; }

        internal KeySelector<K, V> SelectKey { get; }

        public void Push(V value)
        {
            this.values.Add(value);

            this.SiftUp(this.values.Count - 1);
        }

        public V Pop()
        {
            this.ValidateAny();

            V top = this.values[0];

            int last = this.values.Count - 1;
            this.values[0] = this.values[last];
            this.values.RemoveAt(last);

            if (this.values.Any())
            {
                this.SiftDown(0);
            }

            return top;
        }

        public V Peek()
        {
            this.ValidateAny();

            return this.values[0];
        }

        public V Slide(V value)
        {
            this.ValidateAny();

            var top = this.values[0];
            this.values[0] = value;

            this.SiftDown(0);

            return top;
        }

        public void Clear()
        {
            this.values.Clear();
        }

        public void Update()
        {
            this.Heapify();
        }   

        public void Update(int i)
        {
            this.SiftDown(i);
            this.SiftUp(i);
        }

        public IEnumerator<V> GetEnumerator()
        {
            return this.values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private void SiftDown(int i)
        {
            Validate(i, Index(this.values));

            int leavesStart = this.values.Count / 2;
            
            while (i < leavesStart)
            {
                int left = Left(i);
                int right = Right(i);
                bool hasLeft = left < this.values.Count;
                bool hasRight = right < this.values.Count;
                K leftKey = hasLeft ? this.SelectKey(this.values[left]) : default(K);
                K rightKey = hasRight ? this.SelectKey(this.values[right]) : default(K);

                int next = Left(i);
                K nextKey = leftKey;
                if (hasRight && this.Compare(leftKey, rightKey) < 0)
                {
                    next = right;
                    nextKey = rightKey;
                }

                if (this.Compare(this.SelectKey(this.values[i]), nextKey) < 0)
                {
                    this.values.Swap(i, next);
                }
                else
                {
                    break;
                }

                i = next;
            }
        }

        private void SiftUp(int i)
        {
            Validate(i, Index(this.values));

            while (i > 0)
            {
                int parent = Parent(i);
                K key = this.SelectKey(this.values[i]);
                K parentKey = this.SelectKey(this.values[parent]);

                if (this.Compare(key, parentKey) > 0)
                {
                    this.values.Swap(i, parent);
                }
                else
                {
                    break;
                }

                i = parent;
            }
        }

        private void Heapify()
        {
            for (int i = this.values.Count / 2 - 1; i >= 0; i--)
            {
                this.SiftDown(i);
            }
        }

        private void ValidateAny()
        {
            if (!this.values.Any())
            {
                throw new InvalidOperationException("Heap is empty!");
            }
        }
    }
}