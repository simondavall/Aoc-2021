using System.Diagnostics;
using AocHelper;

namespace _10;

internal static class Program
{
    private const long ExpectedPartOne = 323613;
    private const long ExpectedPartTwo = 3103006161;
    private const string Day = "_10";

    public static int Main(string[] args)
    {
        var filename = "input_10.txt";
        if (args.Length > 1 && !string.IsNullOrWhiteSpace(args[1]))
            filename = args[1];

        var input = File.ReadAllText($"{filename}").Split('\n', StringSplitOptions.RemoveEmptyEntries);
        
        var resultPartOne = PartOne(input);
        Console.WriteLine($"Day{Day} Part 1: {resultPartOne}");
        var resultPartTwo = PartTwo();
        Console.WriteLine($"Day{Day} Part 2: {resultPartTwo}");

        return resultPartOne == ExpectedPartOne && resultPartTwo == ExpectedPartTwo ? 0 : 1;
    }

    private static readonly char[] ClosingTags = [')',']', '}', '>' ];
    private static readonly List<string> IncompleteLines = [];
    private static long PartOne(string[] input)
    {
        var stopwatch = Stopwatch.StartNew();

        List<char> invalidTags = [];
        
        long tally = 0;
        foreach (var line in input)
        {
            var s = new Stack<char>();
            var isCorrupted = false;
            foreach (var ch in line.ToCharArray())
            {
                if (ClosingTags.Contains(ch))
                {
                    if (!ValidOpeningTag(ch, s.Peek()))
                    {
                        invalidTags.Add(ch);
                        isCorrupted = true;
                        break;
                    }
                    s.Pop();
                    continue;
                }
                
                s.Push(ch);
            }
            if(!isCorrupted)
                IncompleteLines.Add(line);
        }

        tally += invalidTags.Count(x => x == ')') * 3;
        tally += invalidTags.Count(x => x == ']') * 57;
        tally += invalidTags.Count(x => x == '}') * 1197;
        tally += invalidTags.Count(x => x == '>') * 25137;
        
        stopwatch.Stop();
        Console.WriteLine($"Time elapsed (ms): {(double)stopwatch.Elapsed.Ticks / 10_000}");

        return tally;
    }

    private static long PartTwo()
    {
        var stopwatch = Stopwatch.StartNew();
        
        List<long> completionScore = [];
        foreach (var line in IncompleteLines)
        {
            var s = new Stack<char>();
            foreach (var ch in line.ToCharArray())
            {
                if (ClosingTags.Contains(ch))
                {
                    s.Pop();
                    continue;
                }
                s.Push(ch);
            }
            
            long score = 0;
            while(s.Count > 0)
            {
                score *= 5;
                var closingTag = MatchingTag(s.Pop());
                score += Array.IndexOf(ClosingTags, closingTag) + 1;
            }
            completionScore.Add(score);
        }

        var tally = completionScore.Sorted()[IncompleteLines.Count / 2];
        
        stopwatch.Stop();
        Console.WriteLine($"Time elapsed (ms): {(double)stopwatch.Elapsed.Ticks / 10_000}");

        return tally;
    }

    private static bool ValidOpeningTag(char closing, char lastTag)
    {
        return closing switch
        {
            ')' => lastTag == '(',
            ']' => lastTag == '[',
            '}' => lastTag == '{',
            '>' => lastTag == '<',
            _ => throw new UnreachableException($"Oops, not a known closing tag: {closing}")
        };
    }

    private static char MatchingTag(char tag)
    {
        return tag switch
        {
            '(' => ')',
            '[' => ']',
            '{' => '}',
            '<' => '>',
            _ => throw new UnreachableException($"Ooops, not a recognized opening tag:{tag}")
        };
    }
}