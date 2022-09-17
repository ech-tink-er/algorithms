using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Utilities;

using static Sorting.Sorts;

[TestClass]
public class SortingTests
{
    private int[][] sets;
    private int[][] sorted;

    public SortingTests()
    {
        this.sets = Data.Sets;

        this.sorted = new int[this.sets.Length][];
        for (int i = 0; i < this.sorted.Length; i++)
        {
            this.sorted[i] = this.sets[i].OrderBy(n => n)
                .ToArray();
        }
    }

    [TestMethod]
    public void TestSelectionSort()
    {
        this.TestSort(SelectionSort);
    }

    [TestMethod]
    public void TestInsertionSort()
    {
        this.TestSort(InsertionSort);
    }

    [TestMethod]
    public void TestBubbleSort()
    {
        this.TestSort(BubbleSort);
    }

    [TestMethod]
    public void TestQuickSort()
    {
        this.TestSort(QuickSort);
    }

    [TestMethod]
    public void TestMergeSort()
    {
        this.TestSort(MergeSort);
    }

    [TestMethod]
    public void TestHeapSort()
    {
        this.TestSort(HeapSort);
    }

    [TestMethod]
    public void TestCountingSort()
    {
    }

    [TestMethod]
    public void TestBucketSort()
    {
        this.TestSort(set => BucketSort(set));
    }

    private void TestSort(Action<IList<int>> sort, int sets = -1)
    {
        if (sets < 0)
        {
            sets = this.sets.Length;
        }

        for (int s = 0; s < sets; s++)
        {
            var res = this.sets[s].ToArray();
            sort(res);
            Assert.IsTrue(res.ValuesEqual(this.sorted[s]));
        }
    }
}