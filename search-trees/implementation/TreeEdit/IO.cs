namespace TreeEdit
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;

    using Utilities;

    using static State;
    using static Parsing;

    internal static class IO
    {
        public static TextReader Input = Console.In;
        public static TextWriter Output = Console.Out;
        public static TextWriter Error = Console.Error;
        public static readonly StringBuilder OutBuffer = new StringBuilder();

        public static readonly REPL REPL = new REPL();

        private const string SetsFile = "sets.txt";
        private const string SetsSeparator = ", ";

        public static void PrintSet(int set)
        {
            OutBuffer.AppendLine($"{set + 1}: {Sets[set].Join(", ")}");
        }

        public static bool LoadSets()
        {
            if (!File.Exists(SetsFile))
            {
                throw new FileNotFoundException($"{SetsFile} is missing.");
            }

            var sets = File.ReadAllLines(SetsFile)
                .Where(line => !string.IsNullOrEmpty(line))
                .Select(line => ParseNumbers(line.Split(SetsSeparator)));

            Sets = new List<int[]>(sets);

            return true;
        }

        public static bool SaveSets()
        {
            File.WriteAllLines(SetsFile, Sets.Select(s => s.Join(SetsSeparator)).ToArray());
            return true;
        }
    }
}