namespace SecretLanguage
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    static class Utils
    {
        public static List<T[]> GetPermutations<T>(T[] items) where T : IComparable<T> 
        {
            var permutations = new List<T[]>();

            items = items.OrderBy(i => i)
                .ToArray();

            do
            {
                permutations.Add(items.ToArray());
            } while (NextPermutation(items));

            return permutations;
        }

        public static bool NextPermutation<T>(T[] items) where T : IComparable<T>
        {
            int descentStart = items.Length - 1;
            for (; descentStart >= 1; descentStart--)
            {
                if (items[descentStart - 1].CompareTo(items[descentStart]) < 0)
                {
                    break;
                }
            }

            if (descentStart == 0)
            {
                return false;
            }

            int next = items.Length - 1;
            for (; next >= descentStart; next--)
            {
                if (items[descentStart - 1].CompareTo(items[next]) < 0)
                {
                    break;
                }
            }

            items.Swap(descentStart - 1, next);
            items.Reverse(descentStart);

            return true;
        }

        public static T[] Reverse<T>(this T[] items, int start = 0, int? length = null)
        {
            if (!IsValidIndex(start, items.Length))
            {
                throw new ArgumentException("Start must be a valid index!");
            }

            if (length == null)
            {
                length = items.Length - start;
            }
            else if (length < 0)
            {
                throw new ArgumentException("Length can't be less than 0!");
            }

            int end = start + (int)length - 1;
            if (!IsValidIndex(end, items.Length))
            {
                throw new ArgumentException("Range out of array bounds!");
            }

            for (int i = 0; i < length / 2; i++)
            {
                items.Swap(start + i, end - i);

            }

            return items;
        }

        public static T[] Swap<T>(this T[] items, int first, int second)
        {
            T hold = items[first];
            items[first] = items[second];
            items[second] = hold;

            return items;
        }
        
        public static bool IsValidIndex(int index, int length)
        {
            return 0 <= index && index < length;
        }

        public static void Clear<T>(this T[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = default(T);
            }
        }
    }
}