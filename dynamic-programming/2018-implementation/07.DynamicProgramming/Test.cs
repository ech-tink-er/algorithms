namespace DynamicProgramming
{
    using System;

    class Test
    {
        public Test(string name, Func<object> program)
        {
            this.Name = name;
            this.Program = program;
        }

        public string Name { get; }

        public Func<object> Program { get; }
    }
}