namespace TreeEdit.Commands
{
    using System.IO;
    using System.Linq;
    using System.Collections.Generic;

    using SearchTrees;
    using Utilities;

    using static State;
    using static IO;
    using static Parsing;
    using static Validation;
    using static Util;

    internal static class TreeCommands
    {
        public static readonly SortedDictionary<string, string> Details = new SortedDictionary<string, string>()
        {
            { $"{AddCmd} <[-s <set>] [numbers...]>", "Adds numbers to the tree." },
            { $"{RemoveCmd} <[-a] [-s <set>] [numbers...]>", "Removes numbers from the tree." },
            { CountCmd, "Prints the number of elements in the tree." },
            { $"{HasCmd} <[-s <set>] [numbers...]>", "Checks wheter the numbers are present in the tree." },
            { MinCmd, "Prints the minimum number in the tree." } ,
            { MaxCmd, "Prints the maximum number in the tree." } ,
            { TypesCmd, "Lists the tree types." },
            { $"{TypeCmd} [type] [order]", "Prints or sets the tree type." },
            { $"{PrintCmd} [-v] {{-l|-h}} [-f [path]]", "Prints the contents of the tree." },
        };

        public const string AddCmd = "add";
        public const string RemoveCmd = "rem";
        public const string CountCmd = "count";
        public const string HasCmd = "has";
        public const string MinCmd = "min";
        public const string MaxCmd = "max";
        public const string TypesCmd = "types";
        public const string TypeCmd = "type";
        public const string PrintCmd = "print";

        public static void Add(List<string> parameters)
        {
            ValidateAny(parameters, action: "add");

            int set = ParseSet(parameters);
            if (set != -1)
            {
                Tree.Add(Sets[set]);
            }

            Tree.Add(ParseNumbers(parameters));

            OutBuffer.AppendLine("Numbers added.");
        }

        public static void Remove(List<string> parameters)
        {
            ValidateAny(parameters, action: "remove");

            if (parameters.Contains("-a"))
            {
                Tree.Clear();

                OutBuffer.AppendLine("All numbers removed.");
                return;
            }

            var numbers = new List<int>();

            int set = ParseSet(parameters);
            if (set != -1)
            {
                numbers.AddRange(Sets[set]);
            }

            numbers.AddRange(ParseNumbers(parameters));

            var removed = new List<int>();
            var missing = new List<int>();
            foreach (var number in numbers)
            {
                if (Tree.Remove(number))
                {
                    removed.Add(number);
                }
                else
                {
                    missing.Add(number);
                }
            }

            string rem = !missing.Any() ? "" : $" {{{removed.Join(" ")}}}";
            string mis = !removed.Any() ? "" : $" {{{missing.Join(" ")}}}";

            if (removed.Any())
            {
                OutBuffer.AppendLine($"Numbers{rem} removed.");
            }

            if (missing.Any())
            {
                OutBuffer.AppendLine($"Numbers{mis} missing.");
            }
        }
        
        public static void Count(List<string> parameters)
        {
            OutBuffer.AppendLine($"The tree has {Tree.Count} elements.");
        }

        public static void Has(List<string> parameters)
        {
            ValidateAny(parameters, action: "check");

            var numbers = new List<int>();

            int set = ParseSet(parameters);
            if (set != -1)
            {
                numbers.AddRange(Sets[set]);
            }

            numbers.AddRange(ParseNumbers(parameters));

            for (int i = 0; i < numbers.Count; i++)
            {
                string result = Tree.Contains(numbers[i]) ? "YES" : "NO";

                OutBuffer.AppendLine($"{numbers[i]}: {result}");
            }
        }

        public static void Min(List<string> parameters)
        {
            ValidateTreeNonEmpty();

            OutBuffer.AppendLine($"Min: {Tree.Min()}");
        }

        public static void Max(List<string> parameters)
        {
            ValidateTreeNonEmpty();

            OutBuffer.AppendLine($"Max: {Tree.Max()}");
        }

        public static void Types(List<string> parameters)
        {
            OutBuffer.AppendLine("Types:");
            foreach (var tree in TreeTypeToString.Values)
            {
                OutBuffer.AppendLine(tree);
            }
        }

        public static void Type(List<string> parameters)
        {
            if (!parameters.Any())
            {
                OutBuffer.AppendLine($"Currently using {TreeToString(Tree, verbose: true)}.");
                return;
            }

            string tree = parameters.First().ToUpper();
            switch (tree)
            {
                case "BIN":
                    Tree = new BinTree<int>(Tree);
                    break;
                case "AVL":
                    Tree = new AVLTree<int>(Tree);
                    break;
                case "AA":
                    Tree = new AATree<int>(Tree);
                    break;
                case "RB":
                    Tree = new RBTree<int>(Tree);
                    break;
                case "B":
                    int order = 3;
                    if (1 < parameters.Count)
                    {
                        order = ParseNumber(parameters[1]);
                    }

                    if (order < BTree<int>.MinOrder)
                    {
                        throw new InvalidCommandException($"B tree order can't be less than {BTree<int>.MinOrder}.");
                    }

                    Tree = new BTree<int>(Tree, order);
                    break;
                default:
                    throw new InvalidCommandException($"Invalid tree type. Type '{TypesCmd}' to view tree types.");
            }

            OutBuffer.AppendLine($"Switched to {TreeToString(Tree, verbose: true)}.");
        }

        public static void Print(List<string> parameters)
        {
            bool list = parameters.Remove("-l");
            bool horizontal = parameters.Remove("-h");
            bool verbose = parameters.Remove("-v");

            TreeStringFormat format = horizontal ? TreeStringFormat.Horizontal : TreeStringFormat.Vertical;
            format = list ? TreeStringFormat.List : format;
            string str = Tree.ToString(format, verbose);

            var i = parameters.IndexOf("-f");

            if (i == -1)
            {
                OutBuffer.AppendLine(str);
            }
            else
            {
                string path = i + 1 < parameters.Count ? parameters[i + 1] : "tree.txt";
                string directory = Path.GetDirectoryName(Path.GetFullPath(path));

                if (!Directory.Exists(directory))
                {
                    throw new DirectoryNotFoundException($"No {{{directory}}} directory.");
                }

                File.WriteAllText(path, str);

                OutBuffer.AppendLine($"Printed to file '{path}'.");
            }
        }
    }
}