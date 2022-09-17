namespace TreeEdit
{
    using System;
    using System.Collections.Generic;

    using SearchTrees;

    internal static class Util
    {
        public static readonly Random Random = new Random();

        public static string TreeToString<T>(SearchTree<T> tree, bool verbose = false)
            where T : IComparable<T>
        {
            Type type = tree.GetType();

            string str = TreeTypeToString[type];

            if (verbose)
            {
                str += " tree";
            }

            if (type == typeof(BTree<T>))
            {
                var btree = tree as BTree<T>;

                if (verbose)
                {
                    str += $" of order {btree.Order}";
                }
                else
                {
                    str += $"({btree.Order})";
                }
            }

            return str;
        }

        public static readonly Dictionary<Type, string> TreeTypeToString = new Dictionary<Type, string>()
        {
            { typeof(BinTree<int>), "BIN" },
            { typeof(AVLTree<int>), "AVL" },
            { typeof(AATree<int>), "AA" },
            { typeof(RBTree<int>), "RB" },
            { typeof(BTree<int>), "B" },
        };
    }
}