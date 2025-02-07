using System.Diagnostics;
using AocHelper;
namespace _02;

internal static class Program
{
    private const long ExpectedPartOne = 1427868;
    private const long ExpectedPartTwo = 1568138742;
    private const string Day = "_02";

    private static readonly Dictionary<string, (int dx, int dy)> Directions = new() {{"forward", (1, 0)}, {"up", (0, -1)}, { "down", (0, 1)}};

    public static int Main(string[] args)
    {        
        var filename = "input_02.txt";
        if (args.Length > 1 && !string.IsNullOrWhiteSpace(args[1]))
            filename = args[1];

        var input = File.ReadAllText($"{filename}").Split('\n', StringSplitOptions.RemoveEmptyEntries).AsSpan();

        var course = new (string dir, int multiplier) [input.Length];
        for (var i = 0; i < input.Length; i++)
        {
            var line = input[i];
            var (dir, size) = line.Split(' ').ToTuplePair();
            course[i] = (dir, size.ToInt());
        }

        var resultPartOne = PartOne(course);
        Console.WriteLine($"Day{Day} Part 1: {resultPartOne}");
        var resultPartTwo = PartTwo(course);
        Console.WriteLine($"Day{Day} Part 2: {resultPartTwo}");
        
        return resultPartOne == ExpectedPartOne && resultPartTwo == ExpectedPartTwo ? 0 : 1;
    }

    private static long PartOne(ReadOnlySpan<(string dir, int multiplier)> course)
    {
        var stopwatch = Stopwatch.StartNew();
        
        (int x, int y) position = (0, 0);
        foreach (var instruction in course){
            var (dx, dy) = Directions[instruction.dir];
            position.x += dx * instruction.multiplier;
            position.y += dy * instruction.multiplier;
        }

        stopwatch.Stop();
        Console.WriteLine($"Time elapsed (ms): {(double)stopwatch.Elapsed.Microseconds / 100}");
        
        return position.x * position.y;
    }
    
    private static long PartTwo(ReadOnlySpan<(string dir, int multiplier)> course)
    {
        var stopwatch = Stopwatch.StartNew();
        
        (int x, int y) position = (0, 0);
        var aim = 0;
        foreach (var instruction in course){
            var (dx, dy) = Directions[instruction.dir];
            
            position.x += dx * instruction.multiplier;
            if (dx == 1)
                position.y += instruction.multiplier * aim;
            else{
                aim += dy * instruction.multiplier;
            }
        }

        stopwatch.Stop();
        Console.WriteLine($"Time elapsed (ms): {(double)stopwatch.Elapsed.Microseconds / 100}");
        
        return position.x * position.y;
    }
}
