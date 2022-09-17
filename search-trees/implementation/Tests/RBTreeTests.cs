using Microsoft.VisualStudio.TestTools.UnitTesting;

using SearchTrees;
using static SearchTrees.Iterators;
using static SearchTrees.RBTree<int>;

[TestClass]
public sealed class RBTreeTests : BinTreeTests
{
    public RBTreeTests()
        : base(new RBTree<int>())
    { }

    [TestMethod]
    public void RootIsAlwaysBlack()
    {
        this.OnChange((s, i, v) =>
        {
            Assert.IsTrue(IsBlack(this.GetRoot()));
        });
    }

    [TestMethod]
    public void LeftAndRightNodesHaveTheSameBlackHeight()
    {
        this.OnChange((s, i, v) =>
        {
            foreach (var node in LevelOrder(this.GetRoot()))
            {
                Assert.IsTrue(BlackHeight(node.Left) == BlackHeight(node.Right));
            }
        });
    }

    [TestMethod]
    public void RedNodesHaveBlackChildrenAndParent()
    {
        this.OnChange((s, i, v) =>
        {
            foreach (var node in LevelOrder(this.GetRoot()))
            {
                if (IsRed(node))
                {
                    Assert.IsTrue(IsBlack(node.Parent));
                    Assert.IsTrue(IsBlack(node.Left));
                    Assert.IsTrue(IsBlack(node.Right));
                }
            }
        });
    }
}