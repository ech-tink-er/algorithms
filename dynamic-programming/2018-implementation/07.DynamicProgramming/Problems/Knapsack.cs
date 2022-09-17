namespace DynamicProgramming
{
    using System;

    class Knapsack
    {
        private int capacity;

        private readonly Product[] products;

        private readonly int?[,] cache;

        public Knapsack(int capacity, Product[] products)
        {
            this.capacity = capacity;
            this.products = products;
            this.cache = new int?[capacity + 1, this.products.Length + 1];
        }

        public int Iterative()
        {
            /* Builds a list of possible sacks given the capacity, index = weight. 
             * All sacks that may contain n products are predicated on all sacks that may contain n-1 products.
             * In case that 2 sacks have the same weight it remembers the one with the highest value.
             */
            int?[] oldSacks = new int?[this.capacity + 1];
            int?[] newSacks = new int?[this.capacity + 1];
            oldSacks[0] = 0;

            int max = 0;
            for (int i = 0; i < this.products.Length; i++)
            {
                for (int w = 0; w < oldSacks.Length; w++)
                {
                    if (oldSacks[w] == null)
                    {
                        continue;
                    }

                    if (newSacks[w] == null || oldSacks[w] > newSacks[w])
                    {
                        newSacks[w] = oldSacks[w];
                    }

                    int newWeight = w + this.products[i].Weight;
                    if (newWeight > this.capacity)
                    {
                        continue;
                    }

                    int newValue = (int)oldSacks[w] + this.products[i].Value;

                    newSacks[newWeight] = newValue;

                    if (newValue > max)
                    {
                        max = newValue;
                    }
                }

                var hold = oldSacks;
                oldSacks = newSacks;
                newSacks = hold;
            }

            return max;
        }

        public int Recursive()
        {
            return this.Recursive(this.capacity, this.products.Length);
        }

        private int Recursive(int capacity, int length)
        {
            if (length < 1)
            {
                return 0;
            }

            if (this.cache[capacity, length] != null)
            {
                return (int)this.cache[capacity, length];
            }

            int noTake = this.Recursive(capacity, length - 1);

            int last = length - 1;
            if (this.products[last].Weight > capacity)
            {
                return noTake;
            }

            int take = this.products[last].Value + this.Recursive(capacity - this.products[last].Weight, length - 1);

            int result = Math.Max(noTake, take);

            this.cache[capacity, length] = result;
            return result;
        }
    }
}