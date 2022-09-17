namespace Graphs
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    public static class Flow
    {
        // Edmonds-Karp Algorithm
        public static int MaxFlow<T>(FlowNet<T> net, int source, int target)
        {
            int maxFlow = 0;

            var parents = FindPath(net, source, target);
            while (parents[target] != -1)
            {
                int flow = int.MaxValue;

                Traverse(parents, target, (s, d) => 
                {
                    if (net.RemainingCapacity(s, d) < flow)
                        flow = net.RemainingCapacity(s, d);
                });

                Traverse(parents, target, (s, d) =>
                {
                    net.Flow(s, d, net.Flow(s, d) + flow);
                    net.Flow(d, s, net.Flow(d, s) - flow);
                });

                maxFlow += flow;

                parents = FindPath(net, source, target);
            }

            return maxFlow;
        }

        public static int MaxFlow<T>(FlowNet<T> net, T source, T target)
        {
            return MaxFlow(net, net.VertexToIndex[source], net.VertexToIndex[target]);
        }

        private static int[] FindPath<T>(FlowNet<T> net, int source, int target)
        {
            var parents = InitParent(net.Vertices);
            var frontier = new Queue<int>();

            parents[source] = source;
            frontier.Enqueue(source);

            while (frontier.Any())
            {
                var current = frontier.Dequeue();

                for (int v = 0; v < net.Vertices; v++)
                {
                    if (parents[v] != -1 || net.IsSaturated(current, v))
                        continue;

                    parents[v] = current;
                    frontier.Enqueue(v);

                    if (v == target)
                        return parents;
                }
            }

            return parents;
        }

        private static void Traverse(int[] parents, int start, Action<int, int> action)
        {
            int current = start;
            while (parents[current] != current)
            {
                int source = parents[current];
                int destination = current;

                action(source, destination);

                current = parents[current];
            }
        }

        private static int[] InitParent(int vertices)
        {
            int[] parent = new int[vertices];

            for (int i = 0; i < vertices; i++)
            {
                parent[i] = -1;
            }

            return parent;
        }
    }
}