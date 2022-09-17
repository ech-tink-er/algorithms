namespace ZombieCamel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;

    static class Program
    {
        static void Main()
        {
            int[] distances = GetDistances(ReadObelisks());

            ulong sum = SumCombinations(distances);

            Console.WriteLine(sum);
        }

        static ulong SumCombinations(int[] distances)
        {
            ulong sum = 0;

            ulong totalDistance = (ulong)distances.Sum();

            // Calc for half the combinations then double since they are symetrical.
            for (int choose = 1; choose <= distances.Length / 2; choose++)
            {
                sum += (ulong)CalcCombinationsCount(distances.Length - 1, choose - 1) * totalDistance;
            }

            sum *= 2;

            if (distances.Length % 2 == 1)
            {
                sum += (ulong)CalcCombinationsCount(distances.Length - 1, distances.Length / 2) * totalDistance;
            }

            return sum;
        }

        static BigInteger CalcCombinationsCount(int total, int choose)
        {
            return Utils.Factorial(total) / (Utils.Factorial(choose) * Utils.Factorial(total - choose));
        }

        static int[] GetDistances(int[][] obelisks)
        {
            return obelisks.Select(obelisk => Math.Abs(obelisk[0]) + Math.Abs(obelisk[1]))
                .ToArray();
        }

        static int[][] ReadObelisks()
        {
            int count = int.Parse(Console.ReadLine());

            int[][] obelisks = new int[count][];

            for (int i = 0; i < obelisks.Length; i++)
            {
                obelisks[i] = Console.ReadLine()
                    .Split(' ')
                    .Select(int.Parse)
                    .ToArray();
            }

            return obelisks;
        }
    }

    static class Utils
    {
        private static readonly Dictionary<int, BigInteger> FactorialCache = new Dictionary<int, BigInteger>();

        public static BigInteger Factorial(int number)
        {
            bool isNegative = false;

            if (number < 0)
            {
                number = -number;
                isNegative = true;
            }

            if (number == 0 || number == 1)
            {
                return 1;
            }

            BigInteger result = 1;
            if (Utils.FactorialCache.ContainsKey(number))
            {
                result = Utils.FactorialCache[number]; 
            }
            else
            {
                result = number * Factorial(number - 1);
            }

            if (isNegative)
            {
                result = -result;
            }

            return result;
        }
    }
}