namespace Demo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Utilities;

    using Graphs;
    using static GraphIO;
    using static Graphs.Traversals;

    static class Demos
    {
        private readonly static Dictionary<string, Graph<char>> Graphs;
        private readonly static Dictionary<string, Graph<char>> Undirected;
        private readonly static Dictionary<string, Graph<char>> Unweighted;
        private readonly static Dictionary<string, Graph<char>> Dags;
        private readonly static Dictionary<string, Graph<char>> NegativeWeightCycles;
        private readonly static FlowNet<char> FlowNet;

        static Demos()
        {
            var graphs = LoadGraphs(@"graphz");

            Graphs = graphs[0];
            Undirected = graphs[1];
            Unweighted = graphs[2];
            Dags = graphs[3];
            NegativeWeightCycles = graphs[4];

            FlowNet = ReadFlowNet(@"graphz\flow-net.txt");
        }

        public static void DemoBFS()
        {
            foreach (var pair in Graphs)
            {
                string filename = pair.Key;
                var graph = pair.Value;

                Console.WriteLine($"Running BFS on {filename}:");

                var vertices = new List<List<char>>();
                BreadthTraverse(graph, (v, l, _) =>
                {
                    if (l == 0)
                        vertices.Add(new List<char>());

                    vertices.Last().Add(v.Key);

                    return false;
                });

                string result = string.Join(", ", vertices.Select(list => string.Join(" -> ", list)));
                Console.WriteLine(result);
            }
        }

        public static void DemoDFS()
        {
            foreach (var pair in Graphs)
            {
                string filename = pair.Key;
                var graph = pair.Value;

                Console.WriteLine($"Running DFS on {filename}:");

                var vertices = new List<List<char>>();
                vertices.Add(new List<char>());

                DepthTraverse(graph, (vertex, visited) =>
                {
                    var last = vertices.Last();
                    last.Add(vertex.Key);

                    if (visited.Count(v => !v.Value) == 1)
                    {
                        last.Reverse();
                        vertices.Add(new List<char>());
                    }

                    return false;
                });

                vertices.RemoveAt(vertices.Count - 1);

                string result = string.Join(", ", vertices.Select(list => string.Join(" -> ", list)));
                Console.WriteLine(result);
            }
        }

        public static void DemoShortestPahts()
        {
            foreach (var pair in Graphs)
            {
                string filename = pair.Key;
                var graph = pair.Value;

                var source = PickSource(graph);
                var paths = graph.ShortestPaths(source);

                PrintPaths(pair, source, paths);
            }
        }

        public static void DemoEdgewiseShortestPahts()
        {
            foreach (var pair in Unweighted)
            {
                string filename = pair.Key;
                var graph = pair.Value;

                var source = PickSource(graph);
                var paths = graph.ShortestPaths(source, edgewise: true);

                PrintPaths(pair, source, paths);
            }
        }

        public static void DemoDAGShortestPaths()
        {
            foreach (var pair in Dags)
            {
                var graph = pair.Value;

                var source = PickSource(graph);
                var paths = graph.ShortestPaths(source, dag: true);

                PrintPaths(pair, source, paths);
            }
        }

        public static void DemoSafeShortestPaths()
        {
            var selection = NegativeWeightCycles.Concat(Graphs)
                .Concat(Dags.Where(p => p.Key == "dag2.txt"))
                .Distinct();
            foreach (var pair in selection)
            {
                var graph = pair.Value;

                var source = PickSource(graph);

                try
                {
                    var paths = graph.ShortestPaths(source, safe: true);
                    PrintPaths(pair, source, paths);
                }
                catch (CycleException exception)
                {
                    Console.WriteLine($"Negative weight cycle detected in {pair.Key}.");
                }
            }
        }

        public static void DemoTopoSort()
        {
            foreach (var pair in Dags)
            {
                var graph = pair.Value;

                var sorted = graph.TopoSort()
                    .Select(v => v.Key)
                    .ToArray();

                Console.WriteLine($"{pair.Key} topologically sorted:");
                Console.WriteLine(sorted.Join() + "\n");
            }
        }

        public static void DemoMinSpanningTree()
        {
            foreach (var pair in Undirected)
            {
                var graph = pair.Value;

                var root = PickSource(graph);
                var mst = ReverseTreeDirection(graph.MinSpanningTree(root));
                int weight = mst.Values.SelectMany(e => e).Sum(e => e.Weight);

                Console.WriteLine($"Minimum Spanning Tree of {pair.Key}, Tree Weight = {weight}:");
                PrintTree(mst, root);
            }
        }

        public static void DemoMinSpanningForest()
        {
            foreach (var pair in Undirected)
            {
                var graph = pair.Value;

                var msf = ReverseTreeDirection(graph.MinSpanningForest());
                int weight = msf.Values.SelectMany(e => e).Sum(e => e.Weight);

                Console.WriteLine($"Minimum Spanning Forest of {pair.Key}, Forest Weight = {weight}:");

                var roots = msf.Keys.Where(v => msf.Values.SelectMany(e => e).All(e => e.Destination != v))
                    .ToArray();
                foreach (var root in roots)
                {
                    PrintTree(msf, root);
                }
            }
        }

        public static void DemoMaxFlow()
        {
            char source = 'A';
            char target = 'G';

            FlowNet.Clear();
            var maxFlow = Flow.MaxFlow(FlowNet, source, target);

            Console.WriteLine($"The max flow from {source} to {target} in flow-net.txt is {maxFlow}.");
        }

        private static Vertex<char> PickSource(Graph<char> graph)
        {
            var source = graph.Vertices.OrderBy(v => v.Key).First();
            if (graph.HasVertex('S'))
                source = graph['S'];
            else if (graph.HasVertex('A'))
                source = graph['A'];
            else if (graph.HasVertex('G'))
                source = graph['G'];

            return source;
        }

        private static Dictionary<Vertex<char>, LinkedList<Edge<char>>> ReverseTreeDirection(Dictionary<Vertex<char>, Edge<char>> tree)
        {
            var result = new Dictionary<Vertex<char>, LinkedList<Edge<char>>>();
            foreach (var vertex in tree.Keys)
                result[vertex] = new LinkedList<Edge<char>>();

            foreach (var edge in tree.Values)
            {
                if (edge.Source == edge.Destination)
                    continue;

                var inverse = edge.Inverse;

                result[inverse.Source].AddLast(inverse);
            }

            return result;
        }

        private static void PrintTree(Dictionary<Vertex<char>, LinkedList<Edge<char>>> tree, Vertex<char> vertex, Edge<char> incoming = null, int depth = 0)
        {
            string weight = "";
            if (incoming != null)
                weight = $"({incoming.Weight})> ";

            Console.WriteLine(new string(' ', 2 * depth) + weight + vertex.Key);

            foreach (var edge in tree[vertex])
                PrintTree(tree, edge.Destination, edge, depth + 1);
        }

        private static void PrintPath(Paths<char> paths, Vertex<char> destination)
        {
            string length = paths.Lengths[destination].ToString();
            if (paths.Lengths[destination] == Paths<char>.Unreachable)
                length = "∞";

            Console.WriteLine($"\tTo: {destination.Key} | Length: {length}");

            if (paths.Lengths[destination] == Paths<char>.Unreachable)
            {
                Console.WriteLine("\tUnreachable");
                return;
            }

            var path = new List<Vertex<char>>();
            var current = destination;
            while (current != null)
            {
                path.Add(current);

                current = paths.Previous[current];
            }

            path.Reverse();

            Console.WriteLine("\t" + path.Select(v => v.Key).Join(" -> "));
        }

        private static void PrintPaths(KeyValuePair<string, Graph<char>> graph, Vertex<char> source, Paths<char> paths)
        {
            Console.WriteLine($"Shortest Paths in {graph.Key}, from ({source.Key}):");
            foreach (var vertex in graph.Value.Vertices)
            {
                PrintPath(paths, vertex);
            }
        }
    }
}