namespace Room
{
    using System.Text;
    using System.Linq;
    using System.IO;

    using Testing;

    static class Program
    {
        public static void Main()
        {
            EasyTesting.Test("Tests", true, Solution);
        }

        public static void Solution(TextReader reader, TextWriter writer)
        {
            int type = int.Parse(reader.ReadLine());
            int[] dimensions = reader.ReadLine()
                .Split(' ')
                .Select(int.Parse)
                .ToArray();

            char[,] floor = new char[dimensions[0], dimensions[1]];

            for (int row = 0; row < floor.GetLength(0); row++)
            {
                string r = reader.ReadLine();

                for (int col = 0; col < floor.GetLength(1); col++)
                {
                    floor[row, col] = r[col];
                }
            }

            FloorBuilder builder = new FloorBuilder(floor);

            string result = null;
            if (type == 1)
            {
                result = builder.FindMinBoards().ToString();
            }
            else if (type == 2)
            {
                floor = builder.SetMinBoards();

                floor = Label(floor);

                result = Stringify(floor);
            }

            writer.WriteLine(result);
        }

        public static char[,] Label(char[,] floor)
        {
            char label = 'A';

            for (int row = 0; row < floor.GetLength(0); row++)
            {
                for (int col = 0; col < floor.GetLength(1); col++)
                {
                    if (floor[row, col] != Utils.Horizontal && floor[row, col] != Utils.Vertical)
                    {
                        continue;
                    }

                    floor = Label(floor, row, col, label);
                    label++;
                }
            }

            return floor;
        }

        public static char[,] Label(char[,] floor, int row, int col, char label)
        {
            bool horizontal = floor[row, col] == Utils.Horizontal;

            int start = horizontal ? col : row;
            int length = horizontal ? floor.GetLength(1) : floor.GetLength(0);

            for (int i = start; i < length; i++)
            {
                int r = horizontal ? row : i;
                int c = horizontal ? i : col;

                if ((horizontal && floor[r, c] == Utils.Horizontal) || (!horizontal && floor[r, c] == Utils.Vertical))
                {
                    floor[r, c] = label;
                }
                else
                {
                    break;
                }
            }

            return floor;
        }

        public static string Stringify(char[,] floor)
        {
            StringBuilder builder = new StringBuilder();

            for (int row = 0; row < floor.GetLength(0); row++)
            {
                for (int col = 0; col < floor.GetLength(1); col++)
                {
                    builder.Append(floor[row, col]);
                }

                builder.AppendLine();
            }

            return builder.ToString();  
        }
    }
}