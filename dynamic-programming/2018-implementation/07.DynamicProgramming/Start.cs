namespace DynamicProgramming
{
    using System;
    using System.Diagnostics;
    using System.Collections.Generic;

    static class Start
    {
        public static void Main()
        {
            Product[] products = new Product[]
            {
                new Product(name: "beer", weight: 3, value: 2),
                new Product(name: "vodka", weight: 8, value: 12),
                new Product(name: "cheese", weight: 4, value: 5),
                new Product(name: "nuts", weight: 1, value: 4),
                new Product(name: "ham", weight: 2, value: 3),
                new Product(name: "whiskey", weight: 8, value: 13),

                new Product(name: "a", weight: 3, value: 3),
                new Product(name: "b", weight: 15, value: 25),
                new Product(name: "c", weight: 20, value: 26),
                new Product(name: "d", weight: 8, value: 10),
                new Product(name: "e", weight: 5, value: 3),
                new Product(name: "f", weight: 2, value: 1),
                new Product(name: "g", weight: 100, value: 100),
                new Product(name: "h", weight: 40, value: 55),
                new Product(name: "i", weight: 13, value: 17),
            };

            var min = new Min(new int[] { 5, 4, 3, 2, 1, 2, 3, 4, 5 });
            var hanoi = new Hanoi(10);
            var fib = new Fibonacci(14);
            var ss = new SubsetSum(sum: 412, set: new int[] { 31, 31, 53, 51, 69, 0, 68, 23, 15, 23, 51, 53,23, 2, 12, 2, 7, 15, 45, 101, 33, 8, 37 });
            var lcs = new LCS(first: "YIRPHTQELELTOUWIWOOPRQPLD", second: "AHSEDLFLGOHLSHHJWOLKRKLLJHGKDAFDGJ");
            var lis = new LIS(sequence: new int[] { 1, 8, 2, 7, 3, 4, 1, 6 });
            var ks = new Knapsack(capacity: 120, products: products);
            var med = new MED(from: "developer", to: "enveloped", replaceCost: 1, deleteCost: 0.9, insertCost: 0.8);

            var tests = new Test[]
            {
                new Test("Min Recursive", () => min.Recursive()),
                new Test("Min Iterative", () => min.Iterative()),

                new Test("Hanoi Recusive", () => hanoi.Recursive()),
                new Test("Hanoi Iterative", () => hanoi.Iterative()),

                new Test("Fibonacci Recursive", () => fib.Recursive()),
                new Test("Fibonacci Iterative", () => fib.Iterative()),

                new Test("SubsetSum Recursive", () => ss.Recursive()),
                new Test("SubsetSum Iterative", () => ss.Iterative()),

                new Test("Longest Common Subsequence Recursive", () => lcs.Recursive()),
                new Test("Longest Common Subsequence Iterative", () => lcs.Iterative()),

                new Test("Longest Increasing Subsequence Recursive", () => lis.Recursive()),
                new Test("Longest Increasing Subsequence Iterative", () => lis.Iterative()),

                new Test("Knapsack Recursive", () => ks.Recursive()),
                new Test("Knapsack Iterative", () => ks.Iterative()),

                new Test("Minimum Edit Distance Recursive", () => med.Recursive()),
                new Test("Minimum Edit Distance Itereative", () => med.Iterative()),
                new Test("Minimum Edit Distance Itereative 2", () => med.Iterative2()),
            };

            Run(tests);
        }    

        public static void Run(IEnumerable<Test> tests)
        {
            var sw = new Stopwatch();

            foreach (var test in tests)
            {
                Console.WriteLine(test.Name + ":");
                sw.Restart();

                string result = test.Program().ToString();

                sw.Stop();

                Console.WriteLine($"Result: {result}, Time: {sw.Elapsed}\n");
            }
        }
    }
}