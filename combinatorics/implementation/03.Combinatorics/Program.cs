namespace Combinatorics
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    static class Program
    {
        static void Main()
        {
            int[] numbers = { 1, 2, 3, 4 };

            var code = Conversions.PermutationToDecimal(numbers);
            var permutation = Conversions.DecimalToPermutation(code, numbers);

            Console.WriteLine(code);
            Console.WriteLine(string.Join(" ", permutation));
        }

        static void Test()
        {
            var items = new Dictionary<int, int>() 
            {
                {1, 2},
                {2, 2},
                {3, 3},
                {4, 1}
            };

            //Console.WriteLine("Permutations:");
            //int permutationsCount = PrintPermutations(items);
            //Console.WriteLine($"Count: {permutationsCount}");
            //Console.WriteLine("----------------------");

            Console.WriteLine("Combinations:");
            int combinationsCount = PrintCombinations(items, 6);
            Console.WriteLine($"Count: {combinationsCount}");
            Console.WriteLine("----------------------");
        }

        static int PrintPermutations<T>(IDictionary<T, int> items, int? length = null, int? total = null, List<T> permutation = null)
        {
            if (total == null)
            {
                total = items.Sum(pair => pair.Value);
            }

            if (length == null || length < 0 || length > total)
            {
                length = total;
            }

            if (permutation == null)
            {
                permutation = new List<T>();
            }

            if (permutation.Count == length)
            {
                Console.WriteLine(string.Join(" ", permutation));
                return 1;
            }

            int count = 0;

            foreach (var key in items.Keys.ToArray())
            {
                if (items[key] == 0)
                {
                    continue;
                }

                permutation.Add(key);
                items[key]--;

                count += PrintPermutations(items, length,  total, permutation);

                permutation.RemoveAt(permutation.Count - 1);
                items[key]++;
            }

            return count;
        }

        static int PrintCombinations<T>(IDictionary<T, int> items, int? length = null, int? total = null, List<T> combination = null)
        {
            if (total == null)
            {
                total = items.Sum(pair => pair.Value);
            }

            if (length == null || length < 0 || length > total)
            {
                length = total;
            }

            if (combination == null)
            {
                combination = new List<T>();
            }

            if (combination.Count == length)
            {
                Console.WriteLine(string.Join(" ", combination));
                return 1;
            }

            int count = 0;

            bool skip = true;
            foreach (var key in items.Keys.ToArray())
            {
                if (!combination.Any() || key.Equals(combination.Last()))
                {
                    skip = false;
                }

                if (skip || items[key] == 0)
                {
                    continue;
                }

                combination.Add(key);
                items[key]--;

                count += PrintCombinations(items, length,  total, combination);

                combination.RemoveAt(combination.Count - 1);
                items[key]++;
            }

            return count;
        }
    }
}