using System;

static class Broken
{
    public static int BrokenBinarySearch1<T>(
        T[] a, T target,
        Func<T, T, int> cmp,
        out int comparisons)
    {
        comparisons = 0;
        int lo = 0;
        int hi = a.Length - 1;

        while (lo < hi)
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

    public static int BrokenBinarySearch2<T>(
        T[] a, T target,
        Func<T, T, int> cmp,
        out int iterations)
    {
        iterations = 0;
        int lo = 0;
        int hi = a.Length - 1;

        while (lo <= hi && iterations < 20)
        {
            iterations++;
            int mid = lo + (hi - lo) / 2;

            int result = cmp(a[mid], target);

            if (result == 0)
                return mid;

            if (result < 0)
                lo = mid;
            else
                hi = mid - 1;
        }

        return -1;
    }

    public static int BrokenBinarySearch3<T>(
        T[] a, T target,
        Func<T, T, int> cmp,
        out int comparisons)
    {
        comparisons = 0;
        int lo = 0;
        int hi = a.Length - 1;

        while (lo <= hi)
        {
            int mid = (lo + hi) / 2;
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
}
