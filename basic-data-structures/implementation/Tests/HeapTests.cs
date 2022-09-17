using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using BDS.Heap;
using static BDS.Heap.Heap<int, int>;

using Utilities;

[TestClass]
public class HeapTests
{
    private static void AssertHasHeapProperty<K, V>(Heap<K, V> heap)
        where K : IComparable<K>
    {
        for (int i = 0; i < heap.Count / 2; i++)
        {
            int left = Left(i);
            if (left < heap.Count)
            {
                Assert.IsTrue(heap.Compare(heap.SelectKey(heap[i]), heap.SelectKey(heap[left])) >= 0);
            }

            int right = Right(i);
            if (right < heap.Count)
            {
                Assert.IsTrue(heap.Compare(heap.SelectKey(heap[i]), heap.SelectKey(heap[right])) >= 0);
            }
        }
    }

    private int[][] sets;
    private Composite[][] composites;

    public HeapTests()
    {
        this.sets = Data.Sets;
        this.MakeComposites();
    }

    [TestMethod]
    public void CtorMaintainsHeapProperty()
    {
        for (int s = 0; s < this.sets.Length; s++)
        {
            var heap = new Heap<int>(this.sets[s]);
            AssertHasHeapProperty(heap);
        }
    }

    [TestMethod]
    public void PushMaintainsHeapProperty()
    {
        var heap = new Heap<int>();

        for (int s = 0; s < this.sets.Length; s++)
        {
            for (int i = 0; i < this.sets[s].Length; i++)
            {
                heap.Push(this.sets[s][i]);
                AssertHasHeapProperty(heap);
            }

            heap.Clear();
        }
    }

    [TestMethod]
    public void PushKeepsCount()
    {
        var heap = new Heap<int>();

        for (int s = 0; s < this.sets.Length; s++)
        {
            for (int i = 0; i < this.sets[s].Length; i++)
            {
                heap.Push(this.sets[s][i]);
                Assert.AreEqual(i + 1, heap.Count);
            }

            heap.Clear();
        }
    }

    [TestMethod]
    public void PopMaintainsHeapProperty()
    {
        for (int s = 0; s < this.sets.Length; s++)
        {
            var heap = new Heap<int>(this.sets[s]);

            while (heap.Any())
            {
                heap.Pop();
                AssertHasHeapProperty(heap);
            }
        }
    }

    [TestMethod]
    public void PopReturnsMaxElement()
    {
        for (int i = 0; i < this.composites.Length; i++)
        {
            var heap = new Heap<int, Composite>(c => c.Value, this.composites[i]);
            var sorted = this.composites[i].OrderByDescending(c => c.Value, Comparer<int>.Create(heap.Compare))
                .ToArray();

            var popped = new System.Collections.Generic.List<Composite>();
            while (heap.Any())
            {
                popped.Add(heap.Pop());
            }

            Assert.IsTrue(sorted.ValuesEqual(popped));
        }
    }

    [TestMethod]
    public void PopKeepsCount()
    {
        for (int s = 0; s < this.sets.Length; s++)
        {
            var heap = new Heap<int>(this.sets[s]);
            int count = this.sets[s].Length;

            Assert.AreEqual(count, heap.Count);
            while (heap.Any())
            {
                heap.Pop();
                count--;

                Assert.AreEqual(count, heap.Count);
            }
        }
    }

    [TestMethod]
    public void PeekDoesntRemoveTheTop()
    {
        var heap = new Heap<int>(this.sets[0]);
        int count = heap.Count;

        Assert.AreEqual(heap.Peek(), heap.Peek());
        Assert.AreEqual(count, heap.Count);
    }

    [TestMethod]
    public void SlideMaintainsHeapProperty()
    {
        var heap = new Heap<int>(this.sets[0]);

        for (int s = 1; s < this.sets.Length; s++)
        {
            for (int i = 0; i < this.sets[s].Length; i++)
            {
                heap.Slide(this.sets[s][i]);

                AssertHasHeapProperty(heap);
            }
        }
    }

    [TestMethod]
    public void SlideKeepsCount()
    {
        var heap = new Heap<int>(this.sets[0]);
        int count = heap.Count;

        for (int s = 1; s < this.sets.Length; s++)
        {
            for (int i = 0; i < this.sets[s].Length; i++)
            {
                heap.Slide(this.sets[s][i]);
                Assert.AreEqual(count, heap.Count);
            }
        }
    }

    [TestMethod]
    public void PopPeekAndSlideReturnTheTopElement()
    {
        var heap = new Heap<int>(this.sets[0]);

        Assert.AreEqual(heap[0], heap.Peek());
        Assert.AreEqual(heap.Peek(), heap.Slide(heap.Peek()));
        Assert.AreEqual(heap.Peek(), heap.Pop());
    }

    [TestMethod]
    public void PopPeekAndSlideThrowOnEmptyHeap()
    {
        var heap = new Heap<int>();

        Assert.ThrowsException<InvalidOperationException>(() => heap.Pop());
        Assert.ThrowsException<InvalidOperationException>(() => heap.Peek());
        Assert.ThrowsException<InvalidOperationException>(() => heap.Slide(3));
    }

    [TestMethod]
    public void ClearsCorrectly()
    {
        var heap = new Heap<int>(this.sets[0]);
        Assert.AreEqual(this.sets[0].Length, heap.Count);
        heap.Peek();

        heap.Clear();
        Assert.AreEqual(0, heap.Count);
        Assert.ThrowsException<InvalidOperationException>(() => heap.Peek());
    }

    [TestMethod]
    public void UpdateMaintainsHeapProperty()
    {
        for (int i = 0; i < this.composites.Length; i++)
        {
            var heap = new Heap<int, Composite>(c => c.Value, this.composites[i]);

            for (int h = 0; h < heap.Count; h++)
            {
                if (h % 2 == 0)
                {
                    heap[h].Value -= h;
                }
                else
                {
                    heap[h].Value += h;
                }

                heap.Update(h);

                AssertHasHeapProperty(heap);
            }
        }
    }

    private void MakeComposites()
    {
        this.composites = new Composite[this.sets.Length][];

        for (int i = 0; i < this.composites.Length; i++)
        {
            this.composites[i] = new Composite[this.sets[i].Length];

            for (int c = 0; c < this.composites[i].Length; c++)
            {
                this.composites[i][c] = new Composite($"Name-{i}-{c}", this.sets[i][c]);
            }
        }
    }
}

class Composite
{
    public Composite(string name, int value)
    {
        this.Name = name;
        this.Value = value;
    }

    public string Name { get; set; }

    public int Value { get; set; }
}