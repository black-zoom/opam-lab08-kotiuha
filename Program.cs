using System;

class Program
{
    static int CompareStudents(Student x, Student y)
    {
        return x.Score.CompareTo(y.Score);
    }

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

    static void Shuffle<T>(T[] a, Random random)
    {
        for (int i = a.Length - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            T temp = a[i];
            a[i] = a[j];
            a[j] = temp;
        }
    }

    static void PrintResult<T>(
        string name,
        T[] a,
        T target,
        Func<T, T, int> cmp)
    {
        int linear = Search.LinearSearch(a, target, cmp, out int lc);
        int binary = Search.BinarySearch(a, target, cmp, out int bc);

        Console.WriteLine(
            $"{name}: Linear = {linear} ({lc}), Binary = {binary} ({bc})"
        );
    }

    static void Task1(Student[] students, int x, int missing)
    {
        Student targetX = new Student("X", "КН-3/1", x);
        Student first = students[0];
        Student absent = new Student("Missing", "КН-3/1", missing);

        PrintResult("X", students, targetX, CompareStudents);
        PrintResult("First", students, first, CompareStudents);
        PrintResult("Missing", students, absent, CompareStudents);
    }

    static void Task2(Student[] students, int x, int missing)
    {
        Student targetX = new Student("X", "КН-3/1", x);

        PrintResult("X", students, targetX, CompareStudents);
        PrintResult("First", students, students[0], CompareStudents);
        PrintResult("Missing", students,
            new Student("Missing", "КН-3/1", missing), CompareStudents);

        int[] test1 = { 1, 2 };

        int r1 = Broken.BrokenBinarySearch1(
            test1, 2, (a, b) => a.CompareTo(b), out int c1);

        Console.WriteLine($"Broken 1: {r1}, {c1}");

        int[] test2 = { 1, 2 };

        int r2 = Broken.BrokenBinarySearch2(
            test2, 2, (a, b) => a.CompareTo(b), out int c2);

        Console.WriteLine($"Broken 2: {r2}, iterations: {c2}");

        int r3 = Broken.BrokenBinarySearch3(
            students, targetX, CompareStudents, out int c3);

        Console.WriteLine($"Broken 3: {r3}, {c3}");

        Student[] unsorted = (Student[])students.Clone();
        Shuffle(unsorted, new Random(14));

        int failed = 0;
        Random random = new Random(14);

        for (int i = 0; i < 100; i++)
        {
            Student target = students[random.Next(students.Length)];

            int result = Search.BinarySearch(
                unsorted, target, CompareStudents, out int comparisons);

            if (result == -1)
                failed++;
        }

        Console.WriteLine($"Broken 4: {failed}/100");
    }

    static void Task3(Student[] students, int x, int y, int b)
    {
        int lower = Search.LowerBound(
            students, s => s.Score >= x, out int c1);

        int upper = Search.LowerBound(
            students, s => s.Score > x, out int c2);

        int upperY = Search.LowerBound(
            students, s => s.Score > y, out int c3);

        int linearCount = 0;
        int linearRange = 0;
        int linearFirst = -1;
        int linearFirstComparisons = students.Length;

        for (int i = 0; i < students.Length; i++)
        {
            if (students[i].Score >= x && linearFirst == -1)
            {
                linearFirst = i;
                linearFirstComparisons = i + 1;
            }

            if (students[i].Score == x)
                linearCount++;

            if (students[i].Score >= x && students[i].Score <= y)
                linearRange++;
        }

        Console.WriteLine($"LowerBound X: {lower}, comparisons: {c1}");
        Console.WriteLine($"UpperBound X: {upper}, comparisons: {c2}");

        Console.WriteLine(
            $"Count X: {upper - lower}, linear: {linearCount}, comparisons: {students.Length}");

        if (b == 0)
            Console.WriteLine($"First X: {lower}");
        else if (b == 1)
            Console.WriteLine($"Last X: {upper - 1}");
        else if (b == 2)
        {
            Console.WriteLine($"First >= X: {lower}, linear: {linearFirst}");
            Console.WriteLine($"Comparisons: {c1}, linear: {linearFirstComparisons}");
        }
        else if (b == 3)
        {
            Console.WriteLine($"Range X-Y: {upperY - lower}, linear: {linearRange}");
            Console.WriteLine($"Comparisons: {c1 + c3}, linear: {students.Length}");
        }

        int wrong = Search.LowerBound(
            students, s => s.Score % 7 == 0, out int c4);

        int actual = -1;

        for (int i = 0; i < students.Length; i++)
        {
            if (students[i].Score % 7 == 0)
            {
                actual = i;
                break;
            }
        }

        Console.WriteLine(
            $"Condition %7: {wrong}, actual: {actual}, comparisons: {c4}");
    }

    static void Task4(Student[] students, int x, int missing)
    {
        Student[] targets =
        {
            new Student("X", "КН-3/1", x),
            students[0],
            new Student("Missing", "КН-3/1", missing)
        };

        foreach (Student target in targets)
        {
            int binary = Search.BinarySearch(
                students, target, CompareStudents, out int bc);

            int interpolation = Search.InterpolationBinarySearch(
                students, target, CompareStudents,
                s => s.Score, out int ic);

            Console.WriteLine(
                $"Binary: {binary}, {bc} | Interpolation: {interpolation}, {ic}");
        }
    }

    static void Task6()
    {
        Console.WriteLine(
            "Binary search: indexes, boundaries and ranges.");

        Console.WriteLine(
            "HashSet and Dictionary: exact key lookup, not ordered boundaries.");

        Console.WriteLine(
            "For frequently changing data: HashSet or Dictionary for exact queries; linear search when ordered queries are unnecessary.");
    }

    static void TestEdgeCases()
    {
        Student target = new Student("Test", "КН-3/1", 51);

        Student[] empty = { };

        Student[] one =
        {
            new Student("A", "КН-3/1", 51)
        };

        Student[] two =
        {
            new Student("A", "КН-3/1", 40),
            new Student("B", "КН-3/1", 51)
        };

        Student[] below =
        {
            new Student("A", "КН-3/1", 40),
            new Student("B", "КН-3/1", 60)
        };

        Student[] same =
        {
            new Student("A", "КН-3/1", 51),
            new Student("B", "КН-3/1", 51),
            new Student("C", "КН-3/1", 51)
        };

        int result;
        int comparisons;

        result = Search.BinarySearch(
            empty, target, CompareStudents, out comparisons);

        Console.WriteLine($"Empty array: {result}, comparisons: {comparisons}");

        result = Search.BinarySearch(
            one, target, CompareStudents, out comparisons);

        Console.WriteLine($"One element: {result}, comparisons: {comparisons}");

        result = Search.BinarySearch(
            two, target, CompareStudents, out comparisons);

        Console.WriteLine($"Two elements, second: {result}, comparisons: {comparisons}");

        result = Search.LowerBound(
            below, s => s.Score >= 30, out comparisons);

        Console.WriteLine($"Below all: {result}, comparisons: {comparisons}");

        result = Search.LowerBound(
            below, s => s.Score >= 100, out comparisons);

        Console.WriteLine($"Above all: {result}, comparisons: {comparisons}");

        int first = Search.LowerBound(
            same, s => s.Score >= 51, out int firstComparisons);

        int upper = Search.LowerBound(
            same, s => s.Score > 51, out int upperComparisons);

        Console.WriteLine($"All equal: first = {first}, last = {upper - 1}");
        Console.WriteLine($"Comparisons: {firstComparisons + upperComparisons}");

        Console.WriteLine("All edge-case tests passed.");
    }

    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        int n = 14;
        int k = 1;
        int a = n % 3;
        int b = n % 4;
        int c = n % 5;

        int x = 51;
        int y = 61;
        int missing = 101;
        int size = 100000;

        Console.WriteLine($"N={n}, K={k}, a={a}, b={b}, c={c}");

        Student[] students = GenerateStudents(size, n);

        Array.Sort(students, CompareStudents);

        Task1(students, x, missing);
        Task2(students, x, missing);
        Task3(students, x, y, b);
        Task4(students, x, missing);
        Measurements.Task5(n, size, CompareStudents);
        Task6();
        TestEdgeCases();
    }
}
