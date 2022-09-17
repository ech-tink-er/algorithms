namespace DynamicProgramming
{
    using System;

    class Min
    {
        private int[] numbers;
        
        public Min(int[] numbers)
        {
            this.numbers = numbers;
        }

        public int Recursive(int index = 0)
        {
            if (index == this.numbers.Length - 1)
            {
                return this.numbers[index];
            }

            return Math.Min(this.numbers[index], this.Recursive(index + 1));
        }
        
        public int Iterative()
        {
            int min = this.numbers[0];

            for (int i = 1; i < this.numbers.Length; i++)
            {
                if (this.numbers[i] < min)
                {
                    min = this.numbers[i];
                }
            }

            return min;
        }
    }
}