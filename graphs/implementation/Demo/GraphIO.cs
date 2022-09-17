namespace Demo
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    using Utilities;

    using Graphs;
    using Graphs = System.Collections.Generic.Dictionary<string, Graphs.Graph<char>>;

    public static class GraphIO
    {
        private class EdgeRecord
        {
            public EdgeRecord(char source, char[] destinations, int weight, bool directed)
            {
                this.Source = source;
                this.Destinations = destinations;
                this.Weight = weight;
                this.Directed = directed;
            }

            public char Source { get; }
            public char[] Destinations { get; }
            public int Weight { get; }
            public bool Directed { get; }
        }

        private static readonly Regex FindEdges = new Regex(@"^(\w+)(?:\s*(<)?(?:\((-?\d+)\))?>\s*(.+))?", RegexOptions.Multiline);

        public static Graph<char> ReadGraph(string path)
        {
            var graph = new Graph<char>();

            var records = ReadEdgeRecords(path);

            foreach (var record in records)
            {
                foreach (var destination in record.Destinations)
                {
                    graph.AddEdge(record.Source, destination, record.Weight, record.Directed);
                }
            }

            return graph;
        }

        public static FlowNet<char> ReadFlowNet(string path)
        {
            var records = ReadEdgeRecords(path);

            var sources = new List<char>();
            var destinations = new List<char>();
            var weights = new List<int>();

            foreach (var record in records)
            {
                foreach (var destination in record.Destinations)
                {
                    sources.Add(record.Source);
                    destinations.Add(destination);
                    weights.Add(record.Weight);
                }
            }

            return new FlowNet<char>(sources.ToArray(), destinations.ToArray(), weights.ToArray());
        }

        public static Graphs ReadGraphs(string dir, params string[] markers)
        {
            var graphs = new Graphs();

            var take = new List<string>();
            var skip = new List<string>();
            foreach (var marker in markers)
            {
                if (string.IsNullOrEmpty(marker))
                    continue;

                if (1 < marker.Length && marker[0] == '-')
                    skip.Add(marker.Substring(1));
                else
                    take.Add(marker);
            }

            foreach (var file in new DirectoryInfo(dir).GetFiles())
            {
                string filename = file.Name.ToLower();
                if ((take.Any() && !take.Any(m => filename.Contains(m))) || 
                    (skip.Any() && skip.Any(m => filename.Contains(m))))
                    continue;

                graphs[file.Name] = ReadGraph(file.FullName);
            }

            return graphs;
        }

        public static Graphs[] LoadGraphs(string dir)
        {
            Graphs graphs = ReadGraphs(dir, "-nwc", "-dag2.txt");
            Graphs undirected = ReadGraphs(dir, "u");
            Graphs unweighted = ReadGraphs(dir);
            Graphs dags = ReadGraphs(dir, "dag");
            Graphs negativeWeightCycles = ReadGraphs(dir, "nwc");

            foreach (var graph in unweighted.Values)
                foreach (var edge in graph.Edges)
                    edge.Weight = 1;

            return new Graphs[] { graphs, undirected, unweighted, dags, negativeWeightCycles };
        }

        private static EdgeRecord[] ReadEdgeRecords(string path)
        {
            var records = new List<EdgeRecord>();

            string file = File.ReadAllText(path);

            var matches = FindEdges.Matches(file);
            foreach (Match match in matches)
            {
                char source = match.Groups[1].Value[0];
                char[] destinations = match.Groups[4].Value.Split(" ", StringSplitOptions.RemoveEmptyEntries)
                    .Select(d => d[0])
                    .ToArray();

                bool directed = match.Groups[2].Value == "";
                if (!int.TryParse(match.Groups[3].Value, out int weight))
                    weight = Edge<char>.DefaultWeight;

                foreach (var destination in destinations)
                    records.Add(new EdgeRecord(source, destinations, weight, directed));
            }

            return records.ToArray();
        }
    }
}