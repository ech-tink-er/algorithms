namespace HashTables
{
    public class Binding<K, V>
    {
        public Binding(K key, V value)
        {
            this.Key = key;
            this.Value = value;
        }

        public K Key { get; }

        public V Value { get; }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Binding<K, V>);
        }

        public override int GetHashCode()
        {
            return this.Key.GetHashCode();
        }

        public override string ToString()
        {
            return this.Key.ToString() + ": " + this.Value.ToString();
        }

        private bool Equals(Binding<K, V> other)
        {
            return other != null &&
                this.Key.Equals(other.Key);
        }
    }
}