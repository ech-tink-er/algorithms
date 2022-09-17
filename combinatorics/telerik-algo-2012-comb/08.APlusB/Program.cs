namespace APlusB
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;

    static class Program
    {
        static void Main()
        {
            string expression = Console.ReadLine();
            int exponent = int.Parse(Console.ReadLine());

            char a = expression[1];
            char b = expression[3];

            Console.WriteLine(Formulate(a, b, exponent));
        }

        static string Formulate(char a, char b, int exponent)
        {
            if (exponent < 1)
            {
                return "1";
            }

            StringBuilder formula = new StringBuilder();

            var pascalsLine = PascalsLine(exponent);

            for (int i = 0; i < pascalsLine.Length - 1; i++)
            {
                int aExponent = exponent - i;
                int bExponent = i;

                if (pascalsLine[i] != 1)
                {
                    formula.Append(pascalsLine[i].ToString());
                }

                formula.Append(ExponNotation(a, aExponent));
                formula.Append(ExponNotation(b, bExponent));

                formula.Append('+');
            }

            formula.Append(ExponNotation(b, exponent));

            return formula.ToString();
        }

        static string ExponNotation(char var, int exponent)
        {
            if (exponent == 0)
            {
                return "";
            }

            return $"({var}^{exponent})";
        }

        static int[] PascalsLine(int index)
        {
            int length = index + 1;

            int[] line = new int[length];

            for (int i = 0; i < (length / 2) + 1; i++)
            {
                line[i] = CalcCombinationsCount(index, i);
            }

            for (int i = 0; i < length / 2; i++)
            {
                line[length - 1 - i] = line[i];
            }

            return line;
        }

        static int CalcCombinationsCount(int total, int choose)
        {
            BigInteger permutationsCount = Factorial(total, choose);

            return (int)(permutationsCount / Factorial(choose));
        }

        static BigInteger Factorial(int number, int length = int.MaxValue)
        {
            BigInteger result = 1;

            for (int i = number, c = 0; i >= 2 && c < length; i--, c++)
            {
                result *= i;
            }

            return result;
        }
    }
}