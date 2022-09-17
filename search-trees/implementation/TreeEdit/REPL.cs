namespace TreeEdit
{
    using System;
    using System.IO;
    using System.Collections.Generic;

    using static IO;
    using static State;
    using static Parsing;

    internal sealed class REPL
    {
        public REPL()
        {
            this.Exit = false;
            this.VerbosePrompt = true;
        }

        public bool Exit { get; set; }
        public bool VerbosePrompt { get; set; }

        public void Start()
        {
            this.Exit = false;

            while (!this.Exit)
            {
                this.Prompt();
                string command = ParseCommandLine(Input.ReadLine(), out List<string> parameters);

                try
                {
                    var action = ParseCommand(command);
                    action(parameters);

                    Output.WriteLine(OutBuffer.ToString());
                    OutBuffer.Clear();
                }
                catch (Exception ex)
                {
                    if (ex is ApplicationException || ex is FormatException || ex is IOException)
                    {
                        Error.WriteLine(ex.Message + Environment.NewLine);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        private void Prompt()
        {
            string prompt = "TreeEdit>";
            if (this.VerbosePrompt)
            {
                prompt = "|{0}|{1}|{2}\n" + prompt;
            }

            Output.Write(prompt, Util.TreeToString(Tree, verbose: false), Tree.Count, Tree.ToString());
        }
    }
}