namespace HashTables
{
    using System.Linq;
    using System.Collections;
    using System.Collections.Generic;

    using static Utilities.UMath;

    public abstract class HashTable<K, V> : IEnumerable<V>
    {
        internal const int MinBuckets = 7;
        internal const int GrowthFactor = 2;
        internal const float MaxLoadFactor = 0.75f;
        internal const float MinLoadFactor = MaxLoadFactor / (GrowthFactor * GrowthFactor);

        internal LinkedList<V>[] buckets;

        protected HashTable()
        {
            this.Init();
        }

        public int Count { get; private set; }

        public void Add(V value)
        {
            if (MaxLoadFactor < this.LoadFactor(this.Count +  1))
            {
                this.Grow();
            }

            K key = this.SelectKey(value);
            int i = this.Hash(key);
            if (this.buckets[i] == null)
            {
                this.buckets[i] = new LinkedList<V>();
            }

            var node = this.GetNode(i, key);
            if (node != null)
            {
                this.buckets[i].Remove(node);
            }
            else
            {
                this.Count++;
            }

            this.buckets[i].AddLast(value);
        }

        public bool Remove(K key)
        {
            int i = this.Hash(key);
            var node = this.GetNode(i, key);

            if (node == null)
            {
                return false;
            }

            this.Count--;
            this.buckets[i].Remove(node);

            if (this.LoadFactor() < MinLoadFactor)
            {
                this.Shrink();
            }

            return true;
        }

        public bool Contains(K key)
        {
            return this.GetNode(this.Hash(key), key) != null;
        }

        public void Clear()
        {
            for (int i = 0; i < this.buckets.Length; i++)
            {
                this.buckets[i] = null;
            }

            this.Count = 0;
        }

        public void Trim()
        {
            this.Resize((int)(this.Count / MaxLoadFactor));
        }

        public IEnumerator<V> GetEnumerator()
        {
            foreach (var bucket in this.buckets.Where(b => b != null))
            {
                foreach (var value in bucket)
                {
                    yield return value;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        internal abstract K SelectKey(V value);

        internal float LoadFactor(int count = -1)
        {
            count = count == -1 ? this.Count : count;

            return count / (float)this.buckets.Length;
        }

        protected int Hash(K key)
        {
            return (int)((uint)key.GetHashCode() % (uint)this.buckets.Length);
        }

        protected LinkedListNode<V> GetNode(int bucket, K key)
        {
            var list = this.buckets[bucket];
            if (list == null)
            {
                return null;
            }

            var current = list.First;
            while (current != null)
            {
                if (this.SelectKey(current.Value).Equals(key))
                {
                    return current;
                }

                current = current.Next;
            }
            return null;
        }

        private void Grow() 
        {
            this.Resize(this.buckets.Length * GrowthFactor);
        }

        private void Shrink()
        {
            int buckets = this.buckets.Length / GrowthFactor;
            if (MinBuckets <= buckets)
            {
                this.Resize(buckets);
            }
        }

        private void Resize(int buckets)
        {
            var old = this.buckets;

            this.Init(NextPrime(buckets));
            foreach (var bucket in old.Where(b => b != null))
            {
                foreach (var pair in bucket)
                {
                    this.Add(pair);
                }
            }
        }

        private void Init(int buckets = MinBuckets)
        {
            this.buckets = new LinkedList<V>[buckets];
            this.Count = 0;
        }
    }
}