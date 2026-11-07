using System;
using System.Diagnostics;

static class Measurements
{
    static Student[] GenerateStudents(int size, int seed)
    {
        Random random = new Random(seed);
        Student[] students = new Student[size];

        for (int i = 0; i < size; i++)
        {
            students[i] = new Student(
                "Student" + i,
                "КН-3/1",
                random.Next(0, 101)
            );
        }

        return students;
    }

    static double Measure(Action action)
    {
        action();
        double best = double.MaxValue;

        for (int i = 0; i < 3; i++)
        {
            Stopwatch sw = Stopwatch.StartNew();
            action();
            sw.Stop();

            if (sw.Elapsed.TotalMilliseconds < best)
                best = sw.Elapsed.TotalMilliseconds;
        }

        return best;
    }

    public static void Task5(int n, int size, Func<Student, Student, int> cmp)
    {
        int[] sizes = { 1000, 10000, 100000, 1000000 };

        Console.WriteLine("n | Linear | Binary | Ratio | Theory");

        foreach (int currentSize in sizes)
        {
            Student[] data = GenerateStudents(currentSize, n + currentSize);
            Array.Sort<Student>(
                data,
                (x, y) => cmp(x, y)
            );

            long linearTotal = 0;
            long binaryTotal = 0;

            Random random = new Random(n);

            for (int i = 0; i < 100; i++)
            {
                Student target = new Student(
                    "Q", "КН-3/1", random.Next(0, 102));

                Search.LinearSearch(data, target, cmp, out int lc);
                Search.BinarySearch(data, target, cmp, out int bc);

                linearTotal += lc;
                binaryTotal += bc;
            }

            double linear = linearTotal / 100.0;
            double binary = binaryTotal / 100.0;

            Console.WriteLine(
                $"{currentSize} | {linear:F2} | {binary:F2} | " +
                $"{linear / binary:F2} | {currentSize / 2.0:F0} / {Math.Log2(currentSize):F0}");
        }

        int[] queries = {
            1, 5, 10, 20, 30, 50, 100,
            200, 500, 1000, 2000, 5000, 10000,
            20000, 50000, 100000, 200000,
            500000, 1000000
        };

        Student[] original = GenerateStudents(size, n);
        Random queryRandom = new Random(n + 1);

        Student[] targets = new Student[100];

        for (int i = 0; i < targets.Length; i++)
        {
            targets[i] = new Student(
                "Q", "КН-3/1", queryRandom.Next(0, 102));
        }

        Student[] warmupSorted = (Student[])original.Clone();
        Array.Sort<Student>(
            warmupSorted,
            (x, y) => cmp(x, y)
        );

        for (int i = 0; i < 100; i++)
        {
            Search.LinearSearch(original, targets[i], cmp, out int c1);
            Search.BinarySearch(warmupSorted, targets[i], cmp, out int c2);
        }

        Console.WriteLine("\nq | Linear ms | Sort + Binary ms | Faster");

        int breakEven = -1;

        foreach (int q in queries)
        {
            double linear = Measure(() =>
            {
                for (int i = 0; i < q; i++)
                {
                    Search.LinearSearch(
                        original,
                        targets[i % targets.Length],
                        cmp,
                        out int c);
                }
            });

            double sorted = Measure(() =>
            {
                Student[] temp = (Student[])original.Clone();
                Array.Sort<Student>(
                    temp,
                    (x, y) => cmp(x, y)
                );

                for (int i = 0; i < q; i++)
                {
                    Search.BinarySearch(
                        temp,
                        targets[i % targets.Length],
                        cmp,
                        out int c);
                }
            });

            string faster = linear < sorted
                ? "Linear"
                : "Sort + Binary";

            Console.WriteLine(
                $"{q} | {linear:F4} | {sorted:F4} | {faster}");

            if (breakEven == -1 && sorted < linear)
                breakEven = q;
        }

        if (breakEven == -1)
            Console.WriteLine("\nBreak-even: not reached");
        else
            Console.WriteLine(
                $"\nBreak-even: approximately {breakEven} queries");
    }
}
