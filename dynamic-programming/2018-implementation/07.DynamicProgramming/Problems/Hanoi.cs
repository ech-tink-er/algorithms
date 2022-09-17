namespace DynamicProgramming
{
    class Hanoi
    {
        private int count;

        public Hanoi(int count)
        {
            this.count = count;
        }

        public int Recursive()
        {
            return this.Recursive(this.count);
        }

        public int Recursive(int count)
        {
            if (count == 1)
            {
                return 1;
            }
            else if (count < 1)
            {
                return 0;
            }

            return  1 + (2 * Recursive(count - 1));
        }

        public int Iterative()
        {
            int result = 1;

            for (int i = 0; i < this.count - 1; i++)
            {
                result = 1 + (2 * result);
            }

            return result;
        }
    }
}