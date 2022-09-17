namespace DynamicProgramming
{
    using System.Linq;

    class SubsetSum
    {
        private int sum;

        private int[] set;

        private bool?[,] cache;

        public SubsetSum(int sum, int[] set)
        {
            this.sum = sum;

            this.set = set;

            this.cache = new bool?[this.set.Sum() + 1, this.set.Length];
        }

        public bool Recursive()
        {
            return this.Recursive(this.sum);
        }

        public bool Recursive(int sum, int index = 0)
        {
            if (sum == 0)
            {
                return true;
            }
            else if (sum < 0 || index >= this.set.Length)
            {
                return false;
            }

            if (this.cache[sum, index] != null)
            {
                return (bool)this.cache[sum, index];
            }

            bool result = this.Recursive(sum, index + 1) || this.Recursive(sum - this.set[index], index + 1);

            this.cache[sum, index] = result;
            return result;
        }

        public bool Iterative()
        {
            if (this.sum == 0)
            {
                return true;
            }

            bool[] oldSums = new bool[this.set.Sum() + 1];
            bool[] newSums = new bool[oldSums.Length];
            oldSums[0] = true;

            for (int i = 0; i < this.set.Length; i++)
            {
                for (int s = 0; s < oldSums.Length; s++)
                {
                    if (!oldSums[s])
                    {
                        continue;
                    }

                    newSums[s] = true;

                    int newSum = s + this.set[i];

                    if (newSum == this.sum)
                    {
                        return true;
                    }

                    newSums[newSum] = true;
                }

                var hold = oldSums;
                oldSums = newSums;
                newSums = hold;
            }

            return false;
        }
    }
}