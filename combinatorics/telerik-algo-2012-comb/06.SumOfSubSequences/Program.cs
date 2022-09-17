namespace SumOfSubSequences
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Collections.Generic;
    using System.Numerics;

    static class Program
    {
        static void Main()
        {
            var parameters = ReadParameters("input.txt");

            // Within n choose k combinations each number from n occurs n-1 choose k-1 times - once for every combination that is one shorter of the other numbers from n.
            var results = parameters.Select(@params => 
            {
                return @params.Set.Sum(number => number) * CalcCombinationsCount(@params.Set.Length - 1, @params.Choose - 1);
            });

            foreach (var result in results)
            {
                Console.WriteLine(result);
            }
        }

        static BigInteger CalcCombinationsCount(int total, int length)
        {
            return (Factorial(total, length) / Factorial(length));
        }

        static BigInteger Factorial(int number, int take = int.MaxValue)
        {
            BigInteger result = 1;

            for (int i = number, c = 0; i >= 2 && c < take; i--, c++)
            {
                result *= i;
            }

            return result;
        }

        static Parameters[] ReadParameters(string fileName = null)
        {
            TextReader reader = Console.In;
            if (fileName != null)
            {
                reader = new StreamReader(fileName);
            }

            int parametersCount = int.Parse(reader.ReadLine());
            Parameters[] parameters = new Parameters[parametersCount];

            for (int i = 0; i < parametersCount; i++)
            {
                int remove = int.Parse(reader.ReadLine()
                    .Split(' ')
                    .ElementAt(1));
                int[] sequence = reader.ReadLine()
                    .Split(' ')
                    .Select(int.Parse)
                    .ToArray();

                // remove 5 of 8 == choose 3 of 8, combinations are symetrical
                int choose = sequence.Length - remove;

                parameters[i] = new Parameters(choose, sequence);
            }

            reader.Dispose();

            return parameters;
        }
    }

    class Parameters
    {
        public Parameters(int choose, int[] set)
        {
            this.Choose = choose;
            this.Set = set;
        }

        public int Choose { get; }

        public int[] Set { get; }
    }
}