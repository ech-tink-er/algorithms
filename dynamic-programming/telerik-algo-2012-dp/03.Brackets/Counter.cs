namespace Brackets
{
    class Counter
    {
        private string pattern;

        private long?[,] cache;

        public Counter(string pattern)
        {
            this.pattern = pattern;

            this.cache = new long?[this.pattern.Length, this.pattern.Length];
        }

        public long CountValidExpressions(int index = 0, int open = 0)
        {
            if (index >= this.pattern.Length)
            {
                if (open == 0)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }

            if (this.cache[index, open] != null)
            {
                return (long)this.cache[index, open];
            }

            long result = 0;

            if (this.pattern[index] == '?' || this.pattern[index] == '(')
            {
                result += CountValidExpressions(index + 1, open + 1);
            }

            if (open > 0 && (this.pattern[index] == '?' || this.pattern[index] == ')'))
            {
                result += CountValidExpressions(index + 1, open - 1);
            }

            this.cache[index, open] = result;
            return result;
        }
    }
}