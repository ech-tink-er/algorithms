namespace Graphs
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using BDS.Heap;

    using static Traversals;
    using static Forest;

    public class Graph<T>
    {
        private Dictionary<T, Vertex<T>> vertices;

        public Graph()
        {
            this.vertices = new Dictionary<T, Vertex<T>>();
        }

        public IReadOnlyCollection<Vertex<T>> Vertices
        {
            get
            {
                return this.vertices.Values;
            }
        } 

        public IEnumerable<Edge<T>> Edges
        {
            get
            {
                foreach (var vertex in this.vertices.Values)
                {
                    foreach (var edge in vertex.Edges)
                    {
                        yield return edge;
                    }
                }
            }
        }

        public Vertex<T> this[T key]
        {
            get
            {
                if (!this.vertices.ContainsKey(key))
                    throw new ArgumentException($"Missing vertex - {key}!");

                return this.vertices[key];
            }

            set
            {
                this.vertices[key] = value;
            }
        }

        public void AddVertex(T key)
        {
            this[key] = new Vertex<T>(key);
        }

        public bool RemoveVertex(T key)
        {
            if (!this.vertices.ContainsKey(key))
            {
                return false;
            }

            var vertex = this[key];
            this.vertices.Remove(key);

            var incoming = this.Edges.Where(edge => edge.Destination == vertex)
                .ToArray();

            foreach (var edge in incoming)
            {
                edge.Source.RemoveEdge(edge);
            }

            return true;
        }

        public void AddEdge(T source, T destination, int weight = Edge<T>.DefaultWeight, bool directed = true)
        {
            if (!this.vertices.ContainsKey(source))
                this.AddVertex(source);

            if (!this.vertices.ContainsKey(destination))
                this.AddVertex(destination);

            this[source].AddEdge(this[destination], weight, directed);
        }

        public bool RemoveEdge(Edge<T> edge)
        {
            return edge.Source.RemoveEdge(edge);
        }

        public bool HasVertex(T key)
        {
            return this.vertices.ContainsKey(key);
        }

        public bool HasVertex(Vertex<T> vertex)
        {
            return this.vertices.ContainsKey(vertex.Key) && 
                this.vertices[vertex.Key] == vertex;
        }

        public void Clear()
        {
            this.vertices.Clear();
        }

        public Graph<T> Copy()
        {
            var copy = new Graph<T>();

            foreach (var vertex in this.Vertices)
                copy.AddVertex(vertex.Key);

            foreach (var edge in this.Edges)
                copy.AddEdge(edge.Source.Key, edge.Destination.Key, edge.Weight);

            return copy;
        }

        public Vertex<T>[] TopoSort()
        {
            var sorted = new List<Vertex<T>>();

            DepthTraverse(this, (vertex, visited) =>
            {
                foreach (var edge in vertex.Edges)
                {
                    if (visited.ContainsKey(edge.Destination) && !visited[edge.Destination])
                    {
                        // Destination is currently being visited, cycle detected.
                        throw new CycleException("Can't topologically sort cyclic graph!");
                    } 
                }

                sorted.Add(vertex);

                return false;
            });

            sorted.Reverse();

            return sorted.ToArray();
        }

        public Paths<T> ShortestPaths(
            T key,
            bool edgewise = false,
            bool dag = false,
            bool safe = false
        )
        {
            return this.ShortestPaths(this[key], null, edgewise, dag, safe);
        }

        public Paths<T> ShortestPaths(
            T key,
            T targetKey,
            bool edgewise = false,
            bool dag = false,
            bool safe = false
        )
        {
            return this.ShortestPaths(this[key], this[targetKey], edgewise, dag, safe);
        }

        public Paths<T> ShortestPaths(
            Vertex<T> source,
            Vertex<T> target = null,
            bool edgewise = false,
            bool dag = false,
            bool safe = false
        )
        {
            if (!this.HasVertex(source))
                throw new InvalidOperationException("Source not in graph!");
            else if (edgewise)
                return this.EdgewiseShortestPaths(source, target);
            else if (dag)
                return this.DAGShortestPaths(source, target);
            else if (safe)
                return this.SafeShortestPaths(source);
            else
                return this.ShortestPaths(source, target);
        }

        // Prim's Algorithm
        public Dictionary<Vertex<T>, Edge<T>> MinSpanningTree(Vertex<T> root)
        {
            var tree = new Dictionary<Vertex<T>, Edge<T>>();
            tree[root] = new Edge<T>(root, root, 0);

            var frontier = new Heap<int, Edge<T>>(e => e.Weight, min: true, values: root.Edges);
            while (frontier.Any())
            {
                var next = frontier.Pop();

                if (next.Directed)
                    throw new InvalidOperationException("Can't find MST on directed graphs.");
                else if (tree.ContainsKey(next.Destination))
                    continue;

                tree[next.Destination] = next.Inverse;

                foreach (var edge in next.Destination.Edges)
                    frontier.Push(edge);
            }

            return tree;
        }

        // Kruskal's Algorithm
        public Dictionary<Vertex<T>, Edge<T>> MinSpanningForest()
        {
            var forest = new Dictionary<Vertex<T>, Edge<T>>();
            foreach (var vertex in this.Vertices)
                forest[vertex] = new Edge<T>(vertex, vertex, 0);

            var edges = new Heap<int, Edge<T>>(e => e.Weight, min: true, values: this.Edges);

            while (edges.Any())
            {
                var edge = edges.Pop();

                if (edge.Directed)
                    throw new InvalidOperationException("Can't find MSF on directed graphs.");

                if (FindRoot(forest, edge.Source) != FindRoot(forest, edge.Destination))
                    Join(forest, edge);
            }

            return forest;
        }

        // Dijkstra's Algorithm
        private Paths<T> ShortestPaths(Vertex<T> source, Vertex<T> target = null)
        {
            var paths = new Paths<T>(this, source);
            var visited = new HashSet<Vertex<T>>();
            var frontier = new Heap<int, Vertex<T>>(vertex => paths.Lengths[vertex], min: true);

            frontier.Push(source);
            while (frontier.Any())
            {
                var current = frontier.Pop();

                if (target != null && current.Equals(target))
                {
                    // Target found.
                    return paths;
                }
                else if (visited.Contains(current))
                    continue;
                else
                    visited.Add(current);

                foreach (var edge in current.Edges)
                {
                    if (edge.Weight < 0)
                        throw new InvalidOperationException("Negative edge weights not allowed.");

                    if (!visited.Contains(edge.Destination) && paths.Relax(edge))
                        frontier.Push(edge.Destination);
                }
            }

            return paths;
        }

        // Breadth-First Search
        private Paths<T> EdgewiseShortestPaths(Vertex<T> source, Vertex<T> target = null)
        {
            var paths = new Paths<T>(this, source);

            BreadthTraverse(source, (vertex, level, src) =>
            {
                paths.Lengths[vertex] = level;
                paths.Previous[vertex] = src;

                // Terminate traverse, if target found.
                return target != null && vertex == target;
            });

            return paths;
        }

        // DAG Algorithm
        private Paths<T> DAGShortestPaths(Vertex<T> source, Vertex<T> target = null)
        {
            var paths = new Paths<T>(this, source);

            var ordered = this.TopoSort();

            bool left = true;
            foreach (var vertex in ordered)
            {
                if (target != null && vertex == target)
                    return paths;

                if (vertex == source)
                    left = false;
                else if (left)
                    continue;

                foreach (var edge in vertex.Edges)
                    paths.Relax(edge);
            }

            return paths;
        }

        // Bellman-Ford Algorithm
        private Paths<T> SafeShortestPaths(Vertex<T> source)
        {
            var paths = new Paths<T>(this, source);

            for (int i = 0; i < this.vertices.Count - 1; i++)
            {
                bool relaxed = false;

                foreach (var edge in this.Edges)
                    relaxed |= paths.Relax(edge);

                if (!relaxed)
                    break;
            }

            foreach (var edge in this.Edges)
            {
                if (paths.CanRelax(edge))
                    throw new CycleException("Graph contains negative weight cycles!");
            }

            return paths;
        }
    }
}