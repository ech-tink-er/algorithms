namespace ZigZagSequence
{
    using System;
    using System.Linq;

    static class Program
    {
        static void Main(string[] args)
        {
            int[] data = Console.ReadLine().Split(' ').Select(int.Parse)
                .ToArray();

            int count = CountZigZagSequences(total: data[0], length: data[1]);

            Console.WriteLine(count);
        }

        static int CountZigZagSequences(int total, int length, int index = 0, int previous = -1, bool[] used = null)
        {
            if (used == null)
            {
                used = new bool[total];
            }

            if (index == length)
            {
                return 1;
            }

            int count = 0;

            int start = 0;
            int end = total;

            if (index % 2 == 0)
            {
                start = previous + 1;
            }
            else
            {
                end = previous;
            }

            for (int i = start; i < end; i++)
            {
                if (used[i])
                {
                    continue;
                }

                used[i] = true;
                count += CountZigZagSequences(total, length, index + 1, i, used);
                used[i] = false;
            }

            return count;
        }
    }
}