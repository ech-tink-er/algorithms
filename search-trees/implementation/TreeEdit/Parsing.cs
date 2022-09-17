namespace TreeEdit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Commands;

    using static State;
    using static Validation;

    internal static class Parsing
    {
        private static readonly Regex FindParameters = new Regex(@"(?:(?:^|[^\\])""([\s\S]+?[^\\])""|([\S]+))");

        public static string ParseCommandLine(string line, out List<string> parameters)
        {
            parameters = FindParameters.Matches(line)
                .Cast<Match>()
                .Select(m => m.Groups[1].Length != 0 ? m.Groups[1].Value : m.Groups[2].Value)
                .Select(p => p.Trim().Replace("\\\"", "\"").Replace(@"\\", @"\"))
                .ToList();

            string command = parameters.FirstOrDefault();

            if (parameters.Any())
            {
                parameters.RemoveAt(0);
            }

            return command;
        }

        public static Action<List<string>> ParseCommand(string command)
        {
            switch (command)
            {
                case InterfaceCommands.ExitCmd:
                    return InterfaceCommands.Exit;
                case InterfaceCommands.HelpCmd:
                    return InterfaceCommands.Help;
                case InterfaceCommands.ClearCmd:
                    return InterfaceCommands.Clear;
                case InterfaceCommands.PromptCmd:
                    return InterfaceCommands.Prompt;

                case SetsCommands.SetsCmd:
                    return SetsCommands.Sets;
                case SetsCommands.PickCmd:
                    return SetsCommands.Pick;
                case SetsCommands.SetCmd:
                    return SetsCommands.Set;
                case SetsCommands.UnsetCmd:
                    return SetsCommands.Unset;

                case TreeCommands.AddCmd:
                    return TreeCommands.Add;
                case TreeCommands.RemoveCmd:
                    return TreeCommands.Remove;
                case TreeCommands.CountCmd:
                    return TreeCommands.Count;
                case TreeCommands.HasCmd:
                    return TreeCommands.Has;
                case TreeCommands.MinCmd:
                    return TreeCommands.Min;
                case TreeCommands.MaxCmd:
                    return TreeCommands.Max;
                case TreeCommands.PrintCmd:
                    return TreeCommands.Print;
                case TreeCommands.TypeCmd:
                    return TreeCommands.Type;
                case TreeCommands.TypesCmd:
                    return TreeCommands.Types;

                case null:
                    return (p) => { };
                default:
                    throw new InvalidCommandException($"Invalid command {{{command}}}. Type '{InterfaceCommands.HelpCmd}' to view commands.");
            }
        }

        public static int[] ParseRandom(List<string> parameters)
        {
            int i = parameters.IndexOf("-r");
            if (i == -1)
            {
                return new int[0];
            }

            parameters.RemoveAt(i);

            ValidateAny(parameters, i, "count");

            int count = ParseNumber(parameters[i]);
            parameters.RemoveAt(i);

            int[] numbers = new int[count];
            for (int n = 0; n < count; n++)
            {
                numbers[n] = Util.Random.Next(-1000, 1001);
            }

            return numbers;
        }

        public static int ParseSet(List<string> parameters, int i = -1)
        {
            if (i < 0)
            {
                i = parameters.IndexOf("-s");
                if (i != -1)
                {
                    parameters.RemoveAt(i);
                }
            }

            if (i == -1)
            {
                return i;
            }

            ValidateAny(parameters, i, "set");

            bool parsed = int.TryParse(parameters[i], out int set);
            parameters.RemoveAt(i);

            if (!parsed || set < 1 || Sets.Count < set)
            {
                throw new InvalidCommandException($"Not a valid set {{{parameters[i]}}}. Type '{SetsCommands.SetsCmd}' to view sets.");
            }

            return --set;
        }

        public static int[] ParseNumbers(IEnumerable<string> strs)
        {
            return strs.Select(ParseNumber)
                .ToArray();
        }

        public static int ParseNumber(string str)
        {
            bool parsed = int.TryParse(str, out int number);

            if (!parsed)
            {
                throw new FormatException($"Not a valid number {{{str}}}.");
            }

            return number;
        }
    }
}