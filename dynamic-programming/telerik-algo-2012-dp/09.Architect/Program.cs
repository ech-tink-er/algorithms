namespace Architect
{
    using System;
    using System.Linq;
    using System.IO;

    using Testing;

    static class Program
    {
        public static void Main()
        {
            Console.WriteLine("...");
            EasyTesting.Test("Tests", true, Solution);
        }

        public static void Solution(TextReader reader, TextWriter writer)
        {
            int blocksCount = int.Parse(reader.ReadLine());
            int[][] blocks = new int[blocksCount][];

            for (int i = 0; i < blocksCount; i++)
            {
                blocks[i] = reader.ReadLine()
                    .Split(' ')
                    .Select(int.Parse)
                    .ToArray();
            }

            Builder builder = new Builder(blocks);

            int tallest = builder.FindTallest();

            writer.WriteLine(tallest);
        }
    }
}