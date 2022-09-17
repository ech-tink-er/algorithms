namespace TreeEdit.Commands
{
    using System;
    using System.Collections.Generic;

    using static IO;

    internal static class InterfaceCommands
    {
        public static readonly SortedDictionary<string, string> Details = new SortedDictionary<string, string>()
        {
            { HelpCmd, "Lists commands." },
            { ExitCmd, "Exits TreeEdit." },
            { ClearCmd, "Clears the screen." } ,
            { $"{PromptCmd} [-v]", "Sets the prompt." },
        };

        public const string HelpCmd = "help";
        public const string ExitCmd = "exit";
        public const string ClearCmd = "cls";
        public const string PromptCmd = "prompt";

        public static void Help(List<string> parameters)
        {
            const string Format = "{0} - {1}"; 

            OutBuffer.AppendLine("Interface Commands:");
            foreach (var command in InterfaceCommands.Details)
            {
                OutBuffer.AppendLine(string.Format(Format, command.Key, command.Value));
            }

            OutBuffer.AppendLine("\nTree Commands:");
            foreach (var command in TreeCommands.Details)
            {
                OutBuffer.AppendLine(string.Format(Format, command.Key, command.Value));
            }

            OutBuffer.AppendLine("\nSets Commands:");
            foreach (var command in SetsCommands.Details)
            {
                OutBuffer.AppendLine(string.Format(Format, command.Key, command.Value));
            }
        }

        public static void Exit(List<string> parameters)
        {
            REPL.Exit = true;
        }

        public static void Clear(List<string> parameters)
        {
            if (Output == Console.Out)
            {
                Console.Clear();
            }
        }

        public static void Prompt(List<string> parameters)
        {
            REPL.VerbosePrompt = parameters.Contains("-v");
        }
    }
}