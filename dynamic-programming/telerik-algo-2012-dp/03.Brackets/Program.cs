namespace Brackets
{
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
            // Iterative Solution
            long result = CountValidExpressions(reader.ReadLine());

            // Recursive Solution
            //Counter counter = new Counter(reader.ReadLine());
            //long result = counter.CountValidExpressions();

            writer.WriteLine(result);
        }

        public static long CountValidExpressions(string pattern)
        {
            // Holds the amount of valid expressons for a suffix of pattern given an amount of previous open brackets.
            // Starts at the last suffix, which must be a closing bracket, meaning it forms only one valid expression given one open bracket.
            long[] validExpressions = new long[pattern.Length];
            validExpressions[1] = 1;

            for (int s = pattern.Length - 2; s >= 0; s--)
            {
                long[] newValidExpressions = new long[s + 1];

                for (int i = 0; i < newValidExpressions.Length; i++)
                {
                    if (pattern[s] == '?' || pattern[s] == '(')
                    {
                        newValidExpressions[i] += validExpressions[i + 1];
                    }

                    if (i != 0 && (pattern[s] == '?' || pattern[s] == ')'))
                    {
                        newValidExpressions[i] += validExpressions[i - 1];
                    }
                }

                validExpressions = newValidExpressions;
            }

            return validExpressions[0];
        }
    }
}