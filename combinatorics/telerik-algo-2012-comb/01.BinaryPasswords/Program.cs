namespace BinaryPasswords
{
    using System;
    using System.Numerics;
    using System.Linq;    

    static class Program
    {
        static void Main()
        {
            string pattern = Console.ReadLine();

            int missingCount = pattern.Count(@char => @char == '*');

            Console.WriteLine(BigInteger.Pow(2, missingCount));
        }
    }
}