using Microsoft.VisualStudio.TestTools.UnitTesting;

using SearchTrees;
using static SearchTrees.Iterators;
using static SearchTrees.BinTree<int>;

[TestClass]
public sealed class AVLTreeTests : BinTreeTests
{
    public AVLTreeTests()
        : base(new AVLTree<int>())
    { }

    [TestMethod]
    public void KeepsCorrectBalanceFactors()
    {
        this.OnChange((s, i, v) =>
        {
            foreach (var node in LevelOrder(this.GetRoot()))
            {
                Assert.AreEqual(GetHeight(node.Right) - GetHeight(node.Left), node.Data);
            }
        });
    }

    [TestMethod]
    public void MaintainsProperBalance()
    {
        this.OnChange((s, i, v) =>
        {
            foreach (var node in LevelOrder(this.GetRoot()))
            {
                Assert.IsTrue(-1 <= node.Data && node.Data <= 1);
            }
        });
    }
}