namespace TreeEdit.Commands
{
    using System.Linq;
    using System.Collections.Generic;

    using static IO;
    using static Parsing;
    using static Validation;

    internal static class SetsCommands
    {
        public static readonly SortedDictionary<string, string> Details = new SortedDictionary<string, string>()
        {
            { $"{SetsCmd} [{{-s|-l}}]", "Saves, loads, or lists the sets." },
            { $"{PickCmd} <set>", "Sets the tree to the specified set." },
            { $"{SetCmd} <[-t] [-s <set>] [-r <count>] [numbers...]>", "Defines a new set." },
            { $"{UnsetCmd} <set>", "Deletes a set." },
        };

        public const string SetsCmd = "sets";
        public const string PickCmd = "pick";
        public const string SetCmd = "set";
        public const string UnsetCmd = "unset";

        public static void Sets(List<string> parameters)
        {
            if (parameters.Contains("-s"))
            {
                SaveSets();
                OutBuffer.AppendLine("Sets saved.");
            }
            else if (parameters.Contains("-l"))
            {
                LoadSets();
                OutBuffer.AppendLine("Sets loaded.");
            }
            else if (State.Sets.Any())
            {
                OutBuffer.AppendLine("Sets:");
                for (int i = 0; i < State.Sets.Count; i++)
                {
                    PrintSet(i);
                }
            }
            else
            {
                OutBuffer.AppendLine("There are no sets.");
            }
        }

        public static void Pick(List<string> parameters)
        {
            int set = ParseSet(parameters, 0);

            State.Tree.Clear();
            State.Tree.Add(State.Sets[set]);

            OutBuffer.AppendLine($"Set {set + 1} applied.");
        }

        public static void Set(List<string> parameters)
        {
            var set = new List<int>();

            int s = ParseSet(parameters);
            if (s != -1)
            {
                set.AddRange(State.Sets[s]);
            }

            if (parameters.Remove("-t"))
            {
                set.AddRange(State.Tree);
            }

            set.AddRange(ParseRandom(parameters));

            set.AddRange(ParseNumbers(parameters));

            ValidateAny(set, type: "set");

            State.Sets.Add(set.ToArray());

            OutBuffer.AppendLine("Set added:");
            PrintSet(State.Sets.Count - 1);
        }

        public static void Unset(List<string> parameters)
        {
            int set = ParseSet(parameters, 0);

            State.Sets.RemoveAt(set);

            OutBuffer.AppendLine($"Set {set + 1} removed.");
        }
    }
}