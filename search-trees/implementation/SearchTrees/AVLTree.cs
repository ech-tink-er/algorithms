namespace SearchTrees
{
    using System;
    using System.Collections.Generic;

    /*
       BinNode.Data is used to store a subtree's balance factor.
       A tree's balance factor is the difference between the heights
       of it's right and left subtrees.
    */
    public class AVLTree<T> : BinTree<T> where T : IComparable<T>
    {
        private static void Upkeep(BinNode<T> parent, int childLocation, bool isAddition)
        {
            while (parent != null)
            {
                parent.Data += isAddition ? childLocation : -childLocation;

                var next = Balance(parent);

                if ((isAddition && next.Data == 0) ||
                    (!isAddition && next.Data != 0))
                {
                    break;
                }

                childLocation = next.Location;
                parent = next.Parent;
            }
        }

        private static BinNode<T> Balance(BinNode<T> node)
        {
            int balance = node.Data;
            if (-1 <= balance && balance <= 1)
            {
                return node;
            }

            int direction = -(balance / 2);
            int internalBalance = node.GetChild(-direction).Data;

            if (internalBalance - direction == 0)
            {
                node = DoubleRotate(node, direction);
                DoubleRotateAdjust(node);
            }
            else
            {
                node = Rotate(node, direction);
                RotateAdjust(node, direction);
            }

            return node;
        }

        private static void RotateAdjust(BinNode<T> node, int direction)
        {
            var prev = node.GetChild(direction);

            // When node is balanced (0), it and prev become unbalanced (+-1),
            // and vice versa.
            prev.Data = (node.Data - direction) % 2;
            node.Data = (node.Data + direction) % 2;
        }

        private static void DoubleRotateAdjust(BinNode<T> node)
        {
            // When node is balanced (0), so are it's children, otherwise
            // one child is unbalanced (+-1).
            node.GetChild(-node.Data * 2 - 1).Data = -node.Data;
            node.GetChild(node.Data * 2 + 1).Data = 0;
            node.Data = 0;
        }

        public AVLTree()
            : base()
        { }

        public AVLTree(IEnumerable<T> values)
            : base(values)
        { }

        protected override void AdditionUpkeep(BinNode<T> @new)
        {
            Upkeep(@new.Parent, @new.Location, isAddition: true);
        }

        protected override void RemovalUpkeep(BinNode<T> @new, BinNode<T> parent, int location, BinNode<T> old)
        {
            Upkeep(parent, location, isAddition: false);
        }

        protected override bool RemoveFromLeft(BinNode<T> node)
        {
            return node.Data < 0;
        }
    }
}