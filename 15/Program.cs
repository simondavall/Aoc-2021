using System.Diagnostics;

namespace _15;

internal static class Program
{
    private const long ExpectedPartOne = 769;
    private const long ExpectedPartTwo = 0;
    private const string Day = "_15";

    private static int _mapHeight;
    private static int _mapWidth;
    private static string[] _map = null!;
    private static readonly (int dr, int dc)[] Directions = [(-1, 0), (0, 1), (1, 0), (0, -1)];
    private static (int, int) _endPoint;
    
    public static int Main(string[] args)
    {
        var filename = "input_15.txt";
        if (args.Length > 1 && !string.IsNullOrWhiteSpace(args[1]))
            filename = args[1];

        _map = File.ReadAllText($"{filename}").Split('\n', StringSplitOptions.RemoveEmptyEntries);
        _mapHeight = _map.Length;
        _mapWidth = _map[0].Length;
        _endPoint = (_mapHeight - 1, _mapWidth - 1);
        
        Debug.Assert(_mapHeight == _mapWidth, "map not square");
        
        var resultPartOne = PartOne();
        Console.WriteLine($"Day{Day} Part 1: {resultPartOne}");
        var resultPartTwo = PartTwo();
        Console.WriteLine($"Day{Day} Part 2: {resultPartTwo}");

        return resultPartOne == ExpectedPartOne && resultPartTwo == ExpectedPartTwo ? 0 : 1;
    }

    private static long PartOne()
    {
        var stopwatch = Stopwatch.StartNew();
        
        const int mapSize = 1;
        var start = (0, 0);
        var tally = GetSafePath(start, mapSize);
        
        stopwatch.Stop();
        Console.WriteLine($"Time elapsed (ms): {(double)stopwatch.Elapsed.Ticks / 10_000}");

        return tally;
    }
    
    private static long PartTwo()
    {
        var stopwatch = Stopwatch.StartNew();
        
        const int mapSize = 5;
        var start = (0, 0);
        _endPoint = (_mapHeight * mapSize - 1, _mapWidth * mapSize - 1);
        var tally = GetSafePath(start, mapSize);
        
        stopwatch.Stop();
        Console.WriteLine($"Time elapsed (ms): {(double)stopwatch.Elapsed.Ticks / 10_000}");

        return tally;
    }
    
    private static long GetSafePath((int r, int c) start, int mapSize)
    {
        Dictionary<(int, int), long> visited = [];
        
        var q = new PriorityQueue<(int r, int c), int>();
        q.Enqueue(start, 0);

        while (q.Count > 0)
        {
            if (!q.TryDequeue(out var item, out var risk))
                continue;

            if (item == _endPoint)
                return risk;
            
            var (r, c) = item;

            foreach (var (dr, dc) in Directions)
            {
                var nr = r + dr;
                var nc = c + dc;
                
                if (OutOfBounds(nr, nc, mapSize))
                    continue;
                
                var dnr = nr / _mapHeight;
                var dnc = nc / _mapWidth;

                var mapPointRisk = _map[nr % _mapHeight][nc % _mapWidth] + dnr + dnc - '0';
                if (mapPointRisk > 9)
                    mapPointRisk -= 9;
                
                var nextRisk = risk + mapPointRisk;
                if (visited.ContainsKey((nr, nc)) && visited[(nr, nc)] <= nextRisk)
                    continue;

                visited[(nr, nc)] = nextRisk;
                q.Enqueue((nr, nc), nextRisk);
            }
        }

        return 0;
    }
    
    private static bool OutOfBounds(int r, int c, int factor)
    {
        return r < 0 || r >= _mapHeight * factor || c < 0 || c >= _mapWidth * factor;
    }
}