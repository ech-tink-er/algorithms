namespace Demo
{
    using System;
    using System.Linq;

    using HashTables;

    using Utilities;

    static class Program
    {
        private static readonly int[] Numbers = { 1, 3, 4, 4, 5, 6, 9, 13, 17, 100 };

        public static void Main()
        {
            Set<int> set = new Set<int>();
            set.Add(1);
            set.Add(5);
            set.Add(121);
            set.Add(1231);

            Map<string, string> map = new Map<string, string>();
            map["Apple"] = "Fruit";
            map["Blue"] = "Color";
            map["Dog"] = "Animal";
            map["Magic"] = "Abra Cadabra";


            Console.WriteLine($"Set: {set.Join()}\n");
            Console.WriteLine("Map:");
            foreach (var pair in map)
            {
                Console.WriteLine($"{pair.Key}: {pair.Value}");
            }
        }
    }
}