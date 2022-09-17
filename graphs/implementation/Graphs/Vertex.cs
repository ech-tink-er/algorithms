namespace Graphs
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using Utilities;

    public class Vertex<T>
    {
        private LinkedList<Edge<T>> edges;

        public Vertex(T key)
        {
            this.Key = key;
            this.edges = new LinkedList<Edge<T>>();
        }

        public T Key { get; }

        public IEnumerable<Edge<T>> Edges 
        { 
            get
            {
                return this.edges;
            }
        }

        public Edge<T> AddEdge(Vertex<T> destination, int weight = Edge<T>.DefaultWeight, bool directed = true)
        {
            var edge = new Edge<T>(this, destination, weight, directed);
            this.edges.AddLast(edge);

            if (!directed)
            {
                destination.edges.AddLast(edge.Inverse);
            }

            return edge;
        }

        public bool RemoveEdge(Edge<T> edge)
        {
            if (edge.Source != this)
                throw new ArgumentException("Edge isn't outgoing from this vertex.");

            return this.edges.Remove(edge) &&
                (edge.Directed || edge.Destination.edges.Remove(edge));
        }

        public bool RemoveEdge(Vertex<T> destination)
        {
            var node = this.edges.Find(edge => edge.Destination == destination);
            if (node == null)
                return false;

            this.edges.Remove(node);

            return node.Value.Directed || destination.edges.Remove(node.Value.Inverse);
        }

        public bool RemoveAllEdges(Vertex<T> destination)
        {
            var nodes = this.edges.FindAll(edge => edge.Destination == destination);
            if (!nodes.Any())
                return false;

            foreach (var node in nodes)
                this.edges.Remove(node);

            return nodes.Where(node => !node.Value.Directed)
                .All(node => destination.edges.Remove(node.Value.Inverse));
        }

        public override string ToString()
        {
            var destinations = this.edges.Select(edge => edge.Destination.Key.ToString());

            return $"{this.Key} > {destinations.Join(" ")}";
        }
    }
}