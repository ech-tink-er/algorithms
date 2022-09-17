namespace Demo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using static Demos;

    static class Interpreter
    {
        private const string Line = "-----------------------";

        private static Dictionary<string, Action> Demos = new Dictionary<string, Action>()
        {
            { "bfs", DemoBFS },
            { "dfs", DemoDFS },
            { "sp", DemoShortestPahts },
            { "esp", DemoEdgewiseShortestPahts },
            { "dsp", DemoDAGShortestPaths },
            { "ssp", DemoSafeShortestPaths },
            { "topo", DemoTopoSort },
            { "mst", DemoMinSpanningTree },
            { "msf", DemoMinSpanningForest },
            { "flow", DemoMaxFlow },
        };

        private static Dictionary<string, string> DemosList = new Dictionary<string, string>()
        {
            { "bfs", "Breadth-First Search" },
            { "dfs", "Depth-First Search" },
            { "sp", "Shortest Paths (Dijkstra's Algorithm)" },
            { "esp", "Edgewise Shortest Paths (BFS)"},
            { "dsp", "DAG Shortest Paths" },
            { "ssp", "Safe Shortest Paths (Bellman-Ford Algorithm)" },
            { "topo", "Topological Sort" },
            { "mst", "Minumum Spannig Tree (Prim's Algorithm)" },
            { "msf", "Minumum Spannig Forest (Kruskal's Algorithm)" },
            { "flow", "Maximum Flow (Edmonds-Karp Algorithm)" },
        };

        private const string Help = "Type a demo name to run it, 'list' to see all demos, 'all' to run them, and 'exit' to quit.";

        public static void REPL()
        {
            Console.WriteLine("Graphs Demo");
            Console.WriteLine(Help);
            while (true)
            {
                string command = ReadCommand();
                if (command == "exit")
                    break;
                else if (command == "help")
                    Console.WriteLine(Help);
                else if (command == "list")
                    List();
                else if (command == "all")
                    RunAllDemos();
                else
                    RunDemo(command);

                Console.WriteLine();
            }
        }

        private static void RunAllDemos()
        {
            string[] demos = DemosList.Keys.ToArray();
            for (int i = 0; i < demos.Length - 1; i++)
            {
                RunDemo(demos[i]);
                Console.WriteLine("Press any key to run next demo...\n");
                Console.ReadKey(intercept: true);
            }

            RunDemo(demos[demos.Length - 1]);
        }

        private static void RunDemo(string demo)
        {
            if (!Demos.ContainsKey(demo))
            {
                Console.WriteLine($"No {{{demo}}} demo, type 'list' to see all demos.");
                return;
            }

            string title = PrintTitle(DemosList[demo]);
            Demos[demo]();
            PrintFooter(title.Length);
        }

        private static void List()
        {
            const int NameLength = 8;

            Console.WriteLine("Demos:");
            foreach (var demo in DemosList.Keys)
            {
                Console.WriteLine($"{(demo + ":").PadRight(NameLength)} {DemosList[demo]}");
            }
        }

        private static string ReadCommand()
        {
            Console.Write("=>");
            return Console.ReadLine();
        }

        private static string PrintTitle(string title)
        {
            string line = new string('-', 10);
            string result = $"{line}{title.ToUpper()}{line}";

            Console.WriteLine(result);

            return result;
        }

        private static void PrintFooter(int length = 30)
        {
            Console.WriteLine(new string('-', length));
        }
    }
}