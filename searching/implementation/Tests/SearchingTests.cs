using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using static Searching;

[TestClass]
public class SeachingTests
{
    private int[][] sets;

    public SeachingTests()
    {
        this.sets = Data.Sets;

        this.sets = new int[Data.Sets.Length][];
        for (int i = 0; i < this.sets.Length; i++)
        {
            this.sets[i] = Data.Sets[i].OrderBy(n => n)
                .ToArray();
        }
    }

    [TestMethod]
    public void TestSearch()
    {
        this.TestSearch(Search);
    }

    [TestMethod]
    public void TestBinSearch()
    {
        this.TestSearch((set, query) => BinSearch(set.OrderBy(n => n).ToArray(), query));
    }

    [TestMethod]
    public void TestInterpolationSearch()
    {
        this.TestSearch((set, query) => InterpolationSearch(set, query));
    }

    private void TestSearch(Func<IReadOnlyList<int>, int, int> search, int sets = -1)
    {
        if (sets < 0)
        {
            sets = this.sets.Length;
        }

        for (int s = 0; s < sets; s++)
        {
            for (int i = 0; i < this.sets[s].Length; i++)
            {
                int res = search(this.sets[s], this.sets[s][i]);

				Assert.AreEqual(this.sets[s][i], this.sets[s][res]);
            }
        }
    }
}