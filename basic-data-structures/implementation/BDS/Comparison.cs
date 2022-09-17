namespace BDS
{
    using System;

    static class Comparison
    {
        public static int Compare<T>(T first, T second)
            where T : IComparable<T>
        {
            return first.CompareTo(second);
        }

        public static int ReverseCompare<T>(T first, T second)
            where T : IComparable<T>
        {
            return -first.CompareTo(second);
        }
    }
}