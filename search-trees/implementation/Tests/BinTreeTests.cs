using Microsoft.VisualStudio.TestTools.UnitTesting;

using SearchTrees;
using static SearchTrees.Iterators;

[TestClass]
public class BinTreeTests : SearchTreeTests
{
    protected BinTreeTests(int[][] sets)
        : base(sets)
    { }

    protected BinTreeTests(BinTree<int> tree, int[][] sets)
        : base(tree, sets)
    { }

    protected BinTreeTests(BinTree<int> tree)
    : base(tree)
    { }

    public BinTreeTests()
    : base(new BinTree<int>())
    { }

    [TestMethod]
    public void AllLeftNodesHaveLesserValues()
    {
        this.OnChange((s, i, v) =>
        {
            foreach (var node in LevelOrder(this.GetRoot()))
            {
                Assert.IsTrue(node.Left == null || node.Left.Value < node.Value);
            }
        });
    }

    [TestMethod]
    public void AllRightNodesHaveGreaterValues()
    {
        this.OnChange((s, i, v) =>
        {
            foreach (var node in LevelOrder(this.GetRoot()))
            {
                Assert.IsTrue(node.Right == null || node.Value < node.Right.Value);
            }
        });
    }

    protected BinNode<int> GetRoot()
    {
        return ((BinTree<int>)this.tree).root;
    }
}