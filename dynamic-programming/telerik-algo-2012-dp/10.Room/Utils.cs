namespace Room
{
    using System;

    static class Utils
    {        
        public const char Free = '.';

        public const char Pillar = '#';

        public const char Horizontal = '-';

        public const char Vertical = '|';

        public static int PowOf2(int power)
        {
            return 1 << power;
        }

        public static int SetBit(int number, int i, bool value)
        {
            int mask = 1 << i;

            if (value)
            {
                number = number | mask;
            }
            else
            {
                number = number & ~mask;
            }

            return number;
        }

        public static bool GetBit(int number, int i)
        {
            int mask = 1 << i;

            int result = (number & mask) >> i;

            return result == 1;
        }

        public static int InsertLeft(int number, bool value, int length)
        {
            int mask = ~(1 << length);

            return Utils.SetBit(number << 1, 0, value) & mask;
        }

        public static T[,] ToMatrix<T>(T[] array, int colsCount)
        {
            if (array.Length % colsCount != 0)
            {
                throw new ArgumentException("Array length must be divisible by colsCount for valid matrix!");
            }

            T[,] matrix = new T[array.Length / colsCount, colsCount];

            for (int row = 0; row < matrix.GetLength(0); row++)
            {
                for (int col = 0; col < matrix.GetLength(1); col++)
                {
                    int index = (row * matrix.GetLength(1)) + col;

                    matrix[row, col] = array[index];
                }
            }

            return matrix;
        }
    }
}