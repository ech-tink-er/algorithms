namespace GameOfLife
{
    using System;
    using System.IO;
    using System.Linq;

    using Testing;

    static class Program
    {
        public static readonly int[][] NeighborChanges =
        {
            new int[] { 1, 0 },
            new int[] { -1, 0 },
            new int[] { 0, 1 },
            new int[] { 0, -1 },
                
            new int[] { 1, 1 },
            new int[] { -1, -1 },
            new int[] { 1, -1 },
            new int[] { -1, 1 }
        };

        public static void Main()
        {
            Console.WriteLine("...");
            EasyTesting.Test("Tests", true, Solution);
        }

        public static void Solution(TextReader reader, TextWriter writer)
        {
            int generation = int.Parse(reader.ReadLine());
            int[] dimensions = reader.ReadLine()
                .Split(' ')
                .Select(int.Parse)
                .ToArray();

            bool[,] field = ReadField(dimensions[0], dimensions[1], reader);

            field = GetGeneration(field, generation);

            int result = CountLiving(field);

            writer.WriteLine(result);
        }

        public static int CountLiving(bool[,] field)
        {
            int count = 0;

            for (int row = 0; row < field.GetLength(0); row++)
            {
                for (int col = 0; col < field.GetLength(1); col++)
                {
                    if (field[row, col])
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        public static bool[,] GetGeneration(bool[,] current, int generation)
        {
            bool[,] next = new bool[current.GetLength(0), current.GetLength(1)];

            for (int i = 0; i < generation; i++)
            {
                for (int row = 0; row < current.GetLength(0); row++)
                {
                    for (int col = 0; col < current.GetLength(1); col++)
                    {
                        next[row, col] = NextState(current, row, col);
                    }
                }

                var hold = current;
                current = next;
                next = hold;
            }

            return current;
        }

        public static bool NextState(bool[,] field, int row, int col)
        {
            bool state = field[row, col];
            int livingNeighbors = CountLivingNeighbors(field, row, col);

            if (!state && livingNeighbors == 3)
            {
                return true;
            }
            else if (state && (livingNeighbors < 2 || 3 < livingNeighbors))
            {
                return false;
            }

            return state;
        }

        public static int CountLivingNeighbors(bool[,] field, int row, int col)
        {
            int count = 0;

            foreach (var change in NeighborChanges)
            {
                int neighborRow = row + change[0];
                int neighborCol = col + change[1];

                if (IsInRange(neighborRow, field.GetLength(0)) && 
                    IsInRange(neighborCol, field.GetLength(1)) && 
                    field[neighborRow, neighborCol])
                {
                    count++;
                }
            }

            return count;
        }

        public static bool IsInRange(int value, int length)
        {
            return 0 <= value && value < length;
        }

        public static bool[,] ReadField(int rowsCount, int colsCount, TextReader reader = null)
        {
            if (reader == null)
            {
                reader = Console.In;
            }

            bool[,] field = new bool[rowsCount, colsCount];

            for (int r = 0; r < rowsCount; r++)
            {
                bool[] row = reader.ReadLine()
                    .Split(' ')
                    .Select(str => str == "1")
                    .ToArray();

                for (int c = 0; c < colsCount; c++)
                {
                    field[r, c] = row[c];
                }
            }

            return field;
        }

        public static void Clear<T>(this T[,] matrix)
        {
            for (int row = 0; row < matrix.GetLength(0); row++)
            {
                for (int col = 0; col < matrix.GetLength(1); col++)
                {
                    matrix[row, col] = default(T);
                }
            }
        }
    }
}