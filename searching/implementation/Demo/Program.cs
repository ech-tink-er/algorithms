namespace Demo
{
    using System;
    using System.Linq;

    using static Searching;

    static class Program
    {
        private static readonly int[] Numbers = { 1, 3, 4, 4, 5, 6, 9, 13, 17, 100 };

        public static void Main()
        {
            int query = 17;
            while (true)
            {
                Console.WriteLine(string.Join(", ", Numbers.Select((v, i) => i)));
                Console.WriteLine(string.Join(", ", Numbers));
                query = int.Parse(Console.ReadLine());

                int find = Search(Numbers, query);
                int binary = BinSearch(Numbers, query);
                int inter = InterpolationSearch(Numbers, query);

                Console.WriteLine($"Query: {find}");
                Console.WriteLine($"Lenear Search: {find}");
                Console.WriteLine($"Binary Search: {binary}");
                Console.WriteLine($"Interpolation Search: {inter}\n");
            }
        }
    }
}