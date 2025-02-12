using System.Data;
using System.Diagnostics;
using AocHelper;

namespace _12;

internal static class Program
{
    private const long ExpectedPartOne = 4707;
    private const long ExpectedPartTwo = 130493;
    private const string Day = "_12";

    private static Dictionary<string, HashSet<string>> _connections = [];
    
    public static int Main(string[] args)
    {
        var filename = "input_12.txt";
        if (args.Length > 1 && !string.IsNullOrWhiteSpace(args[1]))
            filename = args[1];

        var input = File.ReadAllText($"{filename}").Split('\n', StringSplitOptions.RemoveEmptyEntries);
        
        var resultPartOne = PartOne(input);
        Console.WriteLine($"Day{Day} Part 1: {resultPartOne}");
        var resultPartTwo = PartTwo();
        Console.WriteLine($"Day{Day} Part 2: {resultPartTwo}");

        return resultPartOne == ExpectedPartOne && resultPartTwo == ExpectedPartTwo ? 0 : 1;
    }
    
    private static long PartOne(string[] map)
    {
        var stopwatch = Stopwatch.StartNew();
        
        _connections = GetConnections(map);
        var tally = GetPathsCount("start", ["start"]);
        
        stopwatch.Stop();
        Console.WriteLine($"Time elapsed (ms): {(double)stopwatch.Elapsed.Ticks / 10_000}");

        return tally;
    }

    private static long PartTwo()
    {
        var stopwatch = Stopwatch.StartNew();

        var tally = GetPathsCount2("start", ["start"], false);
        
        stopwatch.Stop();
        Console.WriteLine($"Time elapsed (ms): {(double)stopwatch.Elapsed.Ticks / 10_000}");

        return tally;
    }

    private static int GetPathsCount(string cave, HashSet<string> visited)
    {
        if (cave == "end")
            return 1;

        var total = 0;
        foreach (var next in _connections[cave])
        {
            if (visited.Contains(next))
                continue;

            if (next == next.ToLower())
                total += GetPathsCount(next, [..visited, next]);
            else
                total += GetPathsCount(next, visited);
        }

        return total;
    }
    
    private static int GetPathsCount2(string cave, HashSet<string> visited, bool is_doubled)
    {
        if (cave == "end")
            return 1;

        var total = 0;
        foreach (var next in _connections[cave])
        {
            if (next == "start")
                continue;
            if (visited.Contains(next) && is_doubled)
                continue;

            if (next == next.ToLower())
                if (visited.Contains(next))
                    total += GetPathsCount2(next, [..visited, next], true);
                else
                    total += GetPathsCount2(next, [..visited, next], is_doubled);
            else
                total += GetPathsCount2(next, visited, is_doubled);
        }

        return total;
    }
    
    private static Dictionary<string, HashSet<string>> GetConnections(string[] map)
    {
        Dictionary<string, HashSet<string>> connections = [];
        foreach (var line in map)
        {
            var (left, right) = line.Split('-').ToTuplePair();
            if (!connections.TryAdd(left, [right]))
                connections[left].Add(right);
            if (!connections.TryAdd(right, [left]))
                connections[right].Add(left);
        }

        return connections;
    }
}