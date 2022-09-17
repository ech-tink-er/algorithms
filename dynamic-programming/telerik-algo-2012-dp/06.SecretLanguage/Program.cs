namespace SecretLanguage
{
    using System.Linq;
    using System.IO;
    using System.Collections.Generic;

    using Testing;

    static class Program
    {
        public static void Main()
        {
            EasyTesting.Test("Tests", true, Solution);
        }

        public static void Solution(TextReader reader, TextWriter writer)
        {
            string message = reader.ReadLine();
            reader.ReadLine();
            string[] words = reader.ReadLine()
                .Split(' ')
                .ToArray();

            reader.Dispose();

            var lengths = new SortedSet<int>(words.Select(str => str .Length))
                .ToArray();

            var decoder = new Decoder(message, words);

            int? result = decoder.Decode() ?? -1;

            writer.WriteLine(result);
        }
    }
}