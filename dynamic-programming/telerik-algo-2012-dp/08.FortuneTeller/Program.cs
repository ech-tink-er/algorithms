namespace FortuneTeller
{
    using System.IO;
    using System.Linq;

    using Testing;

    static class Program
    {
        public const char GoodDay = 'G';

        public const char BadDay = 'B';

        public static void Main()
        {
            EasyTesting.Test("Tests", true, Solution);
        }

        public static void Solution(TextReader reader, TextWriter writer)
        {
            int[] runs = reader.ReadLine()
                .Split(' ')
                .Select(int.Parse)
                .ToArray();

            int totalRight = runs[0];
            int totalWrong = runs[1];

            string prediction = reader.ReadLine();

            Optimizer optimizer = new Optimizer(prediction, totalRight, totalWrong);

            int result = optimizer.GetBestPrediction();

            writer.WriteLine(result);
        }
    }
}