using System;

static class Search
{
    public static int LinearSearch<T>(
        T[] a, T target,
        Func<T, T, int> cmp,
        out int comparisons)
    {
        comparisons = 0;

        for (int i = 0; i < a.Length; i++)
        {
            comparisons++;

            if (cmp(a[i], target) == 0)
                return i;
        }

        return -1;
    }

    public static int BinarySearch<T>(
        T[] a, T target,
        Func<T, T, int> cmp,
        out int comparisons)
    {
        comparisons = 0;
        int lo = 0;
        int hi = a.Length - 1;

        while (lo <= hi)
        {
            int mid = lo + (hi - lo) / 2;
            comparisons++;

            int result = cmp(a[mid], target);

            if (result == 0)
                return mid;

            if (result < 0)
                lo = mid + 1;
            else
                hi = mid - 1;
        }

        return -1;
    }

    public static int LowerBound<T>(
        T[] a,
        Func<T, bool> condition,
        out int comparisons)
    {
        comparisons = 0;
        int lo = 0;
        int hi = a.Length;

        while (lo < hi)
        {
            int mid = lo + (hi - lo) / 2;
            comparisons++;

            if (condition(a[mid]))
                hi = mid;
            else
                lo = mid + 1;
        }

        return lo;
    }

    public static int InterpolationBinarySearch<T>(
        T[] a,
        T target,
        Func<T, T, int> cmp,
        Func<T, int> getKey,
        out int comparisons)
    {
        comparisons = 0;

        if (a.Length == 0)
            return -1;

        int lo = 0;
        int hi = a.Length - 1;

        int first = getKey(a[lo]);
        int last = getKey(a[hi]);
        int key = getKey(target);

        int pos;

        if (last == first)
            pos = lo + (hi - lo) / 2;
        else
        {
            double ratio = (double)(key - first) / (last - first);
            pos = lo + (int)((hi - lo) * ratio);
        }

        pos = Math.Max(lo, Math.Min(hi, pos));

        comparisons++;
        int result = cmp(a[pos], target);

        if (result == 0)
            return pos;

        if (result < 0)
            lo = pos + 1;
        else
            hi = pos - 1;

        while (lo <= hi)
        {
            int mid = lo + (hi - lo) / 2;
            comparisons++;

            result = cmp(a[mid], target);

            if (result == 0)
                return mid;

            if (result < 0)
                lo = mid + 1;
            else
                hi = mid - 1;
        }

        return -1;
    }
}
