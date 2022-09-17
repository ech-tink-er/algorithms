using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using HashTables;

public abstract class HashTableTests<V>
{
    protected static string GetKey(int number)
    {
        return $"THENAME[{number}]";
    }

    protected static string[][] GetKeys(int[][] sets)
    {
        return sets.Select(s => s.Select(n => $"THENAME[{n}]")
            .ToArray())
        .ToArray();
    }

    protected V[][] values;
    protected HashTable<string, V> table;

    protected HashTableTests(HashTable<string, V> table, V[][] values)
    {
        this.values = values;
        this.table = table;
    }

    [TestMethod]
    public void AddKeepsCount()
    {
        for (int s = 0; s < this.values.Length; s++)
        {
            this.table.Clear();

            Assert.AreEqual(0, this.table.Count);
            for (int i = 0; i < this.values[s].Length; i++)
            {
                this.table.Add(this.values[s][i]);

                Assert.AreEqual(i + 1, this.table.Count);
            }
        }
    }

    [TestMethod]
    public void AddStaysBelowMaxLoadFactor()
    {
        for (int s = 0; s < this.values.Length; s++)
        {
            this.table.Clear();

            for (int i = 0; i < this.values[s].Length; i++)
            {
                this.table.Add(this.values[s][i]);

                Assert.IsTrue(this.table.LoadFactor() <= HashTable<string, V>.MaxLoadFactor);
            }
        }
    }

    [TestMethod]
    public void RemoveKeepsCount()
    {
        this.table.Clear();

        for (int s = 0; s < this.values.Length; s++)
        {
            for (int i = 0; i < this.values[s].Length; i++)
            {
                this.table.Add(this.values[s][i]);
            }

            Assert.AreEqual(this.values[s].Length, this.table.Count);
            for (int i = 0; i < this.values[s].Length; i++)
            {
                this.table.Remove(this.table.SelectKey(this.values[s][i]));

                Assert.AreEqual(this.values[s].Length - 1 - i, this.table.Count);
            }
        }
    }

    [TestMethod]
    public void RemoveStaysAboveMinLoadFactor()
    {
        for (int s = 0; s < this.values.Length; s++)
        {
            this.table.Clear();

            for (int i = 0; i < this.values[s].Length; i++)
            {
                this.table.Add(this.values[s][i]);
            }

            for (int i = 0; i < this.values[s].Length; i++)
            {
                this.table.Remove(this.table.SelectKey(this.values[s][i]));

                bool shrinkable = HashTable<string, V>.MinBuckets <= (this.table.buckets.Length / HashTable<string, V>.GrowthFactor);
                Assert.IsTrue(HashTable<string, V>.MinLoadFactor <= this.table.LoadFactor() ||
                    !shrinkable);
            }
        }
    }

    [TestMethod]
    public void RemoveDoesntGoBelowMinBuckets()
    {
        for (int s = 0; s < this.values.Length; s++)
        {
            this.table.Clear();

            for (int i = 0; i < this.values[s].Length; i++)
            {
                this.table.Add(this.values[s][i]);
            }

            for (int i = 0; i < this.values[s].Length; i++)
            {
                this.table.Remove(this.table.SelectKey(this.values[s][i]));

                Assert.IsTrue(HashTable<string, V>.MinBuckets <= this.table.buckets.Length);
            }
        }
    }

    [TestMethod]
    public void RemoveRemovesKey()
    {
        this.table.Clear();

        for (int s = 0; s < this.values.Length; s++)
        {
            for (int i = 0; i < this.values[s].Length; i++)
            {
                this.table.Add(this.values[s][i]);
            }

            for (int i = 0; i < this.values[s].Length; i++)
            {
                Assert.IsTrue(this.table.Contains(this.values[s][i]));
                Assert.IsTrue(this.table.Remove(this.table.SelectKey(this.values[s][i])));
                Assert.IsFalse(this.table.Contains(this.values[s][i]));
            }
        }
    }

    [TestMethod]
    public void RemoveDoesNothingOnMissingKey()
    {
        this.table.Clear();

        var keys = this.values[0];
        var values = this.values[0];
        for (int i = 0; i < keys.Length; i++)
        {
            this.table.Add(values[i]);
        }

        Assert.AreEqual(keys.Length, this.table.Count);
        Assert.IsFalse(this.table.Remove("not a key"));
        Assert.AreEqual(keys.Length, this.table.Count);
    }

    [TestMethod]
    public void ClearsCorrectly()
    {
        this.table.Clear();
        Assert.AreEqual(0, this.table.Count);

        var keys = this.values[0];
        var values = this.values[0];
        for (int i = 0; i < keys.Length; i++)
        {
            this.table.Add(values[i]);
        }

        Assert.AreEqual(keys.Length, this.table.Count);
        Assert.IsTrue(this.table.Contains(keys[0]));

        this.table.Clear();

        Assert.AreEqual(0, this.table.Count);
        Assert.IsFalse(this.table.Contains(keys[0]));
    }

    [TestMethod]
    public void FindsAllKeys()
    {
        for (int s = 0; s < this.values.Length; s++)
        {
            this.table.Clear();

            for (int i = 0; i < this.values[s].Length; i++)
            {
                this.table.Add(this.values[s][i]);

                Assert.IsTrue(this.table.Contains(this.values[s][i]));
            }
        }
    }

    [TestMethod]
    public void OverridesDuplicateKeys()
    {
        this.table.Clear();

        var key = this.values[0][0];
        var value = this.values[0][0];

        this.table.Add(value);
        this.table.Add(value);

        Assert.AreEqual(1, this.table.Count);

        Assert.IsTrue(this.table.Contains(key));
    }

    [TestMethod]
    public void EnumeratesValues()
    {
        for (int s = 0; s < this.values.Length; s++)
        {
            this.table.Clear();

            for (int i = 0; i < this.values[s].Length; i++)
            {
                this.table.Add(this.values[s][i]);
            }

            var values = this.table.ToArray();
            Assert.IsTrue(this.values[s].All(v => values.Contains(v)));
        }
    }
}