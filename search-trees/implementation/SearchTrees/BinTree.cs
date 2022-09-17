namespace SearchTrees
{
    using System;
    using static System.Math;
    using static System.Linq.Enumerable;
    using System.Collections.Generic;

    using static Iterators;

    using static Utilities.UMath;

    public partial class BinTree<T> : SearchTree<T>
        where T : IComparable<T>
    {
        internal static int GetHeight(BinNode<T> node)
        {
            if (node == null)
            {
                return -1;
            }

            return Math.Max(GetHeight(node.Left), GetHeight(node.Right)) + 1;
        }

        protected static BinNode<T> Min(BinNode<T> root)
        {
            if (root == null)
            {
                return null;
            }

            var min = root;

            while (min.Left != null)
            {
                min = min.Left;
            }

            return min;
        }

        protected static BinNode<T> Max(BinNode<T> root)
        {
            if (root == null)
            {
                return null;
            }

            var max = root;

            while (max.Right != null)
            {
                max = max.Right;
            }

            return max;
        }

        // Location 0 points to the returned node, +-1 points to one of it's children.
        protected static BinNode<T> Find(BinNode<T> root, T value, out int location)
        {
            location = 0;
            if (root == null)
            {
                return null;
            }

            BinNode<T> current = null;
            BinNode<T> next = root;
            do
            {
                current = next;

                location = value.CompareTo(current.Value);

                if (location == 0)
                {
                    return current;
                }
                else if (location < 0)
                {
                    next = current.Left;
                }
                else
                {
                    next = current.Right;
                }
            } while (next != null);

            return current;
        }

        protected static BinNode<T> Rotate(BinNode<T> node, int direction)
        {
            if (direction == 0)
            {
                return node;
            }

            direction = direction / Abs(direction);

            var replacement = node?.GetChild(-direction);

            if (node == null || replacement == null)
            {
                return node;
            }

            node.Parent?.SetChild(node.Location, replacement);

            var move = replacement.GetChild(direction);
            move?.Disconnect();

            replacement.SetChild(direction, node);
            node.SetChild(-direction, move);

            return replacement;
        }

        protected static BinNode<T> DoubleRotate(BinNode<T> node, int direction)
        {
            Rotate(node?.GetChild(-direction), -direction);

            return Rotate(node, direction);
        }

        internal BinNode<T> root;

        public BinTree()
        {
            this.Count = 0;
        }

        public BinTree(IEnumerable<T> values)
            : this()
        {
            this.Add(values);
        }

        public override void Add(T value)
        {
            BinNode<T> node = Find(this.root, value, out int location);

            if (node != null && location == 0)
            {
                return;
            }

            BinNode<T> @new = this.NewNode(value);

            if (node == null)
            {
                this.root = @new;
            }
            else
            {
                node.SetChild(location, @new);
            }

            this.Count++;

            this.AdditionUpkeep(@new);
            this.SetRoot();
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
            BinNode<T> node = Find(this.root, value, out int location);

            if (node == null || location != 0)
            {
                return false;
            }

            this.Remove(node);

            this.Count--;

            return true;
        }

        public override void Clear()
        {
            this.root = null;
            this.Count = 0;
        }

        public override bool Contains(T value)
        {
            return Find(this.root, value, out int location) != null &&
                location == 0;
        }

        public override T Min()
        {
            var min = Min(this.root);

            if (min == null)
            {
                throw new InvalidOperationException("Tree is empty!");
            }

            return min.Value;
        }

        public override T Max()
        {
            var max = Max(this.root);

            if (max == null)
            {
                throw new InvalidOperationException("Tree is empty!");
            }

            return max.Value;
        }

        public override IEnumerator<T> GetEnumerator()
        {
            foreach (var node in InOrder(this.root))
            {
                yield return node.Value;
            }
        }

        public override string ToString(TreeStringFormat format, bool verbose = false)
        {
            switch (format)
            {
                case TreeStringFormat.List:
                    return Composition.ListCompose(this, verbose);
                case TreeStringFormat.Vertical:
                    return Composition.VerticalCompose(this, verbose);
                case TreeStringFormat.Horizontal:
                    return Composition.HorizontalCompose(this, verbose);
                default: throw new NotImplementedException($"{format} case not handled!");
            }
        }

        internal BinNode<T>[][] ToTreeArray()
        {
            var tree = Range(0, GetHeight(this.root) + 1)
                .Select(level => new BinNode<T>[PowOf2(level)])
                .ToArray();

            var rows = new Dictionary<BinNode<T>, int>();
            var cols = new Dictionary<BinNode<T>, int>();
            rows[this.root] = 0;
            cols[this.root] = 0;

            foreach (var node in LevelOrder(this.root))
            {
                int r = rows[node];
                int c = cols[node];
                tree[r][c] = node;

                if (node.Left != null)
                {
                    rows[node.Left] = r + 1;
                    cols[node.Left] = c * 2;
                }

                if (node.Right != null)
                {
                    rows[node.Right] = r + 1;
                    cols[node.Right] = c * 2 + 1;
                }
            }

            return tree;
        }

        internal virtual string FormatNode(BinNode<T> node, bool verbose = false)
        {
            string result = "";

            if (node != null)
            {
                result = node.ToString(verbose);
            }

            return result;
        }

        protected virtual void AdditionUpkeep(BinNode<T> @new)
        { }

        protected virtual void RemovalUpkeep(BinNode<T> @new, BinNode<T> parent, int location, BinNode<T> old)
        { }

        protected virtual bool RemoveFromLeft(BinNode<T> node)
        {
            return false;
        }

        protected virtual BinNode<T> NewNode(T value)
        {
            return new BinNode<T>(value);
        }

        private BinNode<T> Remove(BinNode<T> node)
        {
            if (node.Left != null && node.Right != null)
            {
                BinNode<T> replacement;

                if (this.RemoveFromLeft(node))
                {
                    // In-order predecessor.
                    replacement = Max(node.Left);
                }
                else
                {
                    // In-order successor.
                    replacement = Min(node.Right);
                }

                node.Value = replacement.Value;

                this.Remove(replacement);

                return node;
            }

            int location = node.Location;
            var parent = node.Parent;
            var next = node.Left == null ? node.Right : node.Left;

            next?.Disconnect();
            parent?.SetChild(node.Location, next);

            this.root = this.root == node ? next : this.root;

            this.RemovalUpkeep(next, parent, location, node);
            this.SetRoot();

            return next;
        }

        private void SetRoot()
        {
            while (this.root?.Parent != null)
            {
                this.root = this.root.Parent;
            }
        }
    }
}