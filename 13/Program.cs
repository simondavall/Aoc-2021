using System.Diagnostics;
using AocHelper;

namespace _13;

internal static class Program
{
    private const long ExpectedPartOne = 788;
    private const string ExpectedPartTwo = "KJBKEUBG";
    private const string Day = "_13";

    public static int Main(string[] args)
    {        
        var filename = "input_13.txt";
        if (args.Length > 1 && !string.IsNullOrWhiteSpace(args[1]))
            filename = args[1];

        var (left, right) = File.ReadAllText($"{filename}").Split("\n\n", StringSplitOptions.RemoveEmptyEntries).ToTuplePair();
        
        HashSet<(int, int)> dots = [];
        foreach (var line in left.Split('\n', StringSplitOptions.RemoveEmptyEntries))
        {
            var (x, y) = line.Split(',').ToTuplePair();
            dots.Add((x.ToInt(), y.ToInt()));
        }
        
        List<(char, int)> folds = [];
        foreach (var line in right.Split('\n', StringSplitOptions.RemoveEmptyEntries))
        {
            var (ch, value) = line[11..].Split('=').ToTuplePair();
            folds.Add((ch[0], value.ToInt()));
        }
        
        var resultPartOne = PartOne(dots, folds[0]);
        Console.WriteLine($"Day{Day} Part 1: {resultPartOne}");
        var resultPartTwo = PartTwo(dots, folds);
        Console.WriteLine($"Day{Day} Part 2: {resultPartTwo}");
        
        return resultPartOne == ExpectedPartOne && resultPartTwo == ExpectedPartTwo ? 0 : 1;
    }
    
    private static long PartOne(HashSet<(int x, int y)> dots, (char ch, int value) fold)
    {
        var stopwatch = Stopwatch.StartNew();
        
        var workingSet = new HashSet<(int x, int y)>(dots);

        var movedDots = fold.ch == 'x' ? Fold_X(workingSet, fold) : Fold_Y(workingSet, fold);
        
        foreach (var dot in movedDots)
            workingSet.Add(dot);
        
        stopwatch.Stop();
        Console.WriteLine($"Time elapsed (ms): {(double)stopwatch.Elapsed.Ticks / 10_000}");

        return workingSet.Count;
    }
    
    private static string PartTwo(HashSet<(int x, int y)> dots, List<(char ch, int value)> folds)
    {
        var stopwatch = Stopwatch.StartNew();

        foreach (var fold in folds)
        {
            var movedDots = fold.ch == 'x' ? Fold_X(dots, fold) : Fold_Y(dots, fold);
        
            foreach (var dot in movedDots)
                dots.Add(dot);
        }
        
        stopwatch.Stop();
        Console.WriteLine($"Time elapsed (ms): {(double)stopwatch.Elapsed.Ticks / 10_000}");

        PrintDots(dots);
        
        return "KJBKEUBG";
    }

    private static List<(int, int)> Fold_X(HashSet<(int x, int y)> dots, (char ch, int value) fold)
    {
        List<(int, int)> movedDots = [];
        foreach (var dot in dots.Where(dot => dot.x > fold.value))
        {
            dots.Remove(dot);
            movedDots.Add((dot.x - (dot.x - fold.value) * 2, dot.y));
        }
        return movedDots;
    }
    
    private static List<(int, int)> Fold_Y(HashSet<(int x, int y)> dots, (char ch, int value) fold)
    {
        List<(int, int)> movedDots = [];
        foreach (var dot in dots.Where(dot => dot.y > fold.value))
        {
            dots.Remove(dot);
            movedDots.Add((dot.x, dot.y - (dot.y - fold.value) * 2));
        }
        return movedDots;
    }

    private static void PrintDots(HashSet<(int x, int y)> dots)
    {
        (int min, int max) height = (int.MaxValue, int.MinValue);
        (int min, int max) width = (int.MaxValue, int.MinValue);
        foreach (var dot in dots)
        {
            if (dot.x > width.max)
                width.max = dot.x;
            if (dot.x < width.min)
                width.min = dot.x;
            if (dot.y > height.max)
                height.max = dot.y;
            if (dot.y < height.min)
                height.min = dot.y;
        }

        Console.WriteLine();
        for (var y = height.min; y <= height.max; y++)
        {
            for (var x = width.min; x <= width.max; x++)
            {
                if (dots.Contains((x, y)))
                {
                    Console.Write("###");
                    continue;
                }
                Console.Write("...");
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }
}
