namespace Combinatorics
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    // Methods for converting between a permutation of unique elements and an int using Lehmer codes.
    class Conversions
    {
        public static int PermutationToDecimal<T>(T[] permutation) where T : IComparable<T>
        {
            int[] code = PermutationToBaseFactorial(permutation);

            int number = BaseFactorialToDecimal(code);

            return number;
        }

        public static T[] DecimalToPermutation<T>(int number,  T[] items) where T: IComparable<T>
        {
            int[] code = DecimalToBaseFactorial(number);

            T[] permutation = BaseFactorialToPermutation(code, items);

            return permutation;
        }

        public static int[] PermutationToBaseFactorial<T>(T[] permutation) where T : IComparable<T>
        {
            int[] code = new int[permutation.Length - 1];

            for (int i = 0; i < code.Length; i++)
            {
                code[i] = CountRightLesser(permutation, i);
            }

            return code;
        }

        public static T[] BaseFactorialToPermutation<T>(int[] code, T[] items) where T : IComparable<T>
        {
            var ordered = items.OrderBy(item => item)
                .ToList();
            T[] permutation = new T[items.Length];

            int padding = (items.Length - 1) - code.Length;

            code = new int[padding].Concat(code).ToArray();

            for (int i = 0; i < code.Length; i++)
            {
                permutation[i] = ordered[code[i]];

                ordered.RemoveAt(code[i]);
            }

            permutation[permutation.Length - 1] = ordered[0];

            return permutation;
        }

        public static int BaseFactorialToDecimal(int[] code)
        {
            int number = 0;

            for (int i = 0; i < code.Length; i++)
            {
                number += code[i] * Factorial(code.Length - i);
            }

            return number;
        }

        public static int[] DecimalToBaseFactorial(int number)
        {
            var code = new List<int>();

            for (int radix = 2; number != 0; number /= radix, radix++)
            {
                int remainder = number % radix;
                code.Add(remainder);
            }

            code.Reverse();

            return code.ToArray();
        }

        public static int Factorial(int number)
        {
            int result = 1;

            for (int i = number; i > 1; i--)
            {
                result *= i;
            }

            return result;
        }

        private static int CountRightLesser<T>(T[] items, int index) where T : IComparable<T>
        {
            int count = 0;

            for (int i = index + 1; i < items.Length; i++)
            {
                if (items[index].CompareTo(items[i]) > 0)
                {
                    count++;
                }
            }

            return count;
        }
    }
}