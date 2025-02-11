using System.Diagnostics;
using AocHelper;

namespace _09;

internal static class Program
{
    private const long ExpectedPartOne = 631;
    private const long ExpectedPartTwo = 821560;
    private const string Day = "_09";

    private static readonly (int dr, int dc)[] Directions = [(-1, 0), (0, 1), (1, 0), (0, -1)];
    private static int _mapHeight;
    private static int _mapWidth;
    private static readonly Dictionary<(int, int), int> LowPoints = [];
    private static readonly HashSet<(int, int)> Seen = [];
    
    public static int Main(string[] args)
    {
        var filename = "input_09.txt";
        if (args.Length > 1 && !string.IsNullOrWhiteSpace(args[1]))
            filename = args[1];

        var input = File.ReadAllText($"{filename}").Split('\n', StringSplitOptions.RemoveEmptyEntries);

        _mapHeight = input.Length;
        _mapWidth = input[0].Length;

        var resultPartOne = PartOne(input);
        Console.WriteLine($"Day{Day} Part 1: {resultPartOne}");
        var resultPartTwo = PartTwo(input);
        Console.WriteLine($"Day{Day} Part 2: {resultPartTwo}");

        return resultPartOne == ExpectedPartOne && resultPartTwo == ExpectedPartTwo ? 0 : 1;
    }
    
    private static long PartOne(string[] input)
    {
        var stopwatch = Stopwatch.StartNew();
        long tally = 0;
        
        for (var r = 0; r < _mapHeight; r++)
        {
            for (var c = 0; c < _mapWidth; c++)
            {
                var n = input[r][c];
                var isLowPoint = true;
                foreach (var (dr, dc) in Directions)
                {
                    var nr = r + dr;
                    var nc = c + dc;
                    if (OutOfBounds(nr, nc) || input[nr][nc] > n )
                        continue;
                    
                    isLowPoint = false;
                    break;
                }

                if (isLowPoint)
                {
                    LowPoints[(r, c)] = n;
                    tally += n + 1 - '0';
                }
            }
        }
        
        stopwatch.Stop();
        Console.WriteLine($"Time elapsed (ms): {(double)stopwatch.Elapsed.Ticks / 10_000}");

        return tally;
    }

    private static long PartTwo(string[] input)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        List<long> basins = [];
        foreach (var ((r, c), _) in LowPoints)
        {
            var q = new Queue<(int, int)>();
            q.Enqueue((r, c));
            Seen.Add((r, c));
            
            var currentBasinSize = 0;
            while (q.Count > 0)
            {
                var (cr, cc) = q.Dequeue();
                currentBasinSize++;

                foreach (var (dr, dc) in Directions)
                {
                    var nr = cr + dr;
                    var nc = cc + dc;
                    if (OutOfBounds(nr, nc) || Seen.Contains((nr, nc)) || input[nr][nc] == '9') 
                        continue;
                    
                    q.Enqueue((nr, nc));
                    Seen.Add((nr, nc));
                }
            }
            basins.Add(currentBasinSize);
        }
        
        var tally = basins.SortedDesc().Take(3).Product();
        
        stopwatch.Stop();
        Console.WriteLine($"Time elapsed (ms): {(double)stopwatch.Elapsed.Ticks / 10_000}");

        return tally;
    }

    private static bool OutOfBounds(int r, int c)
    {
        return r < 0 || r >= _mapHeight || c < 0 || c >= _mapWidth;
    }

    private static long Product(this IEnumerable<long> list)
    {
        var result = 1L;
        foreach (var n in list)
            result *= n;

        return result;
    }
}