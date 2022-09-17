namespace Graphs
{
    using System.Collections.Generic;

    public class Paths<T>
    {
        public const int Unreachable = int.MaxValue - 100000;

        public Paths(Graph<T> graph, Vertex<T> source)
        {
            this.Lengths = new Dictionary<Vertex<T>, int>();
            this.Previous = new Dictionary<Vertex<T>, Vertex<T>>();

            this.Source = source;

            foreach (var vertex in graph.Vertices)
            {
                this.Lengths[vertex] = Unreachable;
            }

            this.Lengths[source] = 0;
            this.Previous[source] = null;
        }

        public Vertex<T> Source { get; }
        public Dictionary<Vertex<T>, int> Lengths { get; }
        public Dictionary<Vertex<T>, Vertex<T>> Previous { get; }

        public bool Relax(Edge<T> edge)
        {
            if (this.CanRelax(edge))
            {
                this.Lengths[edge.Destination] = this.Lengths[edge.Source] + edge.Weight;
                this.Previous[edge.Destination] = edge.Source;

                return true;
            }

            return false;
        }

        public bool CanRelax(Edge<T> edge)
        {
            return this.Lengths[edge.Source] + edge.Weight < this.Lengths[edge.Destination];
        }
    }
}