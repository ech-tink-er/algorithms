using System;
using System.Collections.Generic;

public static class Searching
{
    public static int Search<T>(IReadOnlyList<T> items, T query)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].Equals(query))
            {
                return i;
            }
        }

        return -1;
    }

    // Find the leftmost location, where the query should be.
    public static int BinSearch<T>(IReadOnlyList<T> items, T query)
        where T : IComparable<T>
    {
        int left = 0;
        int right = items.Count;

        while (left != right)
        {
            int index = (left + right) / 2;

            if (query.CompareTo(items[index]) <= 0)
            {
                right = index;
            }
            else
            {
                left = index + 1;
            }
        }

        return left;
    }

    //Uses linear interpolation.
    public static int InterpolationSearch(IReadOnlyList<int> numbers, int query)
    {
        int left = 0;
        int right = numbers.Count - 1;

        while (left < right)
        {
            int from = query - numbers[left];
            int to = numbers[right] - numbers[left];
            int length = right - left;
            int index = left + (from * length / to);

            if (query == numbers[index])
            {
                return index;
            }
            else if (query < numbers[index])
            {
                right = index - 1;
            }
            else
            {
                left = index + 1;
            }
        }

        return -1;
    }
}