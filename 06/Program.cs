using System.Diagnostics;
using AocHelper;

namespace _06;

internal static class Program
{
    private const long ExpectedPartOne = 379114;
    private const long ExpectedPartTwo = 1702631502303;
    private const string Day = "_06";

    public static int Main(string[] args)
    {
        var filename = "input_06.txt";
        if (args.Length > 1 && !string.IsNullOrWhiteSpace(args[1]))
            filename = args[1];

        var input = File.ReadAllText($"{filename}").Split(',', StringSplitOptions.RemoveEmptyEntries).ToIntArray();

        var resultPartOne = PartOne(input);
        Console.WriteLine($"Day{Day} Part 1: {resultPartOne}");
        var resultPartTwo = PartTwo(input);
        Console.WriteLine($"Day{Day} Part 2: {resultPartTwo}");

        return resultPartOne == ExpectedPartOne && resultPartTwo == ExpectedPartTwo ? 0 : 1;
    }

    private static long PartOne(int[] fish)
    {
        var stopwatch = Stopwatch.StartNew();

        var tally = GetNumberOfFishAfterDays(80, fish);
        
        stopwatch.Stop();
        Console.WriteLine($"Time elapsed (ms): {(double)stopwatch.Elapsed.Microseconds/100}");
        
        return tally;
    }

    private static long PartTwo(int[] fish)
    {
        var stopwatch = Stopwatch.StartNew();

        var tally = GetNumberOfFishAfterDays(256, fish);
        
        stopwatch.Stop();
        Console.WriteLine($"Time elapsed (ms): {(double)stopwatch.Elapsed.Microseconds/100}");
        
        return tally;
    }

    private static long GetNumberOfFishAfterDays(int days, int[] fish)
    {
        var lanternFish = fish.ToList();

        var state = new Dictionary<int, long> {{0,0},{1,0},{2,0},{3,0},{4,0},{5,0},{6,0},{7,0},{8,0}};
        foreach (var i in lanternFish)
            state[i] += 1;

        var counter = 0;
        while (counter++ < days)
        {
            if (counter % 7 != 0) 
                continue;
            
            state = UpdateStateBlock(state);
        }

        var remaining = --counter % 7;
        for (var i = 0; i < remaining; i++)
            state = UpdateStateUnit(state);

        long tally = 0;
        foreach (var (_, value) in state)
            tally += value;
        
        return tally;
    }
    
    private static Dictionary<int, long> UpdateStateBlock(Dictionary<int, long> state)
    {
        var updatedState = state.ToDictionary();
        foreach (var (key, value) in state)
        {
            switch (key)
            {
                case 0 or 1 or 2 or 3 or 4 or 5 or 6:
                    updatedState[key + 2] += value;
                    break;
                case 7:
                    updatedState[0] += value;
                    updatedState[7] -= value;
                    break;
                case 8:
                    updatedState[1] += value;
                    updatedState[8] -= value;
                    break;
                        
                default:
                    throw new Exception($"Unexpected state key: {key}");
            }
        }
        
        return updatedState;
    }
    
    private static Dictionary<int, long> UpdateStateUnit(Dictionary<int, long> state)
    {
        var updatedState = state.ToDictionary();
        foreach (var (key, value) in state)
        {
            switch (key)
            {
                case 0:
                    updatedState[8] += value;
                    updatedState[6] += value;
                    updatedState[0] -= value;
                    break;
                case 1 or 2 or 3 or 4 or 5 or 6 or 7 or 8:
                    updatedState[key - 1] += value;
                    updatedState[key] -= value;
                    break;
                        
                default:
                    throw new Exception($"Unexpected state key: {key}");
            }
        }
        
        return updatedState;
    }
    
}