namespace Graphs
{
    public class Edge<T>
    {
        public const int DefaultWeight = 1;

        private int weight;
        private bool directed;

        public Edge(Vertex<T> source, Vertex<T> destination, int weight = DefaultWeight, bool directed = true)
        {
            this.Source = source;
            this.Destination = destination;
            this.Directed = directed;
            this.Weight = weight;
        }

        public Vertex<T> Source { get; }

        public Vertex<T> Destination { get; }       

        public int Weight 
        {
            get
            {
                return this.weight;
            }

            set
            {
                this.weight = value;

                if (!this.directed)
                    this.Inverse.weight = this.weight;
            }
        }

        public bool Directed
        {
            get
            {
                return this.directed;
            }

            private set
            {
                this.directed = value;

                if (this.directed)
                    return;

                this.Inverse = new Edge<T>(this.Destination, this.Source, this.Weight);

                this.Inverse.directed = false;
                this.Inverse.Inverse = this;
            }
        }

        public Edge<T> Inverse { get; private set; }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Edge<T>);
        }

        public override int GetHashCode()
        {
            if (!this.Directed)
            {
                return base.GetHashCode() ^ this.Inverse.GetBaseHashCode();
            }

            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"{this.Source.Key} ({this.Weight})> {this.Destination.Key}";
        }

        private bool Equals(Edge<T> other)
        {
            return other != null &&
                (other == this || other == this.Inverse);
        }

        public int GetBaseHashCode()
        {
            return base.GetHashCode();
        }
    }
}