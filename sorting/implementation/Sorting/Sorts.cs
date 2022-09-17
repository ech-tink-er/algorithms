namespace Sorting
{
    using System;
    using static System.Math;
    using System.Linq;
    using System.Collections.Generic;

    using Utilities;
    using static Utilities.Bitwise;
    using static Utilities.Misc;

    public delegate int Key<T>(T value);

    public static class Sorts
    {
        public static void SelectionSort<T>(IList<T> items)
            where T : IComparable<T>
        {
            for (int i = 0; i < items.Count - 1; i++)
            {
                int min = i;

                for (int j = i + 1; j < items.Count; j++)
                {
                    if (items[j].CompareTo(items[min]) < 0)
                    {
                        min = j;
                    }
                }

                items.Swap(i, min);
            }
        }

        public static void InsertionSort<T>(IList<T> items)
            where T : IComparable<T>
        {
            for (int i = 1; i < items.Count; i++)
            {
                T value = items[i];

                int hole = i;
                for (; hole >= 1 && items[hole - 1].CompareTo(value) > 0; hole--)
                {
                    items[hole] = items[hole - 1];
                }

                items[hole] = value;
            }
        }

        public static void BubbleSort<T>(IList<T> items)
            where T : IComparable<T>
        {
            bool isSorted;
            do
            {
                isSorted = true;

                for (int i = 0; i < items.Count - 1; i++)
                {
                    if (items[i].CompareTo(items[i + 1]) > 0)
                    {
                        items.Swap(i, i + 1);

                        isSorted = false;
                    }
                }
            } while (!isSorted);
        }

        public static void QuickSort<T>(IList<T> items)
            where T : IComparable<T>
        {
            QuickSort(items, 0, items.Count - 1);
        }

        public static void MergeSort<T>(IList<T> items)
            where T : IComparable<T>
        {
            var output = items;

            IList<T> next = new T[items.Count];

            for (int len = 1; len < items.Count; len *= 2)
            {
                for (int i = 0; i < items.Count; i += len * 2)
                {
                    int second = Min(i + len, items.Count);
                    int length = Min(i + len * 2, items.Count);
                    Merge(items, i, second, length, next);
                }

                Swap(ref items, ref next);
            }

            if (items != output)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    output[i] = items[i];
                }
            }
        }

        public static void HeapSort<T>(IList<T> items)
            where T : IComparable<T>
        {
            var heap = new BDS.Heap.Heap<T>(reverse: true, items: items);

            for (int i = 0; i < items.Count; i++)
            {
                items[i] = heap.Pop();
            }
        }

        public static void BucketSort(IList<int> numbers, int bucketCount = 0, int? start = null, int length = -1)
        {
            int from = start ?? numbers.Min();
            length = length == -1 ? numbers.Max() - from + 1 : length;
            bucketCount = bucketCount < 1 ? 5 : bucketCount;

            var buckets = new List<int>[bucketCount];

            int perBucket = (int)Ceiling((double)length / bucketCount);
            for (int i = 0; i < numbers.Count; i++)
            {
                int b = (numbers[i] - from) / perBucket;
                if (buckets[b] == null)
                {
                    buckets[b] = new List<int>();
                }

                buckets[b].Add(numbers[i]);
            }

            int n = 0;
            foreach (var bucket in buckets)
            {
                if (bucket == null || !bucket.Any())
                {
                    continue;
                }

                InsertionSort(bucket);

                for (int b = 0; b < bucket.Count; b++, n++)
                {
                    numbers[n] = bucket[b];
                }
            }
        }

        public static string[] MSDStringSort(string[] strs, Alphabet alphabet)
        {
            MSDStringSort(new Subarray<string>(strs), alphabet);

            return strs;
        }

        public static void MSDStringSort(Subarray<string> strs, Alphabet alphabet, int s = 0)
        {
            if (strs.Count == 1)
                return;

            Key<string> key = str => s < str.Length ? alphabet.Value(str[s]) : -1;

            int[] partitions = Partition(strs, key, -1, alphabet.Size - 1);

            if (partitions[1] == strs.Count)
                return;

            Distribute(strs, partitions, key, -1, alphabet.Size - 1, maintain: true);

            var range = (From: strs.From, To: strs.To);
            for (int i = 0; i < partitions.Length - 1; i++)
            {
                int from = range.From + partitions[i];
                int to = range.From + partitions[i + 1] - 1;
                if (!strs.IsValidRange(from, to))
                    continue;

                strs.SetRange(from, to);

                MSDStringSort(strs, alphabet, s + 1);
            }
        }

        public static string[] LSDStringSort(string[] strs, Alphabet alphabet)
        {
            int i = 0;
            int length = strs.Max(str => str.Length);

            Key<string> key = str => i < str.Length ? alphabet.Value(str[str.Length - 1 - i]) : 0;

            for (; i < length; i++)
                CountingSort(strs, key, 0, alphabet.Size - 1);

            return strs;
        }

        public static T[] CountingSort<T>(T[] items, Key<T> key, int from, int to)
        {
            int[] partitions = Partition(items, key, from, to);
            Distribute(items, partitions, key, from, to);

            return items;
        }

        public static int[] Count<T>(IList<T> items, Key<T> key, int from, int to)
        {
            int[] counts = new int[to - from + 1];

            for (int i = 0; i < items.Count; i++)
                counts[key(items[i]) - from]++;

            return counts;
        }

        public static int[] Partition<T>(int[] counts, Key<T> key, int from, int to, bool maintain)
        {
            if (maintain)
                counts = counts.ToArray();

            for (int i = 0, prev = 0, next = 0; i < counts.Length - 1; i++)
            {
                next = counts[i + 1];
                counts[i + 1] = counts[i] + prev;
                prev = next;
            }
            counts[0] = 0;

            return counts;
        }

        public static int[] Partition<T>(IList<T> items, Key<T> key, int from, int to)
        {
            int[] counts = Count(items, key, from, to);
            return Partition(counts, key, from, to, maintain: false);
        }

        public static void Distribute<T>(IList<T> items, int[] partitions, Key<T> key, int from, int to, bool maintain = false)
        {
            if (maintain)
                partitions = partitions.ToArray();

            var aux = new T[items.Count];
            for (int i = 0; i < items.Count; i++)
                aux[partitions[key(items[i]) - from]++] = items[i];

            for (int i = 0; i < items.Count; i++)
                items[i] = aux[i];
        }

        private static void QuickSort<T>(IList<T> items, int from, int to)
            where T : IComparable<T>
        {
            if (from >= to)
            {
                return;
            }

            T pivot = items[(from + to) / 2];

            int lesser = from - 1;
            int greater = to + 1;
            while (true)
            {
                do
                {
                    lesser++;
                } while (items[lesser].CompareTo(pivot) < 0);

                do
                {
                    greater--;
                } while (pivot.CompareTo(items[greater]) < 0);

                if (lesser >= greater)
                {
                    break;
                }

                items.Swap(lesser, greater);
            }

            QuickSort(items, greater + 1, to);
            QuickSort(items, from, greater);
        }

        private static void Merge<T>(IList<T> items, int first, int second, int length, IList<T> output)
            where T : IComparable<T>
        {
            int f = first;
            int s = second;
            for (int i = first; i < length; i++)
            {
                if (length <= s || (f < second && items[f].CompareTo(items[s]) <= 0))
                {
                    output[i] = items[f];
                    f++;
                }
                else
                {
                    output[i] = items[s];
                    s++;
                }
            }
        }

        private static int MaxBitLength(IList<int> numbers)
        {
            int max = int.MinValue;
            for (int i = 0; i < numbers.Count; i++)
            {
                if (numbers[i] < 0)
                {
                    max = numbers[i];
                    break;
                }
                else if (max < numbers[i])
                {
                    max = numbers[i];
                }
            }

            int length = 32;
            while (!GetBit(max, length - 1))
            {
                length--;
            }

            return length;
        }
    }
}