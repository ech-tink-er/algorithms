namespace Tribonacci
{
    using System.Linq;
    using System.IO;
    using System.Numerics;

    using Testing;

    static class Program
    {
        public static void Main()
        {
            EasyTesting.Test("Tests", true, Solution);
        }

        public static void Solution(TextReader reader, TextWriter writer)
        {
            var args = reader.ReadLine().Split(' ')
                .Select(long.Parse)
                .ToArray();

            string result = Tribonacci(args[0], args[1], args[2], args[3])
                .ToString();

            writer.WriteLine(result);
        }

        public static BigInteger Tribonacci(BigInteger first, BigInteger second, BigInteger third, long n)
        {
            if (n == 1)
            {
                return first;
            }
            else if (n == 2)
            {
                return second;
            }
            else if (n == 3)
            {
                return third;
            }

            BigInteger result = 0;

            for (int i = 4; i <= n; i++)
            {
                result = first + second + third;

                first = second;
                second = third;
                third = result;
            }

            return result;
        }
    }
}