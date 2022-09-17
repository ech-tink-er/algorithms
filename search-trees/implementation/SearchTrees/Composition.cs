namespace SearchTrees
{
    using System;
    using System.Text;
    using System.Linq;
    using System.Collections.Generic;

    using static Iterators;

    using Utilities;

	internal static class Composition
    {
        public static string ListCompose<T>(BinTree<T> tree, bool verbose = false)
            where T : IComparable<T>
        {
            var values = InOrder(tree.root)
                .Select(node => tree.FormatNode(node, verbose));

            return string.Join(", ", values);
        }

        public static string VerticalCompose<T>(BinTree<T> tree, bool verbose = false)
            where T : IComparable<T>
        {
            var result = new StringBuilder();

            var depths = new Dictionary<BinNode<T>, int>();
            depths[tree.root] = 0;

            foreach (var node in PreOrderRL(tree.root))
            {
                if (node != tree.root)
                {
                    depths[node] = depths[node.Parent] + 1;
                }

                string indent = new string('\t', depths[node]);
                result.AppendLine(indent + tree.FormatNode(node, verbose));
            }

            return result.ToString().TrimEnd();
        }

        public static string VerticalCompose<T>(BTree<T> tree)
            where T : IComparable<T>
        {
            var result = new StringBuilder();

            var depths = new Dictionary<BNode<T>, int>();
            depths[tree.root] = 0;

            foreach (var node in PreOrderRL(tree.root))
            {
                if (node != tree.root)
                {
                    depths[node] = depths[node.Parent] + 1;
                }

                string indent = new string('\t', depths[node]);
                result.AppendLine(indent + node);
            }

            return result.ToString().TrimEnd();
        }

        public static string HorizontalCompose<T>(BinTree<T> tree, bool verbose = false)
            where T : IComparable<T>
        {
            string[][] t = tree.ToTreeArray()
                .Select(level =>
                {
                    return level.Select(node =>
                    {
                        return "(" + tree.FormatNode(node, verbose) + ")";
                    }).ToArray();
                }).ToArray();

            var levels = verbose ? VerboseComposeLevels(t) : ComposeLevels(t);

            return levels.Join(Environment.NewLine);
        }

        public static string HorizontalCompose<T>(BTree<T> tree, bool verbose = false)
            where T : IComparable<T>
        {
            var tarray = tree.ToTreeArray();
            string[] levels;
            if (verbose)
            {
                levels = VerboseComposeLevels(tarray);
            }
            else
            {
                levels = ComposeLevels(
                    tarray.Select(l => l.Select(n => n.ToString()).ToArray())
                        .ToArray()
                );
            }

            return levels.Join(Environment.NewLine);
        }

        private static string[] ComposeLevels(string[][] tree)
        {
            string[] levels = tree.Select(nodes => nodes.Join(" "))
                .ToArray();

            int max = levels.Max(level => level.Length);

            levels = levels.Select(level => level.PadLeft(level.Length + (max - level.Length + 1) / 2))
                .ToArray();

            return levels;
        }

        private static string[] VerboseComposeLevels(string[][] tree)
        {
            string[] levels = new string[tree.Length];

            int max = tree.Max(nodes => nodes.Max(n => n.Length));
            for (int i = levels.Length - 1, blockSize = max + 2; i >= 0; i--, blockSize *= 2)
            {
                levels[i] = tree[i]
                    .Select(value => value.PadDouble(blockSize))
                    .Join("")
                    .TrimEnd();
            }

            return levels;
        }

        private static string[] VerboseComposeLevels<T>(BNode<T>[][] tree)
            where T : IComparable<T>
        {
            const string Separator = " | ";

            string[] levels = new string[tree.Length];

            var starts = new Dictionary<BNode<T>, int>();
            var lengths = new Dictionary<BNode<T>, int>();
            var last = tree.Last();
            for (int i = 0; i < last.Length; i++)
            {
                starts[last[i]] = 0;
                lengths[last[i]] = 0;
            }

            var level = new List<char>();
            for (int l = levels.Length - 1; l >= 0; l--)
            {
                var groups = tree[l].GroupBy(node => node.Parent);

                foreach (var group in groups)
                {
                    int start = level.Count;

                    foreach (var node in group)
                    {
                        int length = level.Count;
                        if (l == levels.Length - 1 && level.Any())
                        {
                            length = -1;
                        }

                        int leftPadding = starts[node] - length;

                        string str = node.ToString().PadDouble(lengths[node]);
                        str = str.PadLeft(leftPadding + str.Length);

                        level.AddRange(str);
                    }

                    level.AddRange(Separator);

                    var parent = group.Key;
                    if (parent != null)
                    {
                        starts[parent] = level.IndexOf('[', start);
                        lengths[parent] = level.LastIndexOf(']') + 1 - starts[parent];
                    }
                }
                level.RemoveRange(level.Count - Separator.Length, Separator.Length);
                levels[l] = new string(level.ToArray());
                level.Clear();
            }

            return levels;
        }
    }
}