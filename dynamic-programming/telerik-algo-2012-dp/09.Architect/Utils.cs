namespace Architect
{
    static class Utils
    {
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
    }
}