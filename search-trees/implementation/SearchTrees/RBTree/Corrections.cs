namespace SearchTrees
{
    public sealed partial class RBTree<T> 
    {
        private static class AdditionCorrections
        {
            public static bool Correct(BinNode<T> node)
            {
                return SimpleCorrect(node) || RotationCorrect(node);
            }

            public static BinNode<T> MoveCorrection(BinNode<T> node)
            {
                var parent = node.Parent;
                var grandparent = parent.Parent;
                var uncle = grandparent.GetChild(-parent.Location);

                ColorBlack(parent);
                ColorBlack(uncle);
                ColorRed(grandparent);

                return grandparent;
            }

            private static bool SimpleCorrect(BinNode<T> node)
            {
                if (node.Parent == null)
                {
                    ColorBlack(node);
                    return true;
                }

                if (IsBlack(node.Parent))
                {
                    return true;
                }

                return false;
            }

            private static bool RotationCorrect(BinNode<T> node)
            {
                var parent = node.Parent;
                var grandparent = parent.Parent;
                var uncle = grandparent.GetChild(-parent.Location);
                if (IsRed(uncle))
                {
                    return false;
                }

                int direction = -parent.Location;

                var sibling = grandparent;
                if (node.Location + parent.Location == 0)
                {
                    grandparent = DoubleRotate(grandparent, direction);
                }
                else
                {
                    grandparent = Rotate(grandparent, direction);
                }

                ColorBlack(grandparent);
                ColorRed(sibling);

                return true;
            }
        }

        private static class RemovalCorrections
        {
            public static bool Correct(BinNode<T> next, BinNode<T> parent, int location, BinNode<T> old)
            {
                return SimpleCorrect(next, parent, old) ||
                    ColorFlipCorrect(next, parent, location) ||
                    RotationCorrect(next, parent, location);
            }

            public static BinNode<T> MoveCorrection(BinNode<T> next, BinNode<T> parent, int location)
            {
                // Set up repair one level down.
                var sibling = parent.GetChild(-location);
                if (IsRed(sibling))
                {
                    ColorRed(parent);
                    ColorBlack(sibling);

                    Rotate(parent, location);

                    return next;
                }

                // Set inbalance at parent.
                ColorRed(sibling);
                return parent;
            }

            private static bool SimpleCorrect(BinNode<T> next, BinNode<T> parent, BinNode<T> old)
            {
                if (IsRed(old))
                {
                    return true;
                }

                if (IsRed(next))
                {
                    ColorBlack(next);
                    return true;
                }

                if (parent == null)
                {
                    return true;
                }

                return false;
            }

            private static bool ColorFlipCorrect(BinNode<T> next, BinNode<T> parent, int location)
            {
                var sibling = parent.GetChild(-location);
                if (IsBlack(parent) || IsRed(sibling.Left) || IsRed(sibling.Right))
                {
                    return false;
                }

                ColorBlack(parent);
                ColorRed(sibling);

                return true;
            }

            private static bool RotationCorrect(BinNode<T> next, BinNode<T> parent, int location)
            {
                var sibling = parent.GetChild(-location);

                var outer = sibling.GetChild(sibling.Location);
                var inner = sibling.GetChild(-sibling.Location);
                if (IsBlack(inner) && IsBlack(outer))
                {
                    return false;
                }

                BinNode<T> grandparent;
                if (IsRed(outer))
                {
                    grandparent = Rotate(parent, location);
                }
                else
                {
                    grandparent = DoubleRotate(parent, location);
                }

                bool parentColor = IsBlack(parent);
                ColorBlack(parent);
                SetColor(grandparent, parentColor);

                ColorBlack(grandparent.GetChild(-location));

                return true;
            }
        }
    }
}