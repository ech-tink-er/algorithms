using Microsoft.VisualStudio.TestTools.UnitTesting;

using SearchTrees;
using static SearchTrees.Iterators;

[TestClass]
public sealed class AATreeTests : BinTreeTests
{
    public AATreeTests()
        : base(new AATree<int>())
    { }

    [TestMethod]
    public void AllLeavesAreAtLevelOne()
    {
        this.OnChange((s, i, v) =>
        {
            foreach (var node in LevelOrder(this.GetRoot()))
            {
                if (node.Left == null && node.Right == null)
                {
                    Assert.AreEqual(1, node.Data);
                }
            }
        });
    }

    [TestMethod]
    public void AllNodesWithTwoChildrenAreAtLeastLevelTwo()
    {
        this.OnChange((s, i, v) =>
        {
            foreach (var node in LevelOrder(this.GetRoot()))
            {
                if (node.Left != null && node.Right != null)
                {
                    Assert.IsTrue(2 <= node.Data);
                }
            }
        });
    }

    [TestMethod]
    public void LeftNodesAreAlwaysOneLevelLower()
    {
        this.OnChange((s, i, v) =>
        {
            foreach (var node in LevelOrder(this.GetRoot()))
            {
                if (node.Left != null)
                {
                    Assert.AreEqual(node.Data - 1, node.Left.Data);
                }
            }
        });
    }

    [TestMethod]
    public void RightNodesAreTheSameLevelOrOneLevelLess()
    {
        this.OnChange((s, i, v) =>
        {
            foreach (var node in LevelOrder(this.GetRoot()))
            {
                if (node.Right != null)
                {
                    Assert.IsTrue(node.Right.Data == node.Data || node.Right.Data == node.Data - 1);
                }
            }
        });
    }

    [TestMethod]
    public void RightGrandchildrenLevelIsStrictlyLess()
    {
        this.OnChange((s, i, v) =>
        {
            foreach (var node in LevelOrder(this.GetRoot()))
            {
                if (node.Right != null && node.Right.Right != null)
                {
                    Assert.IsTrue(node.Data > node.Right.Right.Data);
                }
            }
        });
    }
}