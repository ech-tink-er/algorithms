namespace Graphs
{
    using System.Linq;
    using System.Collections.Generic;

    public delegate bool Visit<T>(Vertex<T> vertex, Dictionary<Vertex<T>, bool> visited);
    public delegate void SimpleVisit<T>(Vertex<T> vertex);

    public delegate bool Discover<T>(Vertex<T> vertex, int level, Vertex<T> source);
    public delegate void SimpleDiscover<T>(Vertex<T> vertex);

    public static class Traversals
    {
        public static void DepthTraverse<T>(Graph<T> graph, SimpleVisit<T> visit)
        {
            DepthTraverse(graph, ToVisit(visit));
        }

        public static void DepthTraverse<T>(Graph<T> graph, Visit<T> visit)
        {
            var visited = new Dictionary<Vertex<T>, bool>();

            foreach (var vertex in graph.Vertices)
            {
                if (visited.ContainsKey(vertex))
                    continue;

                DepthTraverse(vertex, visited, visit);
            }
        }

        public static void DepthTraverse<T>(Vertex<T> source, SimpleVisit<T> visit)
        {
            DepthTraverse(source, ToVisit(visit));
        }

        public static void DepthTraverse<T>(Vertex<T> source, Visit<T> visit)
        {
            DepthTraverse(source, new Dictionary<Vertex<T>, bool>(), visit);
        }

        private static bool DepthTraverse<T>(
            Vertex<T> source,
            Dictionary<Vertex<T>, bool> visited,
            Visit<T> visit
        )
        {
            visited[source] = false;

            foreach (var edge in source.Edges)
            {
                if (visited.ContainsKey(edge.Destination))
                    continue;

                if (DepthTraverse(edge.Destination, visited, visit))
                    return true;
            }

            bool terminate = visit(source, visited);
            visited[source] = true;

            return terminate;
        }

        public static void BreadthTraverse<T>(Graph<T> graph, SimpleDiscover<T> discover)
        {
            BreadthTraverse(graph, ToDiscover(discover));
        }

        public static void BreadthTraverse<T>(Graph<T> graph, Discover<T> discover)
        {
            var discovered = new HashSet<Vertex<T>>();

            foreach (var vertex in graph.Vertices)
            {
                if (discovered.Contains(vertex))
                    continue;

                BreadthTraverse(vertex, discovered, discover);
            }
        }

        public static void BreadthTraverse<T>(Vertex<T> source, SimpleDiscover<T> discover)
        {
            BreadthTraverse(source, ToDiscover(discover));
        }

        public static void BreadthTraverse<T>(Vertex<T> source, Discover<T> discover)
        {
            BreadthTraverse(source, new HashSet<Vertex<T>>(), discover);
        }

        private static void BreadthTraverse<T>(
            Vertex<T> source,
            HashSet<Vertex<T>> discovered,
            Discover<T> discover
        )
        {
            var frontier = new Queue<Vertex<T>>();

            discover(source, 0, null);
            discovered.Add(source);
            frontier.Enqueue(source);

            int level = 1;
            while (frontier.Any())
            {
                var count = frontier.Count;
                for (int i = 0; i < count; i++)
                {
                    var current = frontier.Dequeue();

                    foreach (var edge in current.Edges)
                    {
                        if (!discovered.Contains(edge.Destination))
                        {
                            if (discover(edge.Destination, level, current))
                                return;

                            discovered.Add(edge.Destination);
                            frontier.Enqueue(edge.Destination);
                        }
                    }
                }

                level++;
            }
        }

        private static Visit<T> ToVisit<T>(SimpleVisit<T> visit)
        {
            return (vertex, _) => 
            {
                visit(vertex);

                return false;
            }; 
        }

        private static Discover<T> ToDiscover<T>(SimpleDiscover<T> discover)
        {
            return (vertex, _, __) =>
            {
                discover(vertex);

                return false;
            };
        }
    }
}