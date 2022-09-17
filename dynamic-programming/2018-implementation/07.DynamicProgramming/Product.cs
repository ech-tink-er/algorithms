namespace DynamicProgramming
{
    class Product
    {
        public Product(string name, int weight, int value)
        {
            this.Name = name;
            this.Weight = weight;
            this.Value = value;
        }

        public string Name { get; }

        public int Weight { get; }

        public int Value { get; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}