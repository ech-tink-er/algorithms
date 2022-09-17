namespace DynamicProgramming
{
    using System;

    // Longest Common Subsequence
    class LCS
    {
        private readonly string first;

        private readonly string second;

        private readonly int?[,] cache;

        public LCS(string first, string second)
        {
            this.first = first;
            this.second = second;

            this.cache = new int?[this.first.Length + 1, this.second.Length + 1];
        }

        public int Recursive()
        {
            return this.Recursive(this.first.Length, this.second.Length);
        }

        private int Recursive(int firstLength, int secondLength)
        {
            if (firstLength == 0 || secondLength == 0)
            {
                return 0;
            }

            if (this.cache[firstLength, secondLength] != null)
            {
                return (int)this.cache[firstLength, secondLength];
            }

            int result = 0;
            if (this.first[firstLength - 1] ==  this.second[secondLength - 1])
            {
                result =  1 + this.Recursive(firstLength - 1, secondLength - 1);
            }
            else
            {
                result = Math.Max(this.Recursive(firstLength - 1, secondLength), this.Recursive(firstLength, secondLength - 1));
            }

            this.cache[firstLength, secondLength] = result;

            return result;
        }

        public int Iterative()
        {
            int[,] results = new int[this.first.Length + 1, this.second.Length + 1];

            for (int fLen = 1; fLen < results.GetLength(0); fLen++)
            {
                for (int sLen = 1; sLen < results.GetLength(1); sLen++)
                {
                    if (this.first[fLen - 1] == this.second[sLen - 1])
                    {
                        results[fLen, sLen] = results[fLen - 1, sLen - 1] + 1;
                    }
                    else
                    {
                        results[fLen, sLen] = Math.Max(results[fLen - 1, sLen], results[fLen, sLen - 1]);
                    }
                }
            }

            return results[this.first.Length, this.second.Length];
        }
    }
}