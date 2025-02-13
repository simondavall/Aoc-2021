using System.Diagnostics;
using System.Runtime.InteropServices;
using AocHelper;

namespace _14;

internal static class Program
{
    private const long ExpectedPartOne = 2447;
    private const long ExpectedPartTwo = 3018019237563;
    private const string Day = "_14";

    public static int Main(string[] args)
    {
        var filename = "input_14.txt";
        if (args.Length > 1 && !string.IsNullOrWhiteSpace(args[1]))
            filename = args[1];

        var (left, right) = File.ReadAllText($"{filename}").Split("\n\n", StringSplitOptions.RemoveEmptyEntries).ToTuplePair();
        
        Dictionary<string, long> poly_pairs = [];
        for (var i = 0; i < left.Length - 1; i++)
        {
            var str = left[i..(i + 2)];
            poly_pairs.Add(str, 1);
        }
        
        Dictionary<string, (string, string)> ruleSet2 = [];
        foreach (var line in right.Split('\n', StringSplitOptions.RemoveEmptyEntries))
        {
            var key = line[..2];
            var ch = line[6];
            ruleSet2[key] = (new string([key[0], ch]), new string([ch, key[1]]));
        }
        
        var resultPartOne = PartOne(ruleSet2, poly_pairs);
        Console.WriteLine($"Day{Day} Part 1: {resultPartOne}");
        var resultPartTwo = PartTwo(ruleSet2, poly_pairs);
        Console.WriteLine($"Day{Day} Part 2: {resultPartTwo}");

        return resultPartOne == ExpectedPartOne && resultPartTwo == ExpectedPartTwo ? 0 : 1;
    }
    
    private static long PartOne(Dictionary<string, (string left, string right)> ruleSet, Dictionary<string, long> poly_pairs)
    {
        var stopwatch = Stopwatch.StartNew();
        
        var tally = GetResult(10, ruleSet, poly_pairs);

        stopwatch.Stop();
        Console.WriteLine($"Time elapsed (ms): {(double)stopwatch.Elapsed.Ticks / 10_000}");

        return tally;
    }
    
    private static long PartTwo(Dictionary<string, (string left, string right)> ruleSet, Dictionary<string, long> poly_pairs)
    {
        var stopwatch = Stopwatch.StartNew();
        
        var tally = GetResult(40, ruleSet, poly_pairs);
        
        stopwatch.Stop();
        Console.WriteLine($"Time elapsed (ms): {(double)stopwatch.Elapsed.Ticks / 10_000}");

        return tally;
    }
    
    private static long GetResult(int steps,  Dictionary<string, (string left, string right)> ruleSet, Dictionary<string, long> poly_pairs)
    {
        var counter = 0;
        while (counter++ < steps)
        {
            Dictionary<string, long> newPairs = [];
            foreach (var pair in poly_pairs)
            {
                var (left, right) = ruleSet[pair.Key];
                newPairs.AddOrIncrement(left, pair.Value);
                newPairs.AddOrIncrement(right, pair.Value);
            }

            poly_pairs = newPairs;
        }

        Dictionary<char, long> counters = [];
        foreach (var pair in poly_pairs)
        {
            counters.AddOrIncrement(pair.Key[0], pair.Value);
            counters.AddOrIncrement(pair.Key[1], pair.Value);
        }
        
        return (counters.Values.Max() + 1) / 2 - counters.Values.Min() / 2;
    }
    
    private static void AddOrIncrement<TKey>(this Dictionary<TKey, long> dict, TKey key, long value)
        where TKey : notnull
    {
        ref var val = ref CollectionsMarshal.GetValueRefOrAddDefault(dict, key, out var exists);
        if (exists)
        {
            val += value;
            return;
        }
        val = value;
    }
}