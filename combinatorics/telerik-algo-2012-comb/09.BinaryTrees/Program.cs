namespace BinaryTrees
{
    using System;
    using System.Collections.Generic;
    using System.Numerics;

    static class Program
    {
        static void Main()
        {
            string sequence = Console.ReadLine();

            BigInteger treesCount = CalcTreesCount(sequence.Length);
            BigInteger permutationsCount = CalcPermutationsCount(sequence);

            BigInteger result = treesCount * permutationsCount;

            Console.WriteLine(result);
        }

        // Catalan Numbers
        public static long CalcTreesCount(int nodes)
        {
            if (nodes < 1)
            {
                return 1;
            }

            long[] counts = new long[nodes + 1];
            counts[0] = 1;
            counts[1] = 1;

            for (int i = 2; i < counts.Length; i++)
            {
                int remaining = i - 1;

                for (int left = 0; left < i / 2; left++)
                {
                    int right = remaining - left;

                    counts[i] += counts[left] * counts[right];
                }

                counts[i] *= 2;

                if (i % 2 == 1)
                {
                    int mid = remaining / 2;

                    counts[i] += counts[mid] * counts[mid];
                }
            }

            return counts[nodes];
        }

        static BigInteger CalcPermutationsCount(string sequence)
        {
            var bag = ToBag(sequence);

            BigInteger total = Factorial(sequence.Length);

            foreach (var pair in bag)
            {
                total /= Factorial(pair.Value);
            }

            return total;
        }

        static BigInteger Factorial(int number, int length = int.MaxValue)
        {
            BigInteger result = 1;

            for (int i = number, c = 0; i >= 2 && c < length; i--, c++)
            {
                result *= i;
            }

            return result;
        }

        static Dictionary<char, int> ToBag(string sequence)
        {
            var bag = new Dictionary<char, int>();

            for (int i = 0; i < sequence.Length; i++)
            {
                if (bag.ContainsKey(sequence[i]))
                {
                    bag[sequence[i]]++;
                }
                else
                {
                    bag[sequence[i]] = 1;
                }
            }

            return bag;
        }
    }
}