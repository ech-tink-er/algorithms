namespace SearchTrees
{
    using System;
    using System.Collections.Generic;

    // BinNode.Data is used to store a subree's level.
    public class AATree<T> : BinTree<T>
        where T : IComparable<T>
    {
        private static bool DecreaseLevel(BinNode<T> node)
        {
            int leftLevel = node.Left == null ? 0 : node.Left.Data;
            int rightLevel = node.Right == null ? 0 : node.Right.Data;

            int properLevel = Math.Min(leftLevel, rightLevel) + 1;

            if (node.Data == rightLevel)
            {
                node.Right.Data = properLevel;
            }

            bool decreased = node.Data != properLevel;

            node.Data = properLevel;

            return decreased;
        }

        private static BinNode<T> Skew(BinNode<T> node, out bool triggered)
        {
            triggered = false;
            if (node.Left == null || node.Left.Data != node.Data)
            {
                return node;
            }

            triggered = true;
            return Rotate(node, 1);
        }

        private static BinNode<T> Split(BinNode<T> node, out bool triggered)
        {
            triggered = false;
            if (node.Right == null || node.Right.Right == null || node.Data != node.Right.Right.Data)
            {
                return node;
            }

            var root = Rotate(node, -1);
            root.Data++;

            triggered = true;
            return root;
        }

        public AATree()
            : base()
        { }

        public AATree(IEnumerable<T> values)
            : base(values)
        { }

        protected override void AdditionUpkeep(BinNode<T> @new)
        {
            var current = @new.Parent;

            int noIncreaseCount = 0;
            while (current != null)
            {
                var next = Skew(current, out bool triggered);
                next = Split(next, out triggered);

                noIncreaseCount = triggered ? 0 : noIncreaseCount + 1;

                if (noIncreaseCount == 2)
                {
                    break;
                }

                current = next.Parent;
            }
        }

        protected override void RemovalUpkeep(BinNode<T> @new, BinNode<T> parent, int location, BinNode<T> old)
        {
            var current = parent;
            while (current != null)
            {
                if (!DecreaseLevel(current))
                {
                    break;
                }

                var next = Skew(current, out bool triggered);
                Skew(next.Right, out triggered);
                if (next.Right != null && next.Right.Right != null)
                {
                    Skew(next.Right.Right, out triggered);
                }

                next = Split(next, out triggered);
                Split(next.Right, out triggered);

                current = next.Parent;
            }
        }

        protected override BinNode<T> NewNode(T value)
        {
            return new BinNode<T>(value, 1);
        }

        // Since AA trees can only be right heavy, always pick right.
        protected override bool RemoveFromLeft(BinNode<T> node)
        {
            return false;
        }
    }
}