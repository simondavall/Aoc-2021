using AocHelper;
namespace _01;

internal static class Program
{
    private const long ExpectedPartOne = 1446;
    private const long ExpectedPartTwo = 1486;
    private const string Day = "_01";

    public static int Main(string[] args)
    {        
        var filename = "input_01.txt";
        if (args.Length > 1 && !string.IsNullOrWhiteSpace(args[1]))
            filename = args[1];

        var input = File.ReadAllText($"{filename}").Split('\n', StringSplitOptions.RemoveEmptyEntries).ToIntArray();
        
        var resultPartOne = PartOne(input);
        Console.WriteLine($"Day{Day} Part 1: {resultPartOne}");
        var resultPartTwo = PartTwo(input);
        Console.WriteLine($"Day{Day} Part 2: {resultPartTwo}");
        
        return resultPartOne == ExpectedPartOne && resultPartTwo == ExpectedPartTwo ? 0 : 1;
    }

    private static long PartOne(int[] depths)
    {
        long tally = 0;
        for (var i = 0; i < depths.Length - 1; i++)
        {
          if (depths[i] < depths[i+1])
            tally++;
        }

        return tally;
    }
    
    private static long PartTwo(int[] depths)
    {
        long tally = 0;
        for (var i = 0; i < depths.Length - 3; i++)
        {
            var first = depths[i] + depths[i + 1] + depths[i + 2];
            var second = depths[i + 1] + depths[i + 2] + depths[i + 3];
            if (first < second)
                tally++;
        }

        return tally;
    }
}
