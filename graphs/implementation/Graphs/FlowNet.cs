namespace Graphs
{
    using System.Linq;
    using System.Collections.Generic;

    public class FlowNet<T>
    {
        private int[,] flow;
        private int[,] capacities;
        private Dictionary<T, int> vertexToIndex;
        private Dictionary<int, T> indexToVertex;

        public FlowNet(T[] sources, T[] destinations, int[] capacities)
        {
            this.vertexToIndex = new Dictionary<T, int>();
            this.indexToVertex = new Dictionary<int, T>();

            T[] vertices = sources.Concat(destinations)
                .Distinct()
                .ToArray();

            for (int i = 0; i < vertices.Length; i++)
            {
                this.vertexToIndex[vertices[i]] = i;
                this.indexToVertex[i] = vertices[i];
            }

            this.flow = new int[vertices.Length, vertices.Length];
            this.capacities = new int[vertices.Length, vertices.Length];

            for (int i = 0; i < sources.Length; i++)
            {
                this.Capacity(sources[i], destinations[i], capacities[i]);
            }
        }

        public IReadOnlyDictionary<T, int> VertexToIndex
        {
            get
            {
                return this.vertexToIndex;
            }
        }

        public IReadOnlyDictionary<int, T> IndexToVertex
        {
            get
            {
                return this.indexToVertex;
            }
        }

        public int Vertices
        {
            get
            {
                return this.flow.GetLength(0);
            }
        }

        public int Flow(int source, int destination)
        {
            return this.flow[source, destination];
        }

        public void Flow(int source, int destination, int flow)
        {
            this.flow[source, destination] = flow;
        }

        public int Capacity(int source, int destination)
        {
            return this.capacities[source, destination];
        }

        private void Capacity(int source, int destination, int capacity)
        {
            this.capacities[source, destination] = capacity;
        }

        public bool IsSaturated(int source, int destination)
        {
            return this.Flow(source, destination) >= this.Capacity(source, destination);
        }

        public int RemainingCapacity(int source, int destination)
        {
            return this.Capacity(source, destination) - this.Flow(source, destination);
        }

        public int Flow(T source, T destination)
        {
            return this.flow[this.VertexToIndex[source], this.VertexToIndex[destination]];
        }

        public void Flow(T source, T destination, int flow)
        {
            this.flow[this.VertexToIndex[source], this.VertexToIndex[destination]] = flow;
        }

        public int Capacity(T source, T destination)
        {
            return this.capacities[this.VertexToIndex[source], this.VertexToIndex[destination]];
        }

        private void Capacity(T source, T destination, int capacity)
        {
            this.capacities[this.VertexToIndex[source], this.VertexToIndex[destination]] = capacity;
        }

        public bool IsSaturated(T source, T destination)
        {
            return this.IsSaturated(this.VertexToIndex[source], this.VertexToIndex[destination]);
        }

        public int RemainingCapacity(T source, T destination)
        {
            return this.RemainingCapacity(this.VertexToIndex[source], this.VertexToIndex[destination]);
        }

        public void Clear()
        {
            for (int s = 0; s < this.Vertices; s++)
                for (int d = 0; d < this.Vertices; d++)
                    this.flow[s, d] = 0;
        }
    }
}