namespace SearchTrees
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    internal static class Iterators
    {
        public static IEnumerable<BinNode<T>> PreOrder<T>(BinNode<T> root)
            where T : IComparable<T>
        {
            if (root == null)
            {
                yield break;
            }

            var stack = new Stack<BinNode<T>>();
            stack.Push(root);

            while (stack.Any())
            {
                var current = stack.Pop();

                yield return current;

                if (current.Right != null)
                {
                    stack.Push(current.Right);
                }

                if (current.Left != null)
                {
                    stack.Push(current.Left);
                }
            }
        }

        public static IEnumerable<BNode<T>> PreOrder<T>(BNode<T> root)
            where T : IComparable<T>
        {
            if (root == null)
            {
                yield break;
            }

            var stack = new Stack<BNode<T>>();
            stack.Push(root);

            while (stack.Any())
            {
                var current = stack.Pop();

                yield return current;

                foreach (var child in current.Children.Reverse())
                {
                    stack.Push(child);
                }
            }
        }

        public static IEnumerable<BinNode<T>> PreOrderRL<T>(BinNode<T> root)
            where T : IComparable<T>
        {
            if (root == null)
            {
                yield break;
            }

            var stack = new Stack<BinNode<T>>();
            stack.Push(root);

            while (stack.Any())
            {
                var current = stack.Pop();

                yield return current;

                if (current.Left != null)
                {
                    stack.Push(current.Left);
                }

                if (current.Right != null)
                {
                    stack.Push(current.Right);
                }
            }
        }

        public static IEnumerable<BNode<T>> PreOrderRL<T>(BNode<T> root)
            where T : IComparable<T>
        {
            if (root == null)
            {
                yield break;
            }

            var stack = new Stack<BNode<T>>();
            stack.Push(root);

            while (stack.Any())
            {
                var current = stack.Pop();

                yield return current;

                foreach (var child in current.Children)
                {
                    stack.Push(child);
                }
            }
        }

        public static IEnumerable<BinNode<T>> InOrder<T>(BinNode<T> root)
            where T : IComparable<T>
        {
            if (root == null)
            {
                yield break;
            }

            var stack = new Stack<BinNode<T>>();
            stack.Push(root);

            while (true)
            {
                BinNode<T> current = stack.Peek();
                if (current != null)
                {
                    stack.Push(current.Left);
                    continue;
                }

                stack.Pop();
                if (!stack.Any())
                {
                    break;
                }

                current = stack.Pop();

                yield return current;

                stack.Push(current.Right);
            }
        }

        public static IEnumerable<BinNode<T>> LevelOrder<T>(BinNode<T> root)
            where T : IComparable<T>
        {
            if (root == null)
            {
                yield break;
            }

            var queue = new Queue<BinNode<T>>();
            queue.Enqueue(root);

            while (queue.Any())
            {
                var current = queue.Dequeue();

                yield return current;

                if (current.Left != null)
                {
                    queue.Enqueue(current.Left);
                }

                if (current.Right != null)
                {
                    queue.Enqueue(current.Right);
                }
            }
        }

        public static IEnumerable<BNode<T>> LevelOrder<T>(BNode<T> root)
            where T : IComparable<T>
        {
            if (root == null)
            {
                yield break;
            }

            var queue = new Queue<BNode<T>>();
            queue.Enqueue(root);

            while (queue.Any())
            {
                var current = queue.Dequeue();

                yield return current;

                foreach (var child in current.Children)
                {
                    queue.Enqueue(child);
                }
            }
        }
    }
}