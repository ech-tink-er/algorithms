namespace Guitar
{
    using System.IO;
    using System.Linq;

    using Testing;

    static class Program
    {
        public static void Main()
        {
            EasyTesting.Test("Tests", true, Solution);
        }

        public static void Solution(TextReader reader, TextWriter writer)
        {
            reader.ReadLine();

            int[] changes = reader.ReadLine()
                .Split(' ')
                .Select(int.Parse)
                .ToArray();

            int startVolume = int.Parse(reader.ReadLine());

            int maxVolume = int.Parse(reader.ReadLine());

            int result = FindMaxLastVolume(changes, startVolume, maxVolume);

            writer.WriteLine(result);
        }

        public static int FindMaxLastVolume(int[] changes, int startVolume, int maxVolume)
        {
            bool[] volumes = new bool[maxVolume + 1];
            volumes[startVolume] = true;

            bool[] newVolumes = new bool[volumes.Length];
            for (int c = 0; c < changes.Length; c++)
            {
                bool fail = true;
                for (int v = 0; v < volumes.Length; v++)
                {
                    if (volumes[v] == false)
                    {
                        continue;
                    }

                    int higherVolume = v + changes[c];
                    int lowerVolume = v - changes[c];

                    if (higherVolume <= maxVolume)
                    {
                        newVolumes[higherVolume] = true;
                        fail = false;
                    }

                    if (0 <= lowerVolume)
                    {
                        newVolumes[lowerVolume] = true;
                        fail = false;
                    }
                }

                if (fail)
                {
                    return -1;
                }

                var hold = volumes;
                volumes = newVolumes;
                newVolumes = hold;
                newVolumes.Clear();
            }

            for (int i = volumes.Length - 1; i >= 0; i--)
            {
                if (volumes[i])
                {
                    return i;
                }
            }

            return -1;
        }

        public static void Clear<T>(this T[] array) 
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = default(T);
            }
        }
    }
}