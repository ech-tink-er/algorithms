namespace TreeEdit
{
    using System.IO;
    using System.Linq;

    using static State;

    internal static class Start
    {
        public static void Main()
        {
            Init();
            IO.REPL.Start();
        }

        public static void Init()
        {
            try
            {
                IO.LoadSets();
            }
            catch (IOException)
            { }

            if (Sets.Any())
            {
                Tree.Add(Sets.First());
            }
        }
    }
}