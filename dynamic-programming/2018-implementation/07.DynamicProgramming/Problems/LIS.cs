namespace DynamicProgramming
{
    // Longest Increasing Subsequence
    class LIS
    {
        private class Result
        {
            public Result(int length, int last)
            {
                this.Length = length;
                this.Last = last;
            }

            public int Length { get; set; }

            public int Last { get; set; }

            public Result Clone()
            {
                return new Result(this.Length, this.Last);
            }
        }

        private int[] sequence;

        private int[] cache;

        public LIS(int[] sequence)
        {
            this.sequence = sequence;
            this.cache = new int[this.sequence.Length];
        }

        public int Recursive()
        {
            int max = 1;

            for (int i = 1; i < this.sequence.Length; i++)
            {
                int result = this.Recursive(i);

                if (result > max)
                {
                    max = result;
                }
            }

            return max;
        }

        public int Recursive(int index)
        {
            if (this.cache[index] != 0)
            {
                return this.cache[index];
            }

            if (index < 0)
            {
                return 0;
            }

            int longest = 1;
            for (int i = index - 1; i >= 0; i--)
            {
                if (this.sequence[index] <= this.sequence[i])
                {
                    continue;
                }

                int result = this.Recursive(i) + 1;

                if (result > longest)
                {
                    longest = result;
                }
            }

            this.cache[index] = longest;
            return longest;
        }

        public int Iterative()
        {
            int[] results = new int[this.sequence.Length];
            results[0] = 1;

            int max = results[0];
            for (int i = 1; i < results.Length; i++)
            {
                for (int j = i - 1; j >= 0; j--)
                {
                    if (this.sequence[i] > this.sequence[j] &&
                        results[i] < results[j] + 1)
                    {
                        results[i] = results[j] + 1;
                    }
                }

                if (results[i] > max)
                {
                    max = results[i];
                }
            }

            return max;
        }
    }
}