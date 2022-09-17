namespace TreeEdit
{
    using System.Collections.Generic;

    using SearchTrees;

    internal static class State
    {
        public static SearchTree<int> Tree = new AVLTree<int>();
        public static List<int[]> Sets = new List<int[]>();
    }
}