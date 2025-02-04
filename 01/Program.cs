namespace _01;

internal static class Program
{
    private const long ExpectedPartOne = 0;
    private const long ExpectedPartTwo = 0;
    private const string Day = "_01";

    public static int Main(string[] args)
    {        
        var filename = "sample.txt";
        if (args.Length > 1 && !string.IsNullOrWhiteSpace(args[1]))
            filename = args[1];

        var input = File.ReadAllText($"{filename}").Split('\n', StringSplitOptions.RemoveEmptyEntries);



        var resultPartOne = PartOne();
        Console.WriteLine($"Day{Day} Part 1: {resultPartOne}");
        var resultPartTwo = PartTwo();
        Console.WriteLine($"Day{Day} Part 2: {resultPartTwo}");
        
        return resultPartOne == ExpectedPartOne && resultPartTwo == ExpectedPartTwo ? 0 : 1;
    }

    private static long PartOne()
    {
        long tally = 0;
        return tally;
    }
    
    private static long PartTwo()
    {
        long tally = 0;
        return tally;
    }
}
