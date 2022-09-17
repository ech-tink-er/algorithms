namespace HashTables
{
    public class Set<T> : HashTable<T, T>
    {
        public Set()
            : base()
        { }

        internal override T SelectKey(T value)
        {
            return value;
        }
    }
}