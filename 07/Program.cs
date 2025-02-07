using System.Diagnostics;
using AocHelper;

namespace _07;

internal static class Program
{
    private const long ExpectedPartOne = 344605;
    private const long ExpectedPartTwo = 93699985;
    private const string Day = "_07";

    public static int Main(string[] args)
    {
        var filename = "input_07.txt";
        if (args.Length > 1 && !string.IsNullOrWhiteSpace(args[1]))
            filename = args[1];

        var input = File.ReadAllText($"{filename}").Split(',', StringSplitOptions.RemoveEmptyEntries).ToIntArray();
        
        var resultPartOne = PartOne(input.ToList().Sorted());
        Console.WriteLine($"Day{Day} Part 1: {resultPartOne}");
        var resultPartTwo = PartTwo(input);
        Console.WriteLine($"Day{Day} Part 2: {resultPartTwo}");

        return resultPartOne == ExpectedPartOne && resultPartTwo == ExpectedPartTwo ? 0 : 1;
    }

    private static long PartOne(List<int> crabs)
    {
        var stopwatch = Stopwatch.StartNew();
        
        var last = crabs.Count - 1;
        long tally = 0;
        
        while (true)
        {
            var median = crabs[crabs.Count / 2];
            if (crabs[0] == median && crabs[last] == median)
                break;
            
            tally += crabs[last] - median;
            crabs[last] = median;
            tally += median - crabs[0];
            crabs[0] = median;
            
            crabs.Sort();
        }
        
        stopwatch.Stop();
        Console.WriteLine($"Time elapsed (ms): {(double)stopwatch.Elapsed.Microseconds/100}");
        
        return tally;
    }

    private static long PartTwo(int[] crabs)
    {
        var stopwatch = Stopwatch.StartNew();
        
        var mean = (int)Math.Round((double)crabs.Sum() / crabs.Length);
        
        var current = CalcFuelConsumption(mean, crabs);
        var prev = CalcFuelConsumption(mean - 1, crabs);
        var next = CalcFuelConsumption(mean + 1, crabs);
        
        while (true)
        {
            if (current > prev)
            {
                next = current;
                current = prev;
                mean--;
                prev = CalcFuelConsumption(mean - 1, crabs);
                continue;
            }
            if (current > next)
            {
                prev = current;
                current = next;
                mean++;
                next = CalcFuelConsumption(mean + 1, crabs);
                continue;
            }
            
            break;
        }
        
        stopwatch.Stop();
        Console.WriteLine($"Time elapsed (ms): {(double)stopwatch.Elapsed.Microseconds/100}");
        
        return current;
    }
    
    private static long CalcFuelConsumption(int baseUnit, int[] crabs)
    {
        var fuel = 0;
        foreach (var crab in crabs)
        {
            var n = int.Abs(crab - baseUnit);
            fuel += (n + 1) * n / 2;
        }
        return fuel;
    }
}