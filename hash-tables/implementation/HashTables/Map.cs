namespace HashTables
{
    using System;

    public class Map<K, V> : HashTable<K, Binding<K, V>>
    {
        public Map()
            : base()
        { }

        public void Add(K key, V value)
        {
            this.Add(new Binding<K, V>(key, value));
        }

        public V this[K key]
        {
            get
            {
                var node = this.GetNode(this.Hash(key), key);

                if (node == null)
                {
                    throw new InvalidOperationException("Key not found.");
                }

                return node.Value.Value;
            }

            set
            {
                this.Add(key, value);
            }
        }

        public bool Contains(V value)
        {
            foreach (var pair in this)
            {
                if (pair.Value.Equals(value))
                {
                    return true;
                }
            }

            return false;
        }

        internal override K SelectKey(Binding<K, V> value)
        {
            return value.Key;
        }
    }
}