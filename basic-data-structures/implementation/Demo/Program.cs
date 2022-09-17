namespace Demo
{
    using System;

    using BDS;
    using BDS.Heap;

    static class Program
    {
        private static readonly int[] Numbers = { 1, 3, 4, 4, 5, 6, 9, 13, 17, 100 };

        public static void Main()
        {
            var list = new List<int>(Numbers);
            var linkedList = new LList<int>(Numbers);
            var stack = new Stack<int>(Numbers);
            var queue = new Queue<int>(Numbers);
            var heap = new Heap<int>(Numbers);

            Console.WriteLine($"Array:       {string.Join(", ",  Numbers)}");
            Console.WriteLine($"List:        {string.Join(", ",  list)}");
            Console.WriteLine($"Linked List: {string.Join(", ",  linkedList)}");
            Console.WriteLine($"Stack:       {string.Join(", ",  stack)}");
            Console.WriteLine($"Queue:       {string.Join(", ",  queue)}");
            Console.WriteLine($"Heap:        {string.Join(", ",  heap)}\n");
        }
    }
}