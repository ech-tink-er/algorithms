namespace SearchTrees
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using static Utilities.UMath;

    internal partial class BNode<T>
        where T : IComparable<T>
    {
        private List<T> keys;

        public BNode(int order = 3)
        {
            if (order < BTree<T>.MinOrder)
            {
                throw new ArgumentException($"Order can't be less than {BTree<T>.MinOrder}.");
            }

            this.Order = order;
            this.MaxChildren = this.Order;
            this.MinChildren = this.MaxChildren == 3 ? 2 : this.MaxChildren / 2;
            this.MaxKeys = this.MaxChildren - 1;
            this.MinKeys = this.MaxKeys / 2;

            this.keys = new List<T>(this.MaxKeys + 1);
            this.children = new List<BNode<T>>(this.MaxChildren + 1);
        }

        public BNode(int order = 3, params T[] keys)
            : this(order)
        {
            Array.Sort(keys);
            this.keys.AddRange(keys);
        }

        public int Order { get; }

        public int MinKeys { get; }

        public int MaxKeys { get; }

        public IReadOnlyList<T> Keys
        {
            get
            {
                return this.keys;
            }
        }

        public BNode<T> Parent { get; private set; }

        public void SetKey(int location, T key)
        {
            location = LoopNumber(location, this.keys.Count);

            this.keys[location] = key;
        }

        public void InsertKey(int location, T key)
        {
            location = LoopNumber(location, this.keys.Count + 1);

            this.keys.Insert(location, key);
        }

        public void InsertKeys(int location,  IEnumerable<T> keys)
        {
            location = LoopNumber(location, this.keys.Count + 1);

            this.keys.InsertRange(location, keys);
        }

        public void PrependKey(T key)
        {
            this.InsertKey(0, key);
        }

        public void PrependKeys(IEnumerable<T> keys)
        {
            this.InsertKeys(0, keys);
        }

        public void AppendKey(T key)
        {
            this.InsertKey(this.keys.Count, key);
        }

        public void AppendKeys(IEnumerable<T> keys)
        {
            this.InsertKeys(this.keys.Count, keys);
        }

        public T PopKey(int location)
        {
            location = LoopNumber(location, this.keys.Count);

            T key = this.keys[location];

            this.keys.RemoveAt(location);

            return key;
        }

        public T[] PopKeys(int from = 0, int count = -1)
        {
            from = LoopNumber(from, this.keys.Count);
            count = LoopNumber(count, 1, this.keys.Count + 1);

            T[] keys = this.keys.Skip(from)
                .Take(count)
                .ToArray();

            this.keys.RemoveRange(from, count);

            return keys;
        }

        public T PopKeyHead()
        {
            return this.PopKey(0);
        }

        public T[] PopKeysHead(int count)
        {
            return this.PopKeys(0, count);
        }

        public T PopKeyTail()
        {
            return this.PopKey(this.keys.Count - 1);
        }

        public T[] PopKeysTail(int count)
        {
            return this.PopKeys(this.keys.Count - 1, count);
        }

        public bool KeyAtEquals(int location, T value)
        {
            return 0 <= location && location < this.keys.Count && 
                this.keys[location].Equals(value);
        }
       
        public override string ToString()
        {
            return "[" + string.Join(" ", this.keys) + "]";
        }

        private void ValidateKeysInsertion(int location, int count)
        {
            if (this.keys.Count + count > this.children.Capacity)
            {
                throw new InvalidOperationException("Keys are at maximum capacity!");
            }
        }
    }
}