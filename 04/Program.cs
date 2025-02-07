using System.Diagnostics;
using AocHelper;

namespace _04;

internal static class Program
{
    private const long ExpectedPartOne = 16716;
    private const long ExpectedPartTwo = 4880;
    private const string Day = "_04";

    public static int Main(string[] args)
    {
        var filename = "input_04.txt";
        if (args.Length > 1 && !string.IsNullOrWhiteSpace(args[1]))
            filename = args[1];

        var input = File.ReadAllText($"{filename}").Split("\n\n", StringSplitOptions.RemoveEmptyEntries);

        var numbers = input[0].Split(',', StringSplitOptions.RemoveEmptyEntries).ToIntArray();
        var boards = BuildBoards(input);
        
        var resultPartOne = PartOne(boards, numbers);
        Console.WriteLine($"Day{Day} Part 1: {resultPartOne}");
        boards = BuildBoards(input);
        var resultPartTwo = PartTwo(boards, numbers);
        Console.WriteLine($"Day{Day} Part 2: {resultPartTwo}");

        return resultPartOne == ExpectedPartOne && resultPartTwo == ExpectedPartTwo ? 0 : 1;
    }

    private static long PartOne(List<CardItem[][]> boards, int[] numbers)
    {
        var stopwatch = Stopwatch.StartNew();

        var index = 0;

        while (index < numbers.Length)
        {
            var current = numbers[index++];
            foreach (var cardItem in boards.SelectMany(x => x).SelectMany(x => x).Where(x => x.Number == current))
                cardItem.Seen = true;

            var winnerId = GetWinners(boards);
            if (winnerId.Count == 0) 
                continue;
            
            var unseenTotal = 0;
            foreach (var cardItem in boards[winnerId[0]].SelectMany(x => x).Where(x => !x.Seen))
                unseenTotal += cardItem.Number;
            
            stopwatch.Stop();
            Console.WriteLine($"Time elapsed (ms): {(double)stopwatch.Elapsed.Microseconds / 100}");
            
            return unseenTotal * current;
        }
        
        stopwatch.Stop();
        Console.WriteLine($"Time elapsed (ms): {(double)stopwatch.Elapsed.Microseconds / 100}");
        
        return -1;
    }

    private static long PartTwo(List<CardItem[][]> boards, int[] numbers)
    {
        var stopwatch = Stopwatch.StartNew();

        CardItem[][] lastWinner = null!;
        var index = 0;

        while (index < numbers.Length)
        {
            var current = numbers[index++];
            foreach (var cardItem in boards.SelectMany(x => x).SelectMany(x => x).Where(x => x.Number == current))
                cardItem.Seen = true;

            var winnerIds = GetWinners(boards);
            if (winnerIds.Count == 0)
                continue;
            
            foreach (var id in winnerIds.SortedDesc())
            {
                lastWinner = boards[id];
                boards.RemoveAt(id);
            }
            
            if (boards.Count > 0) 
                continue;
            
            var unseenTotal = 0;
            foreach (var cardItem in lastWinner.SelectMany(x => x).Where(x => !x.Seen))
                unseenTotal += cardItem.Number;
            
            stopwatch.Stop();
            Console.WriteLine($"Time elapsed (ms): {(double)stopwatch.Elapsed.Microseconds / 100}");
            
            return unseenTotal * current;
        }
        
        stopwatch.Stop();
        Console.WriteLine($"Time elapsed (ms): {(double)stopwatch.Elapsed.Microseconds / 100}");
        
        return -1;
    }

    private static List<int> GetWinners(List<CardItem[][]> boards)
    {
        List<int> winners = [];
        for (var index = 0; index < boards.Count; index++)
        {
            if(IsWinner(boards[index]))
                winners.Add(index);
        }
        
        return winners;
    }

    private static bool IsWinner(CardItem[][] board)
    {
        for (var i = 0; i < board.Length; i++)
        {
            if (board[i].All(x => x.Seen))
                return true; // row complete
            
            if (board.All(x => x[i].Seen))
                return true; // col complete
        }
        return false;
    }
    
    private static List<CardItem[][]> BuildBoards(string[] input)
    {
        List<CardItem[][]> boards = [];
        foreach (var rest in input.AsSpan()[1..])
        {
            var rows = rest.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            
            var board = new CardItem[rows.Length][];
            for (var i = 0; i < rows.Length; i++)
                board[i] = rows[i].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => new CardItem {Number = x.ToInt()}).ToArray();
            
            boards.Add(board);
        }

        return boards;
    }
    
    private class CardItem {
        public int Number { get; init; }
        public bool Seen { get; set; }
    }
}