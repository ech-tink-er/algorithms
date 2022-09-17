namespace Dividers
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    static class Program
    {
        static void Main()
        {
            int digitsCount = int.Parse(Console.ReadLine());
            char[] digits = new char[digitsCount];

            for (int i = 0; i < digitsCount; i++)
            {
                digits[i] = Console.ReadLine()[0];
            }

            var permutations = GetPermutations(digits)
                .Select(dgts => new string(dgts))
                .Select(int.Parse)
                .ToArray();

            int result = FindNumberWithMinDividerCount(permutations);

            Console.WriteLine(result);
        }

        static int FindNumberWithMinDividerCount(int[] numbers)
        {
            int number = numbers[0];
            int minDividersCount = CountDividers(numbers[0]);

            for (int i = 1; i < numbers.Length; i++)
            {
                int count = CountDividers(numbers[i]); 

                if (count < minDividersCount)
                {
                    number = numbers[i];
                    minDividersCount = count;
                }
            }

            return number;
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

        static int CountDividers(int number)
        {
            int count = 0;

            int end = (int)Math.Sqrt(number);
            for (int i = 1; i <= end; i++)
            {
                if (number % i == 0)
                {
                    count += 2;
                }
            }

            if (end * end == number)
            {
                count--;
            }

            return count;
        }

        static void Reverse<T>(this T[] items, int start = 0, int length = -1)
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
        }

        static void Swap<T>(this T[] items, int first, int second)
        {
            T hold = items[first];
            items[first] = items[second];
            items[second] = hold;
        }
    }
}