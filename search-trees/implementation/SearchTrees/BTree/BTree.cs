namespace SearchTrees
{
    using System;
    using static System.Math;
    using System.Linq;
    using System.Collections.Generic;

    using static Searching;
    using static Iterators;

    public sealed class BTree<T> : SearchTree<T>
        where T : IComparable<T>
    {
        public const int MinOrder = 3;

        // Location points to the key index that should equal value.
        private static BNode<T> Find(BNode<T> root, T value, out int location)
        {
            location = -1;
            BNode<T> current = null;
            BNode<T> next = root;
            while (next != null)
            {
                current = next;

                location = BinSearch(current.Keys, value);

                if (current.KeyAtEquals(location, value))
                {
                    return current;
                }

                next = current.Children.Any() ? current.Children[location] : null;
            }

            return current;
        }

        private static BNode<T> Min(BNode<T> root)
        {
            BNode<T> current = root;
            while (current.Children.Any())
            {
                current = current.Children.First();
            }

            return current;
        }

        private static BNode<T> Max(BNode<T> root)
        {
            BNode<T> current = root;
            while (current.Children.Any())
            {
                current = current.Children.Last();
            }

            return current;
        }

        private static void Rotate(BNode<T> node, int location, int direction)
        {
            if (node == null || location < 0 || node.Children.Count < location + 1 )
            {
                return;
            }

            direction = direction / Abs(direction);

            int fromDiff = (1 - direction) / 2;
            int toDiff = (1 + direction) / 2;

            var from = node.Children[location + fromDiff];
            var to = node.Children[location + toDiff];

            if (!from.Keys.Any())
            {
                return;
            }

            var fromEnd = -toDiff;
            var toEnd = -fromDiff;

            to.InsertKey(toEnd, node.PopKey(location));
            node.InsertKey(location, from.PopKey(fromEnd));

            if (from.Children.Any())
            {
                to.InsertChild(toEnd, from.PopChild(fromEnd));
            }

            return;
        }

        internal BNode<T> root;

        public BTree(int order = 3)
        {
            this.Order = order;
            this.root = this.NewNode();
        }

        public BTree(IEnumerable<T> values, int order = 3)
            : this(order)
        {
            this.Add(values);
        }

        public int Order { get; }

        public override void Add(T value)
        {
            var node = Find(this.root, value, out int location);

            if (node.KeyAtEquals(location, value))
            {
                return;
            }

            this.Count++;
            
            node.InsertKey(location, value);
            this.InsetionUpkeep(node);
        }

        public override void Add(IEnumerable<T> values)
        {
            foreach (var value in values)
            {
                this.Add(value);
            }
        }

        public override bool Remove(T value)
        {
            var node = Find(this.root, value, out int location);

            if (!node.KeyAtEquals(location, value))
            {
                return false;
            }

            this.Remove(node, location);
            this.Count--;

            return true;
        }

        public override void Clear()
        {
            this.root = this.NewNode();
            this.Count = 0;
        }

        public override bool Contains(T value)
        {
            var node = Find(this.root, value, out int location);

            return node.KeyAtEquals(location, value);
        }

        public override T Min()
        {
            var min = Min(this.root);
            if (!min.Keys.Any())
            {
                throw new InvalidOperationException("Tree is empty!");
            }

            return min.Keys.First();
        }

        public override T Max()
        {
            var max = Max(this.root);
            if (!max.Keys.Any())
            {
                throw new InvalidOperationException("Tree is empty!");
            }

            return max.Keys.Last();
        }

        public override IEnumerator<T> GetEnumerator()
        {
            var reads = new Dictionary<BNode<T>, int>();
            var stack = new Stack<BNode<T>>();

            var current = this.root;
            reads[this.root] = 0;

            while (stack.Any() || current != null)
            {
                while (current != null)
                {
                    foreach (var child in current.Children)
                    {
                        if (!reads.ContainsKey(child))
                        {
                            reads[child] = 0;
                        }
                    }

                    stack.Push(current);
                    current = current.Children.FirstOrDefault();
                }

                current = stack.Pop();

                for (int i = reads[current]; i < current.Keys.Count; i++)
                {
                    yield return current.Keys[i];
                }

                var parent = current.Parent;
                if (parent == null || reads[parent] == parent.Keys.Count)
                {
                    current = null;
                    continue;
                }

                yield return parent.Keys[reads[parent]++];

                current = parent.Children[reads[parent]];
            }
        }

        public override string ToString(TreeStringFormat format, bool verbose = false)
        {
            switch (format)
            {
                case TreeStringFormat.List:
                    return string.Join(", ", this);
                case TreeStringFormat.Vertical:
                    return Composition.VerticalCompose(this);
                case TreeStringFormat.Horizontal:
                    return Composition.HorizontalCompose(this, verbose);
                default: throw new NotImplementedException($"{format} case not handled!");
            }
        }

        internal BNode<T>[][] ToTreeArray()
        {
            var tree = new List<List<BNode<T>>>();
            tree.Add(new List<BNode<T>>());

            var levels = new Dictionary<BNode<T>, int>();
            levels[this.root] = 0;

            foreach (var node in LevelOrder(this.root))
            {
                int level = levels[node];
                tree[level].Add(node);

                if (!node.Children.Any())
                {
                    continue;
                }

                if (level == tree.Count - 1)
                {
                    tree.Add(new List<BNode<T>>());
                }

                foreach (var child in node.Children)
                {
                    levels[child] = level + 1;
                }
            }

            return tree.Select(l => l.ToArray()).ToArray();
        }

        private BNode<T> NewNode(params T[] values)
        {
            var node = new BNode<T>(this.Order, values);

            return node;
        }

        private void Remove(BNode<T> node, int location)
        {
            if (node.Children.Any())
            {
                var predecessor = Max(node.Children[location]);
                int last = predecessor.Keys.Count - 1;

                node.SetKey(location, predecessor.Keys[last]);

                this.Remove(predecessor, last);
            }
            else
            {
                T key = node.PopKey(location);

                this.RemovalUpkeep(node, key);
            }
        }

        private void InsetionUpkeep(BNode<T> node)
        {
            while (this.Split(node))
            {
                node = node.Parent;
            }
        }

        private void RemovalUpkeep(BNode<T> node, T key)
        {
            var next = node.Parent;
            while (!this.CorrectRemoval(node, key))
            {
                node = next;
                next = node.Parent;
            }
        }

        private bool CorrectRemoval(BNode<T> node, T key)
        {
            if (node.MinKeys <= node.Keys.Count || node.Parent == null)
            {
                return true;
            }

            var parent = node.Parent;
            int location = BinSearch(parent.Keys, key);

            int left = location - 1;
            int right = location + 1;
            if (0 <= left && node.MinKeys < parent.Children[left].Keys.Count)
            {
                Rotate(node.Parent, left, 1);
                return true;
            }
            else if (right < node.Parent.Children.Count && node.MinKeys < node.Parent.Children[right].Keys.Count)
            {
                Rotate(node.Parent, right - 1, -1);
                return true;
            }

            this.Merge(node, location);
            return false;
        }

        private bool Split(BNode<T> node)
        {
            if (node.Keys.Count <= node.MaxKeys)
            {
                return false;
            }

            var left = this.NewNode();
            var right = node;

            int half = right.Keys.Count / 2;

            left.AppendKeys(right.PopKeysHead(half));
            T median = right.PopKeyHead();

            if (right.Children.Any())
            {
                left.AppendChildren(right.PopChildren(count: half + 1));
            }

            if (right.Parent == null)
            {
                this.root = this.NewNode();
                this.root.AppendChild(right);
            }

            var parent = right.Parent;

            int location = BinSearch(parent.Keys, median);

            parent.InsertKey(location, median);
            parent.InsertChild(location, left);

            return true;
        }

        private void Merge(BNode<T> node, int location)
        {
            int leftLocation = location == 0 ? location : location - 1;
            int rightLocation = location == 0 ? location + 1 : location;
        
            var parent = node.Parent;
            var left = parent.Children[leftLocation];
            var right = parent.PopChild(rightLocation);

            left.AppendKey(parent.PopKey(leftLocation));

            left.AppendKeys(right.Keys);
            left.AppendChildren(right.Children);

            if (parent == this.root && !parent.Keys.Any())
            {
                parent.PopChild(leftLocation);
                this.root = left;
            }
        }
    }
}