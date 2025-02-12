using System.Diagnostics;

namespace _11;

internal static class Program
{
    private const long ExpectedPartOne = 1585;
    private const long ExpectedPartTwo = 382;
    private const string Day = "_11";

    private static int _height;
    private static int _width;
    private static readonly (int dr, int dc)[] Directions = [(-1, 0), (-1, 1), (0, 1), (1, 1), (1, 0), (1, -1), (0, -1), (-1, -1)];
    
    public static int Main(string[] args)
    {
        var filename = "input_11.txt";
        if (args.Length > 1 && !string.IsNullOrWhiteSpace(args[1]))
            filename = args[1];

        var input = File.ReadAllText($"{filename}").Split('\n', StringSplitOptions.RemoveEmptyEntries);

        _height = input.Length;
        _width = input[0].Length;

        var octopus = BuildOctopusMap(input);
        var resultPartOne = PartOne(octopus);
        Console.WriteLine($"Day{Day} Part 1: {resultPartOne}");
        
        octopus = BuildOctopusMap(input);
        var resultPartTwo = PartTwo(octopus);
        Console.WriteLine($"Day{Day} Part 2: {resultPartTwo}");

        return resultPartOne == ExpectedPartOne && resultPartTwo == ExpectedPartTwo ? 0 : 1;
    }

    private static int[][] BuildOctopusMap(string[] input)
    {
        var octopus = new int[_height][];
        for (var i = 0; i < _height; i++)
            octopus[i] = input[i].ToCharArray().Select(x => x -'0').ToArray();
        return octopus;
    }
    
    private static long PartOne(int[][] octopus)
    {
        var stopwatch = Stopwatch.StartNew();

        long tally = 0;
        var counter = 0;
        while (counter++ < 100)
        {
            var flashed = Step(octopus);
            foreach (var (r, c) in flashed)
                octopus[r][c] = 0;
            
            tally += flashed.Count;
        }

        
        stopwatch.Stop();
        Console.WriteLine($"Time elapsed (ms): {(double)stopwatch.Elapsed.Ticks / 10_000}");

        return tally;
    }

    private static long PartTwo(int[][] octopus)
    {
        var stopwatch = Stopwatch.StartNew();
        
        var counter = 0;
        while (true)
        {
            counter++;

            var flashed = Step(octopus);
            if (flashed.Count == _height * _width)
                break;
            
            foreach (var (r, c) in flashed)
                octopus[r][c] = 0;
        }
        
        stopwatch.Stop();
        Console.WriteLine($"Time elapsed (ms): {(double)stopwatch.Elapsed.Ticks / 10_000}");

        return counter;
    }

    private static HashSet<(int, int)> Step(int[][] octopus)
    {
        Queue<(int, int)> q = [];

        for (var r = 0; r < _height; r++) {
            for (var c = 0; c < _width; c++) {
                if (++octopus[r][c] > 9)
                    q.Enqueue((r, c));
            }
        }

        HashSet<(int, int)> flashed = [];
        while (q.Count > 0)
        {
            var (r, c) = q.Dequeue();
            flashed.Add((r, c));
            foreach (var (dr, dc) in Directions)
            {
                var nr = r + dr;
                var nc = c + dc;
                if (OutOfBounds(nr, nc) || flashed.Contains((nr, nc)) || q.Contains((nr, nc))) 
                    continue;
                    
                if (++octopus[nr][nc] > 9)
                    q.Enqueue((nr, nc));
            }
        }

        return flashed;
    }
    
    private static bool OutOfBounds(int r, int c)
    {
        return r < 0 || r >= _height || c < 0 || c >= _width;
    }
}