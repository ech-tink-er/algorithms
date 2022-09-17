namespace Graphs
{
    using System.Collections.Generic;

    static class Forest
    {
        public static Vertex<T> FindRoot<T>(Dictionary<Vertex<T>, Edge<T>> forest, Vertex<T> vertex)
        {
            var current = vertex;
            while (forest[current].Destination != current)
                current = forest[current].Destination;

            return current;
        }

        public static void Join<T>(Dictionary<Vertex<T>, Edge<T>> forest, Edge<T> edge)
        {
            Edge<T> current = forest[edge.Source];
            forest[edge.Source] = edge;

            while (current.Source != current.Destination)
            {
                Edge<T> next = forest[current.Destination];

                forest[current.Destination] = current.Inverse;

                current = next;
            }
        }
    }
}