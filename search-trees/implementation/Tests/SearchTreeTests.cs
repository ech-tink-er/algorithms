using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SearchTrees;

using Utilities;

public abstract class SearchTreeTests
{
    protected readonly int[][] sets;

    protected SearchTree<int> tree;

    protected PrivateObject treePrivates;

    protected SearchTreeTests(int[][] sets)
    {
        this.sets = sets;
    }

    protected SearchTreeTests()
        : this(Data.Sets)
    { }

    protected SearchTreeTests(SearchTree<int> tree, int[][] sets)
        : this(sets)
    {
        this.NewTree(tree);
    }

    protected SearchTreeTests(SearchTree<int> tree)
        : this(tree, Data.Sets)
    { }

    [TestMethod]
    public void KeepsItemsSorted()
    {
        this.OnSet(s =>
        {
            Assert.IsTrue(this.tree.IsSorted());
        });
    }

    [TestMethod]
    public void DoesntKeepDuplicates()
    {
        this.tree.Clear();

        this.tree.Add(7);
        this.tree.Add(7);

        Assert.AreEqual(1, this.tree.Count);

        Assert.IsTrue(this.tree.Remove(7));

        Assert.AreEqual(0, this.tree.Count);
        Assert.IsFalse(this.tree.Contains(7));
    }

    [TestMethod]
    public void StoresAndFindsAllItems()
    {
        this.OnSet(s =>
        {
            Assert.IsTrue(this.sets[s].All(n => this.tree.Contains(n)));
        });
    }

    [TestMethod]
    public void FindsMin()
    {
        this.tree.Clear();

        for (int s = 0; s < this.sets.Length; s++)
        {
            this.tree.Add(this.sets[s]);

            Assert.AreEqual(this.sets[s].Min(), this.tree.Min());

            this.tree.Clear();

            Assert.ThrowsException<InvalidOperationException>(() => this.tree.Min());
        }
    }

    [TestMethod]
    public void FindsMax()
    {
        this.tree.Clear();

        for (int s = 0; s < this.sets.Length; s++)
        {
            this.tree.Add(this.sets[s]);

            Assert.AreEqual(this.sets[s].Max(), this.tree.Max());

            this.tree.Clear();

            Assert.ThrowsException<InvalidOperationException>(() => this.tree.Max());
        }
    }

    [TestMethod]
    public void InsertKeepsCount()
    {
        this.tree.Clear();

        for (int s = 0; s < this.sets.Length; s++)
        {
            var hashset = new HashSet<int>();

            Assert.AreEqual(hashset.Count, this.tree.Count);
            for (int i = 0; i < this.sets[s].Length; i++)
            {
                hashset.Add(this.sets[s][i]);
                this.tree.Add(this.sets[s][i]);

                Assert.AreEqual(hashset.Count, this.tree.Count);
            }

            hashset.Clear();
            this.tree.Clear();
        }
    }

    [TestMethod]
    public void RemoveKeepsCount()
    {
        this.tree.Clear();

        for (int s = 0; s < this.sets.Length; s++)
        {
            var hashset = new HashSet<int>();

            hashset.UnionWith(this.sets[s]);
            this.tree.Add(this.sets[s]);

            Assert.AreEqual(hashset.Count, this.tree.Count);
            for (int i = 0; i < this.sets[s].Length; i++)
            {
                hashset.Remove(this.sets[s][i]);
                this.tree.Remove(this.sets[s][i]);

                Assert.AreEqual(hashset.Count, this.tree.Count);
            }
        }
    }

    [TestMethod]
    public void RemoveRemovesOnlyGivenValue()
    {
        this.OnSet(s =>
        {
            var list = this.sets[s].Distinct()
                .OrderBy(n => n)
                .ToList();

            for (int i = 0; i < this.sets[s].Length; i++)
            {
                this.tree.Remove(this.sets[s][i]);
                list.Remove(this.sets[s][i]);

                Assert.IsTrue(this.tree.ValuesEqual(list));
            }
        });
    }

    [TestMethod]
    public void RemoveDoesNothingOnMissingValue()
    {
        this.tree.Clear();
        this.tree.Add(this.sets[0]);

        int missing = int.MinValue;
        for (; this.tree.Contains(missing); missing++) ;

        Assert.IsFalse(this.tree.Remove(missing));
        Assert.IsTrue(this.tree.ValuesEqual(this.sets[0].OrderBy(n => n)));
    }

    [TestMethod]
    public void ClearsCorrectly()
    {
        this.tree.Clear();
        this.tree.Add(this.sets[0]);

        Assert.IsTrue(this.tree.Any());

        this.tree.Clear();

        Assert.IsFalse(this.tree.Any());
    }

    [TestMethod]
    public void CopyToWorksCorrectly()
    {
        const int Extra = 2;

        this.OnSet(s =>
        {
            int[] array = new int[this.tree.Count + Extra];

            Assert.ThrowsException<InvalidOperationException>(() => this.tree.CopyTo(array, Extra + 1));

            this.tree.CopyTo(array, Extra);

            Assert.IsTrue(array.ValuesEqual(new int[Extra].Concat(this.tree)));
        });
    }

    [TestMethod]
    public void ToArrayIsInOrder()
    {
        this.OnSet(s =>
        {
            int[] array = this.tree.ToArray();

            Assert.AreEqual(this.tree.Count, array.Length);

            Assert.IsTrue(this.tree.ValuesEqual(array));
        });
    }

    protected virtual void OnSet(Action<int> action)
    {
        this.tree.Clear();

        for (int s = 0; s < this.sets.Length; s++)
        {
            this.tree.Add(this.sets[s]);

            action(s);

            this.tree.Clear();
        }
    }

    protected virtual void OnInsert(Action<int, int, int> action)
    {
        this.tree.Clear();

        for (int s = 0; s < this.sets.Length; s++)
        {
            for (int i = 0; i < this.sets[s].Length; i++)
            {
                this.tree.Add(this.sets[s][i]);

                action(s, i, this.sets[s][i]);
            }

            this.tree.Clear();
        }
    }

    protected virtual void OnRemove(Action<int, int, int> action)
    {
        this.tree.Clear();

        for (int s = 0; s < this.sets.Length; s++)
        {
            this.tree.Add(this.sets[s]);

            for (int i = 0; i < this.sets[s].Length; i++)
            {
                this.tree.Remove(this.sets[s][i]);

                action(s, i, this.sets[s][i]);
            }
        }
    }

    protected virtual void OnChange(Action<int, int, int> action)
    {
        this.tree.Clear();

        for (int s = 0; s < this.sets.Length; s++)
        {
            for (int i = 0; i < this.sets[s].Length; i++)
            {
                this.tree.Add(this.sets[s][i]);

                action(s, i, this.sets[s][i]);
            }

            for (int i = 0; i < this.sets[s].Length; i++)
            {
                this.tree.Remove(this.sets[s][i]);

                action(s, i, this.sets[s][i]);
            }
        }
    }

    protected void NewTree(SearchTree<int> tree)
    {
        this.tree = tree;
        this.treePrivates = new PrivateObject(this.tree);
    }
}