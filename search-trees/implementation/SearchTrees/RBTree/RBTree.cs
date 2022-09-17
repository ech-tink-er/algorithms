namespace SearchTrees
{
    using System;
    using System.Collections.Generic;

    // BinNode marks the node red (0), or black (1).
    public sealed partial class RBTree<T> : BinTree<T>
        where T : IComparable<T>
    {
        internal static bool IsBlack(BinNode<T> node)
        {
            return node == null || node.Data == 1;
        }

        internal static bool IsRed(BinNode<T> node)
        {
            return !IsBlack(node);
        }

        internal static void SetColor(BinNode<T> node, bool black)
        {
            if (node == null)
            {
                return;
            }

            node.Data = black ? 1 : 0;
        }

        internal static void ColorRed(BinNode<T> node)
        {
            SetColor(node, black: false);
        }

        internal static void ColorBlack(BinNode<T> node)
        {
            SetColor(node, black: true);
        }

        internal static int BlackHeight(BinNode<T> node)
        {
            if (node == null)
            {
                return 1;
            }

            return node.Data + Math.Max(BlackHeight(node.Left), BlackHeight(node.Right));
        }

        public RBTree()
            : base()
        { }

        public RBTree(IEnumerable<T> values)
            : base(values)
        { }

        protected override void AdditionUpkeep(BinNode<T> @new)
        {
            var current = @new;
            while (!AdditionCorrections.Correct(current))
            {
                current = AdditionCorrections.MoveCorrection(current);
            }
        }

        protected override void RemovalUpkeep(BinNode<T> next, BinNode<T> parent, int location, BinNode<T> old)
        {
            while (!RemovalCorrections.Correct(next, parent, location, old))
            {
                next = RemovalCorrections.MoveCorrection(next, parent, location);
                if (next != null)
                {
                    parent = next.Parent;
                    location = next.Location;
                }
            }
        }

        internal override string FormatNode(BinNode<T> node, bool verbose = false)
        {
            string result = "";

            if (node != null)
            {
                result = node.ToString(verbose, d => d == 0 ? "R" : "B");
            }

            return result;
        }
    }
}