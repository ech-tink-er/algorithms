namespace AcademyProblems
{
    using System;
    using System.Linq;
    using System.IO;

    using Testing;

    class Program
    {
        static void Main()
        {
            EasyTesting.Test("Tests", true, Solution);
        }

        static void Solution(TextReader reader, TextWriter writer)
        {
            reader.ReadLine();

            int[] tasks = reader.ReadLine()
                .Split(' ')
                .Select(int.Parse)
                .ToArray();

            int tolerance = int.Parse(reader.ReadLine());

            int result = Solve(tasks, tolerance);

            writer.WriteLine(result);
        }

        static int Solve(int[] tasks, int tolerance)
        {
            if (tasks.Length == 0)
            {
                return 0;
            }
            else if (tasks.Length == 1)
            {
                return 1;
            }

            int min = tasks.Length;

            for (int l = 0; l < tasks.Length - 1; l++)
            {
                for (int r = 1; r < tasks.Length; r++)
                {
                    if (Math.Abs(tasks[l] - tasks[r]) >= tolerance)
                    {
                        int count = Distance(0, l) + Distance(l, r) + 1;

                        if (count < min)
                        {
                            min = count;
                        }
                    }
                }
            }

            return min;
        }

        static int Distance(int first, int second)
        {
            return (Math.Abs(first - second) + 1) / 2;
        }
    }
}