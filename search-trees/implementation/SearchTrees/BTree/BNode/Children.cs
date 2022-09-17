namespace SearchTrees
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using static Utilities.UMath;

    internal partial class BNode<T>
        where T : IComparable<T>
    {
        private List<BNode<T>> children;

        public int MinChildren { get; }

        public int MaxChildren { get; }

        public IReadOnlyList<BNode<T>> Children
        {
            get
            {
                return this.children;
            }
        }

        public void InsertChild(int location, BNode<T> node)
        {
            location = LoopNumber(location, this.children.Count + 1);

            this.ValidateChildrenInsertion(location, node);

            this.children.Insert(location, node);
            node.Parent = this;
        }

        public void InsertChildren(int location, IEnumerable<BNode<T>> nodes)
        {
            location = LoopNumber(location, this.children.Count + 1);

            this.ValidateChildrenInsertion(location, nodes.ToArray());

            this.children.InsertRange(location, nodes);

            foreach (var node in nodes)
            {
                node.Parent = this;
            }
        }

        public void PrependChild(BNode<T> node)
        {
            this.InsertChild(0, node);
        }

        public void PrependChildren(IEnumerable<BNode<T>> nodes)
        {
            this.InsertChildren(0, nodes);
        }

        public void AppendChild(BNode<T> node)
        {
            this.InsertChild(this.children.Count, node);
        }

        public void AppendChildren(IEnumerable<BNode<T>> nodes)
        {
            this.InsertChildren(this.children.Count, nodes);
        }

        public BNode<T> PopChild(int location)
        {
            location = LoopNumber(location, this.children.Count);

            var child = this.children[location];

            this.children.RemoveAt(location);
            child.Parent = null;

            return child;
        }

        public BNode<T>[] PopChildren(int from = 0, int count = -1)
        {
            from = LoopNumber(from, this.children.Count);
            count = LoopNumber(count, 1, this.children.Count + 1);

            var children = this.children.Skip(from)
                .Take(count)
                .ToArray();

            this.children.RemoveRange(from, count);

            foreach (var child in children)
            {
                child.Parent = null;
            }

            return children;
        }

        public BNode<T> PopChildHead()
        {
            return this.PopChild(0);
        }

        public BNode<T>[] PopChildrenHead(int count)
        {
            return this.PopChildren(0, count);
        }

        public BNode<T> PopChildTail()
        {
            return this.PopChild(this.children.Count - 1);
        }

        public BNode<T>[] PopChildrenTail(int count)
        {
            return this.PopChildren(this.children.Count - count, count);
        }

        private void ValidateChildrenInsertion(int location, params BNode<T>[] nodes)
        {
            if (nodes.Any(child => child == null))
            {
                throw new ArgumentNullException("Can't insert a null child!");
            }
            else if (this.children.Count + nodes.Length > this.children.Capacity)
            {
                throw new InvalidOperationException("Children are at maximum capacity!");
            }
        }
    }
}