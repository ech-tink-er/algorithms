namespace DynamicProgramming
{
    using System.Collections.Generic;

    class Fibonacci
    {
        private int n;

        private readonly Dictionary<int, int> cache;

        public Fibonacci(int n)
        {
            this.n = n;

            this.cache = new Dictionary<int, int>();
        }

        public int Recursive()
        {
            return this.Recursive(this.n);
        }

        public int Recursive(int n)
        {
            if (n <= 1)
            {
                return 0;
            }
            else if (n == 2)
            {
                return 1;

            }

            if (!this.cache.ContainsKey(n))
            {
                this.cache[n] = this.Recursive(n - 1) + this.Recursive(n - 2); 
            }

            return this.cache[n];
        }

        public int Iterative()
        {
            int first = 0;
            if (this.n <= 1)
            {
                return first;
            }

            int second = 1;
            if (this.n == 2)
            {
                return second;
            }

            for (int i = 3; i <= this.n; i++)
            {
                int result = first + second;

                first = second;
                second = result;
            }

            return second;
        }
    }
}