namespace Demo
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using static Sorting.Sorts;

    using Utilities;

    static class Program
    {
        private static readonly int[] Numbers = { 1, 3, 4, 4, 5, 6, 9, 13, 17, 100 };

        public static void Main()
        {
            var shuffled = new int[] { 4, 6, 17, 100, 13, 5, 4, 9, 3, 1 };
            var sorts = new Dictionary<string, Action<IList<int>>>()
            {
                { "Selection Sort", SelectionSort },
                { "Insertion Sort", InsertionSort },
                { "Bubble Sort", BubbleSort },

                { "Quick Sort", QuickSort },
                { "Merge Sort", MergeSort },
                { "Heap Sort", HeapSort },

                { "Bucket Sort", n => BucketSort(n) },
            };

            foreach (var sort in sorts)
            {
                var result = Numbers.ToArray()
                    .Shuffle();

                sort.Value(result);

                Console.WriteLine($"{(sort.Key + ":").PadRight(10)} {result.Join(", ")}");
            }

            Console.WriteLine();
        }
    }
}