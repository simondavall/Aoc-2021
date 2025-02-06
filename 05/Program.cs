using System.Runtime.InteropServices;
using AocHelper;

namespace _05;

internal static class Program
{
    private const long ExpectedPartOne = 6283;
    private const long ExpectedPartTwo = 18864;
    private const string Day = "_05";

    public static int Main(string[] args)
    {
        var filename = "input_05.txt";
        if (args.Length > 1 && !string.IsNullOrWhiteSpace(args[1]))
            filename = args[1];

        var input = File.ReadAllText($"{filename}").Split('\n', StringSplitOptions.RemoveEmptyEntries);

        List<(int x1, int y1, int x2, int y2)> ranges = [];

        foreach (var line in input)
        {
            var (left, right) = line.Split(" -> ", StringSplitOptions.RemoveEmptyEntries).ToTuplePair();
            var (x1, y1) = left.Split(',').ToTuplePair();
            var (x2, y2) = right.Split(',').ToTuplePair();
            ranges.Add((x1.ToInt(), y1.ToInt(), x2.ToInt(), y2.ToInt()));
        }
        
        var resultPartOne = PartOne(ranges);
        Console.WriteLine($"Day{Day} Part 1: {resultPartOne}");
        var resultPartTwo = PartTwo(ranges);
        Console.WriteLine($"Day{Day} Part 2: {resultPartTwo}");

        return resultPartOne == ExpectedPartOne && resultPartTwo == ExpectedPartTwo ? 0 : 1;
    }

    private static readonly Dictionary<(int, int), int> HydroThermals = [];
    private static long PartOne(List<(int x1, int y1, int x2, int y2)> ranges)
    {
        foreach (var (x1, y1, x2, y2) in ranges)
        {
            if (x1 != x2 && y1 != y2)
                continue;
            
            for (var i = Math.Min(x1, x2); i <= Math.Max(x1, x2); i++)
                for (var j = Math.Min(y1, y2); j <= Math.Max(y1, y2); j++)
                    HydroThermals.AddOrIncrement((i, j));
        }
        
        return HydroThermals.Count(x => x.Value > 1);
    }
    
    private static long PartTwo(List<(int x1, int y1, int x2, int y2)> ranges)
    {
        foreach (var (x1, y1, x2, y2) in ranges)
        {
            if (x1 == x2 || y1 == y2)
                continue;

            var r = OrientateRange((x1, y1, x2, y2));
            
            var dx = (r.x2 - r.x1) / int.Abs(r.x2 - r.x1);
            var dy = (r.y2 - r.y1) / int.Abs(r.y2 - r.y1);

            var x = r.x1;
            var y = r.y1;
            
            while (x <= r.x2)
            {
                HydroThermals.AddOrIncrement((x, y));
                x += dx;
                y += dy;
            }
        }

        return HydroThermals.Count(x => x.Value > 1);
    }

    private static (int x1, int y1, int x2, int y2) OrientateRange((int x1, int y1, int x2, int y2) r)
    {
        return r.x2 < r.x1 ? (r.x2, r.y2, r.x1, r.y1) : r;
    }

    private static void AddOrIncrement(this Dictionary<(int, int), int> dict, (int, int)key)
    {
        ref var val = ref CollectionsMarshal.GetValueRefOrAddDefault(dict, key, out _);
        val++;
    }
}