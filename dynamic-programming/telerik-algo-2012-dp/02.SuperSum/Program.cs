namespace SuperSum
{
    using System.IO;
    using System.Linq;

    using Testing;

    static class Program
    {
        public static void Main()
        {
            EasyTesting.Test("Tests", true, Solution);
        }

        public static void Solution(TextReader reader, TextWriter writer)
        {
            int[] args = reader.ReadLine()
                .Split(' ')
                .Select(int.Parse)
                .ToArray();

            int result = SuperSum(args[1], args[0]);

            writer.WriteLine(result);
        }

        public static int SuperSum(int numbers, int sumCount)
        {
            int[] sums = GetNumbers(numbers);
            for (int c = 0, sum = 0; c < sumCount; c++, sum = 0)
            {
                for (int i = 0; i < sums.Length; i++)
                {
                    sum += sums[i];
                    sums[i] = sum;
                }
            }

            return sums[sums.Length - 1];
        }

        public static int[] GetNumbers(int n)
        {
            int[] numbers = new int[n];

            for (int i = 0; i < numbers.Length; i++)
            {
                numbers[i] = i + 1;
            }

            return numbers;
        }
    }
}