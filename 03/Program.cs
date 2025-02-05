using System.Globalization;

namespace _03;

internal static class Program
{
    private const long ExpectedPartOne = 3277364;
    private const long ExpectedPartTwo = 5736383;
    private const string Day = "_03";

    public static int Main(string[] args)
    {
        var filename = "input_03.txt";
        if (args.Length > 1 && !string.IsNullOrWhiteSpace(args[1]))
            filename = args[1];

        var input = File.ReadAllText($"{filename}").Split('\n', StringSplitOptions.RemoveEmptyEntries);

        var resultPartOne = PartOne(input);
        Console.WriteLine($"Day{Day} Part 1: {resultPartOne}");
        var resultPartTwo = PartTwo(input);
        Console.WriteLine($"Day{Day} Part 2: {resultPartTwo}");

        return resultPartOne == ExpectedPartOne && resultPartTwo == ExpectedPartTwo ? 0 : 1;
    }

    private static long PartOne(string[] input)
    {
        var digits = new int[input[0].Length];
        foreach (var line in input)
            for (var i = 0; i < input[0].Length; i++)
                digits[i] += line[i] - '0';

        int gamma = 0, epsilon = 0;
        var factor = 1;
        foreach (var digit in digits.Reverse())
        {
            if (digit > input.Length / 2)
                gamma += factor;
            else
                epsilon += factor;
            
            factor *= 2;
        }
        return gamma * epsilon;
    }

    private static long PartTwo(string[] input)
    {
        var oxy = GetRating(input.ToArray(), Criteria.Max);
        var co2 = GetRating(input.ToArray(), Criteria.Min);

        return oxy * co2;
    }

    private static int GetRating(string[] list, Criteria criteria, int index = 0)
    {
        var v = list.Select(x => x[index]).ToList();
        var q = criteria == Criteria.Max ? 
            v.Count(x => x == '1') >= v.Count(x => x == '0') ? '1' : '0' : 
            v.Count(x => x == '1') < v.Count(x => x == '0') ? '1' : '0';

        list = list.Where(x => x[index] == q).ToArray();
        return list.Length == 1 ? 
            int.Parse(list[0], NumberStyles.BinaryNumber) : 
            GetRating(list, criteria, index + 1);
    }

    private enum Criteria
    {
        Min,
        Max
    }
}