using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SearchTrees;
using static SearchTrees.Iterators;

using Utilities;

[TestClass]
public sealed class BTreeTests : SearchTreeTests
{
    private const int MinOrder = BTree<int>.MinOrder;
    private const int MaxOrder = 8;

    public BTreeTests()
        : base(new BTree<int>())
    { }

    [TestMethod]
    public void CantCreateBTreeOfOrderLessThanMin()
    {
        Assert.ThrowsException<ArgumentException>(() => new BTree<int>(MinOrder - 1));
    }

    [TestMethod]
    public void KeysAreAlwaysSorted()
    {
        this.OnChange((s, i, v) =>
        {
            foreach (var node in LevelOrder(this.GetRoot()))
            {
                Assert.IsTrue(node.Keys.IsSorted());
            }
        });
    }

    [TestMethod]
    public void NonLeafKeysProperlySeparateChildren()
    {
        this.OnChange((s, i, v) =>
        {
            foreach (var node in LevelOrder(this.GetRoot()))
            {
                if (!node.Children.Any())
                {
                    return;
                }

                for (int k = 0; k < node.Keys.Count; k++)
                {
                    Assert.IsTrue(node.Children[k].Keys.All(key => key < node.Keys[k]));
                    Assert.IsTrue(node.Children[k + 1].Keys.All(key => node.Keys[k] < key));
                }
            }
        });
    }

    [TestMethod]
    public void NeverExceedsMaxKeys()
    {
        this.OnChange((s, i, v) =>
        {
            foreach (var node in LevelOrder(this.GetRoot()))
            {
                Assert.IsTrue(node.Keys.Count <= node.MaxKeys);
            }
        });
    }

    [TestMethod]
    public void NeverExceedsMaxChildren()
    {
        this.OnChange((s, i, v) =>
        {
            foreach (var node in LevelOrder(this.GetRoot()))
            {
                Assert.IsTrue(node.Children.Count <= node.MaxChildren);
            }
        });
    }

    [TestMethod]
    public void KeepsNonRootsWithAtLeastMinKeys()
    {
        this.OnChange((s, i, v) =>
        {
            var root = this.GetRoot();

            foreach (var node in LevelOrder(this.GetRoot()))
            {
                if (node != root)
                {
                    Assert.IsTrue(node.MinKeys <= node.Keys.Count);
                }
            }
        });
    }

    [TestMethod]
    public void KeepsInternalNodesWithAtLeastMinChildren()
    {
        this.OnChange((s, i, v) =>
        {
            var root = this.GetRoot();

            foreach (var node in LevelOrder(this.GetRoot()))
            {
                if (node != root && node.Children.Any())
                {
                    Assert.IsTrue(node.MinChildren <= node.Children.Count);
                }
            }
        });
    }

    [TestMethod]
    public void NeverGrowsKeysCapacity()
    {
        this.NewTree(new BTree<int>());

        this.OnChange((s, i, v) =>
        {
            foreach (var node in LevelOrder(this.GetRoot()))
            {
                var keys = (List<int>)new PrivateObject(node).GetField("keys");

                Assert.AreEqual(node.MaxKeys + 1, keys.Capacity);
            }
        });
    }

    [TestMethod]
    public void NeverGrowsChildrenCapacity()
    {
        this.NewTree(new BTree<int>());

        this.OnChange((s, i, v) =>
        {
            foreach (var node in LevelOrder(this.GetRoot()))
            {
                var keys = (List<BNode<int>>)new PrivateObject(node).GetField("children");

                Assert.AreEqual(node.MaxChildren + 1, keys.Capacity);
            }
        });
    }

    [TestMethod]
    public void KeysAreAlwaysOneLessThanChildrenInNonLeaves()
    {
        this.OnChange((s, i, v) =>
        {
            foreach (var node in LevelOrder(this.GetRoot()))
            {
                if (node.Children.Any())
                {
                    Assert.IsTrue(node.Keys.Count + 1 == node.Children.Count);
                }
            }
        });
    }

    [TestMethod]
    public void ThereIsNeverOnlyOneChild()
    {
        this.OnChange((s, i, v) =>
        {
            foreach (var node in LevelOrder(this.GetRoot()))
            {
                if (node.Children.Any())
                {
                    Assert.IsTrue(node.Children.Count != 1);
                }
            }
        });
    }

    [TestMethod]
    public void LeavesAreAlwaysTheSameHeight()
    {
        var depths = new Dictionary<BNode<int>, int>();

        this.OnChange((s, i, v) =>
        {
            var root = this.GetRoot();

            depths[root] = 0;
            int leafDepth = -1;

            foreach (var node in LevelOrder(root))
            {
                if (node != root)
                {
                    depths[node] = depths[node.Parent] + 1;
                }

                if (!node.Children.Any())
                {
                    if (leafDepth == -1)
                    {
                        leafDepth = depths[node];
                    }
                    else
                    {
                        Assert.AreEqual(leafDepth, depths[node]);
                    }
                }
            }

            depths.Clear();
        });
    }

    protected override void OnInsert(Action<int, int, int> action)
    {
        for (int order = MinOrder; order <= MaxOrder; order++)
        {
            this.NewTree(new BTree<int>(order));

            base.OnInsert(action);
        }
    }

    protected override void OnRemove(Action<int, int, int> action)
    {
        for (int order = MinOrder; order <= MaxOrder; order++)
        {
            this.NewTree(new BTree<int>(order));

            base.OnRemove(action);
        }
    }

    protected override void OnChange(Action<int, int, int> action)
    {
        for (int order = MinOrder; order <= MaxOrder; order++)
        {
            this.NewTree(new BTree<int>(order));

            base.OnChange(action);
        }
    }

    private BNode<int> GetRoot()
    {
        return ((BTree<int>)this.tree).root;
    }
}