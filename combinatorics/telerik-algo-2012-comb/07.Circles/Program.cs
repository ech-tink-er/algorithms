namespace Circles
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;

    static class Program
    {
        static void Main()
        {
            string sequence = "123456789a";//Console.ReadLine();

            int result = CountUniques(sequence);

            Console.WriteLine(result);
        }

        static int CountUniques(string start)
        {
            int uniquesCount = 0;

            var permutations = new HashSet<string>();

            char[] sequence = start.OrderBy(c => c)
                .ToArray();

            int total = CalcPermutationsCount(sequence);

            do
            {
                int oldCount = permutations.Count;

                AddVariants(permutations, sequence);

                if (permutations.Count > oldCount)
                {
                    uniquesCount++;
                }

                if (permutations.Count == total)
                {
                    break;
                }
            } while (NextPermutation(sequence));


            return uniquesCount;
        }

        static void AddVariants(HashSet<string> permutations, char[] sequence)
        {
            var shifts = GetShifts(sequence);

            foreach (var shift in shifts)
            {
                permutations.Add(new string(shift));
                permutations.Add(new string(shift.Reverse()));
            }
        }

        static List<T[]> GetShifts<T>(this T[] items)
        {
            var shifts = new List<T[]>();
            shifts.Add(items.ToArray());

            for (int i = 1; i < items.Length; i++)
            {
                shifts.Add(items.RightShift(i));
            }

            return shifts;
        }

        static T[] RightShift<T>(this T[] items, int amount = 1)
        {
            amount = amount % items.Length;

            T[] result = new T[items.Length];

            for (int i = 0; i < result.Length; i++)
            {
                int from = ((items.Length - amount) + i) % items.Length;

                result[i] = items[from];
            }

            return result;
        }

        static List<T[]> GetPermutations<T>(T[] items) where T : IComparable<T>
        {
            var permutations = new List<T[]>();

            items = items.OrderBy(item => item)
                .ToArray();

            do
            {
                permutations.Add(items.ToArray());
            } while (NextPermutation(items));

            return permutations;
        }

        static bool NextPermutation<T>(T[] items) where T : IComparable<T>
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
            for (; next > descentStart; next--)
            {
                if (items[next].CompareTo(items[descentStart - 1]) > 0)
                {
                    break;
                }
            }

            items.Swap(descentStart - 1, next);

            items.Reverse(descentStart);

            return true;
        }

        static T[] Reverse<T>(this T[] items, int start = 0, int length = -1)
        {
            if (start < 0 || items.Length <= start )
            {
                throw new IndexOutOfRangeException("Start out of array range!");
            }

            if (length == -1)
            {
                length = items.Length - start;
            }

            int end = start + length - 1;
            if (items.Length <= end)
            {
                throw new IndexOutOfRangeException("Length out of array range!");
            }

            for (int i = 0; i < length / 2; i++)
            {
                int left = start + i;
                int right = end - i;

                items.Swap(left, right);
            }

            return items;
        }

        static void Swap<T>(this T[] items, int first, int second)
        {
            T hold = items[first];
            items[first] = items[second];
            items[second] = hold;
        }

        static int CalcPermutationsCount<T>(T[] sequence)
        {
            var bag = new Dictionary<T, int>();

            foreach (var item in sequence)
            {
                if (bag.ContainsKey(item))
                {
                    bag[item]++;
                }
                else{
                    bag[item] = 1;
                }
            }

            var all = Factorial(sequence.Length);
            var duplicates = bag.Select(pair => Factorial(pair.Value))
                .Aggregate((total, next) => total * next);

            return (int)(all / duplicates);
        }

        static BigInteger Factorial(BigInteger number)
        {
            BigInteger result = 1;

            for (BigInteger i = number; i >= 2; i--)
            {
                result *= i;
            }

            return result;
        }
    }
}