using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using HashTables;

[TestClass]
public class MapTests : HashTableTests<Binding<string, int>>
{
    private static Binding<string, int>[][] GetPairs(int[][] sets)
    {
        var keys = GetKeys(sets);

        var pairs = new Binding<string, int>[keys.Length][];

        for (int s = 0; s < sets.Length; s++)
        {
            pairs[s] = new Binding<string, int>[sets[s].Length];
            for (int i = 0; i < sets[s].Length; i++)
            {
                pairs[s][i] = new Binding<string, int>(keys[s][i], sets[s][i]);
            }
        }

        return pairs;
    }

    private Map<string, int> map;

    public MapTests()
        : base(new Map<string, int>(), GetPairs(Data.SetsDistinct))
    {
        this.map = (Map<string, int>)this.table;
    }


    [TestMethod]
    public void FindsAllValues()
    {
        for (int s = 0; s < this.values.Length; s++)
        {
            this.table.Clear();

            for (int i = 0; i < this.values[s].Length; i++)
            {
                this.table.Add(this.values[s][i]);

                Assert.IsTrue(this.map.Contains(this.values[s][i].Value));
            }
        }
    }

    [TestMethod]
    public void SetThrowsOnMissingKey()
    {
        this.map.Clear();

        Assert.ThrowsException<InvalidOperationException>(() => this.map["not a key"]);
    }
}