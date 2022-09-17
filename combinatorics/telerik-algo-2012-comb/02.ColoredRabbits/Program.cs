namespace ColoredRabbits
{
    using System;
    using System.Collections.Generic;
    using System.Linq;    

    static class Program
    {
        static void Main()
        {
            int numbersCount = int.Parse(Console.ReadLine());

            int finishedGroupsSum = 0;

            var groups = new Dictionary<int, int>();

            for (int i = 0; i < numbersCount; i++)
            {
                int groupSize = int.Parse(Console.ReadLine()) + 1;

                if (groups.ContainsKey(groupSize))
                {
                    groups[groupSize]++;
                }
                else
                {
                    groups[groupSize] = 1;
                }

                if (groups[groupSize] == groupSize)
                {
                    finishedGroupsSum += groupSize;

                    groups.Remove(groupSize);
                }
            }

            Console.WriteLine(groups.Keys.Sum() + finishedGroupsSum);
        }
    }
}