namespace SequenceOfColoredBalls
{
    using System;
    using System.Numerics;
    using System.Collections.Generic;
    using System.Linq;

    static class Program
    {
        static void Main()
        {
            string sequence = Console.ReadLine();

            var counts = CountDuplicates(sequence);

            BigInteger permutationsCount = CalcPermutationsCount(counts);

            Console.WriteLine(permutationsCount);
        }

        static BigInteger CalcPermutationsCount(Dictionary<char, int> counts)
        {
            int total = counts.Sum(pair => pair.Value);

            BigInteger result = Factorial(total);

            foreach (var pair in counts)
            {
                result /= Factorial(pair.Value);
            }

            return result;
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

        static Dictionary<char, int> CountDuplicates(string sequence)
        {
            var counts = new Dictionary<char, int>();

            foreach (var @char in sequence)
            {
                if (counts.ContainsKey(@char))
                {
                    counts[@char]++;
                }
                else
                {
                    counts[@char] = 1;
                }
            }

            return counts;
        }
    }
}