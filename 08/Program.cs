using System.Diagnostics;
using AocHelper;

namespace _08;

internal static partial class Program
{
    private const long ExpectedPartOne = 476;
    private const long ExpectedPartTwo = 1011823;
    private const string Day = "_08";

    private static char _a = '\0';
    private static char _b = '\0';
    private static char _c = '\0';
    private static char _d = '\0';
    private static char _e = '\0';
    private static char _f = '\0';
    private static char _g = '\0';

    public static int Main(string[] args)
    {
        var filename = "input_08.txt";
        if (args.Length > 1 && !string.IsNullOrWhiteSpace(args[1]))
            filename = args[1];

        var input = File.ReadAllText($"{filename}").Split('\n', StringSplitOptions.RemoveEmptyEntries);
        
        var resultPartOne = PartOne(input);
        Console.WriteLine($"Day{Day} Part 1: {resultPartOne}");
        var resultPartTwo = PartTwo(input);
        Console.WriteLine($"Day{Day} Part 2: {resultPartTwo}");

        return resultPartOne == ExpectedPartOne && resultPartTwo == ExpectedPartTwo ? 0 : 1;
    }

    private static long PartOne(ReadOnlySpan<string> input)
    {
        var stopwatch = Stopwatch.StartNew();

        long tally = 0;
        
        List<(string, string)> signal = [];
        foreach (var line in input)
            signal.Add(line.Split(" | ", StringSplitOptions.RemoveEmptyEntries).ToTuplePair());
        
        int[] targetSignalLength = [2, 3, 4, 7];
        
        foreach (var (_, output) in signal) {
            foreach (var value in output.Split(' ', StringSplitOptions.RemoveEmptyEntries)) {
                if (targetSignalLength.Contains(value.Length))
                    tally++;
            }
        }
        
        stopwatch.Stop();
        Console.WriteLine($"Time elapsed (ms): {(double)stopwatch.Elapsed.Ticks / 10_000}");

        return tally;
    }

    private static long PartTwo(ReadOnlySpan<string> input)
    {
        var stopwatch = Stopwatch.StartNew();

        long tally = 0;

        List<(string, string)> signal = [];
        foreach (var line in input)
            signal.Add(line.Split(" | ", StringSplitOptions.RemoveEmptyEntries).ToTuplePair());
        
        foreach (var (inputValues, outputValues) in signal) {
            
            var allValues = inputValues.Split(' ').Select(x => x.InsertionSort())
                .Concat(outputValues.Split(' ').Select(x => x.InsertionSort()))
                .Distinct().ToArray();

            ResetChars();

            char[] cf = [];
            char[] bd = [];
            char[] eg = [];

            var two = GetValue(2, allValues);
            var three = GetValue(3, allValues);
            var four = GetValue(4, allValues);
            var seven = GetValue(7, allValues);

            var five = GetValues(5, allValues);
            var six = GetValues(6, allValues);
            
            if (two.Length == 2)
                cf = two.ToCharArray();
            
            if (cf.Length != 0 && three.Length == 3)
            {
                _a = three.Except(cf).Single();
            }
            
            if (cf.Length != 0 && four.Length == 4)
            {
                bd = four.Except(cf).ToArray();
            }
            
            if (_a == '\0' || bd.Length == 0)
            {
                if (three.Length == 3 && four.Length == 4)
                {
                    _a =  three.Except(four).Single();
                    bd = four.Except(three).ToArray();
                }
            }
            
            if (three.Length == 3 && four.Length == 4 && seven.Length == 7)
            {
                eg = seven.Except(three.Concat(four)).ToArray();
            }

            if (three.Length == 3 && four.Length == 4 && six.Length > 0)
            {
                foreach (var x in six.Select(x => x.Except(three.Concat(four)).ToArray()))
                {
                    if (x.Length == 1)
                        _g = x[0];
                    else
                        eg = x;
                }
                
                if (_g != '\0' && eg.Length > 0)
                    _e = eg.Except([_g]).First();
            }
            
            if (three.Length == 3 && four.Length == 4 && five.Length > 0 && _g == '\0')
            {
                foreach (var x in five.Select(x => x.Except(three.Concat(four)).ToArray()))
                {
                    if (x.Length == 1)
                        _g = x[0];
                    else
                        eg = x;
                }
                
                if (_g != '\0' && eg.Length > 0)
                    _e = eg.Except([_g]).First();
            }
            
            if (five.Length > 1 && _e != '\0')
            {
                for (var i = 0; i < five.Length - 1; i++)
                {
                    for (var j = i + 1; j < five.Length; j++)
                    {
                        var ch = five[i].Except(five[j]).ToArray();
                        if (ch.Length == 2 && ch.Contains(_e))
                            _c = ch.Except([_e]).First();
                        
                        ch = five[j].Except(five[i]).ToArray();
                        if (ch.Length == 2 && ch.Contains(_e))
                            _c = ch.Except([_e]).First();
                    }
                }
            }
            
            if (_c != '\0' && cf.Length > 0)
                _f = cf.Except([_c]).First();

            if (_c != '\0' && _e != '\0')
            {
                foreach (var str in six)
                {
                    var ch = seven.Except(str).Single();
                    if (ch == _c || ch == _e) 
                        continue;
                    
                    _d = ch;
                    break;
                }
            }
            
            if (_d != '\0' && bd.Length > 0)
                _b = bd.Except([_d]).First();
            
            if (!IsComplete())
            {
                throw new UnreachableException("Ooops. Not resolved all cases");
            }
            
            var translator = BuildTranslation();
            
            var number = 0;
            foreach (var value in outputValues.Split(' ').Select(x => x.InsertionSort()))
            {
                number *= 10;
                number += translator[value];
            }
            
            tally += number;
        }
        
        stopwatch.Stop();
        Console.WriteLine($"Time elapsed (ms): {(double)stopwatch.Elapsed.Ticks / 10_000}");

        return tally;
    }
    
    private static string GetValue(int length, string[] values)
    {
        return values.SingleOrDefault(x => x.Length == length) ?? string.Empty;
    }
    
    private static string[] GetValues(int length, string[] values)
    {
        return values.Where(x => x.Length == length).ToArray();
    }

    private static Dictionary<string, int> BuildTranslation()
    {
        Dictionary<string, int> translator = [];
        translator.Add(new string([_a, _b, _c, _e, _f, _g]).InsertionSort(), 0);
        translator.Add(new string([_c, _f]).InsertionSort(), 1);
        translator.Add(new string([_a, _c, _d, _e, _g]).InsertionSort(), 2);
        translator.Add(new string([_a, _c, _d, _f, _g]).InsertionSort(), 3);
        translator.Add(new string([_b, _c, _d, _f]).InsertionSort(), 4);
        translator.Add(new string([_a, _b, _d, _f, _g]).InsertionSort(), 5);
        translator.Add(new string([_a, _b, _d, _e, _f, _g]).InsertionSort(), 6);
        translator.Add(new string([_a, _c, _f]).InsertionSort(), 7);
        translator.Add(new string([_a, _b, _c, _d, _e, _f, _g]).InsertionSort(), 8);
        translator.Add(new string([_a, _b, _c, _d, _f, _g]).InsertionSort(), 9);

        return translator;
    }

    private static bool IsComplete()
    {
        char[] chars = [_a, _b, _c, _d, _e, _f];
        return !chars.Contains('\0');
    }
    
    private static void ResetChars()
    {
        _a = '\0';
        _b = '\0';
        _c = '\0';
        _d = '\0';
        _e = '\0';
        _f = '\0';
        _g = '\0';
    }
    
}