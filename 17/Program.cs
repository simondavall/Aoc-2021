using System.Diagnostics;
using System.Text.RegularExpressions;
using AocHelper;

namespace _17;

internal static class Program
{
    private const long ExpectedPartOne = 4186;
    private const long ExpectedPartTwo = 2709;
    private const string Day = "_17";

    public static int Main(string[] args)
    {
        var filename = "input_17.txt";
        if (args.Length > 1 && !string.IsNullOrWhiteSpace(args[1]))
            filename = args[1];

        var input = File.ReadAllText($"{filename}");

        const string pattern = @"(-?\d+)";
        var matches = Regex.Matches(input, pattern);
        if (matches.Count < 4)
            throw new UnreachableException("Oops, pattern did not match.");

        var x_min = matches[0].Value.ToInt();
        var x_max = matches[1].Value.ToInt();
        var y_min = matches[2].Value.ToInt();
        var y_max = matches[3].Value.ToInt();

        ((int min, int max) x, (int min, int max) y) targetArea = ((x_min, x_max), (y_min, y_max));
        
        var resultPartOne = PartOne(targetArea);
        Console.WriteLine($"Day{Day} Part 1: {resultPartOne}");
        var resultPartTwo = PartTwo(targetArea);
        Console.WriteLine($"Day{Day} Part 2: {resultPartTwo}");

        return resultPartOne == ExpectedPartOne && resultPartTwo == ExpectedPartTwo ? 0 : 1;
    }

    private static long PartOne(((int min, int max) x, (int min,int max) y) targetArea)
    {
        var stopwatch = Stopwatch.StartNew();
        
        var result = FindMaxY(targetArea);
        
        stopwatch.Stop();
        Console.WriteLine($"Time elapsed (ms): {(double)stopwatch.Elapsed.Ticks / 10_000}");

        return result.max_height;
    }

    private static long PartTwo(((int min, int max) x, (int min, int max) y) targetArea)
    {
        var stopwatch = Stopwatch.StartNew();
        
        var (min_y, max_y) = (targetArea.y.min, FindMaxY(targetArea).max_start_y);
        var (min_x, max_x) = (FindMinX(targetArea), targetArea.x.max);

        long tally = 0;
        
        for (var x = min_x; x <= max_x; x++)
            for (var y = min_y; y <= max_y; y++)
                if (HitsTarget(x, y, targetArea))
                    tally++;
        
        stopwatch.Stop();
        Console.WriteLine($"Time elapsed (ms): {(double)stopwatch.Elapsed.Ticks / 10_000}");

        return tally;
    }

    private static bool HitsTarget(int dx, int dy, ((int min, int max) x, (int min, int max) y) targetArea)
    {
        int x = 0, y = 0;
        for (; y >= targetArea.y.min && x <= targetArea.x.max; dy--, dx--)
        {
            if (y <= targetArea.y.max && x >= targetArea.x.min)
                return true;

            y += dy;
            x += Math.Max(0, dx);
        }
        
        return false;
    }
    
    private static (int max_height, int max_start_y) FindMaxY(((int min, int max) x, (int min,int max) y) targetArea)
    {
        var starting_y = 0;
        var max_height = 0;
        while (true)
        {
            var y = 0;
            var localMaxY = 0;

            for (var dy = starting_y++; y >= targetArea.y.min; dy--)
            {
                if (y < 0)
                    break;
                
                localMaxY = Math.Max(localMaxY, y);
                y += dy;
            }

            if (y < targetArea.y.min)
                break;
            
            if (localMaxY > max_height)
                max_height = localMaxY;
        }

        return (max_height, starting_y - 2);
    }

    private static int FindMinX(((int min, int max) x, (int min,int max) y) targetArea)
    {
        int x = 0, dx = 0;
        for (; x <= targetArea.x.min; dx++)
            x += dx;

        return dx - 1;
    }
}
