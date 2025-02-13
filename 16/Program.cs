using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;

namespace _16;

internal static class Program
{
    private const long ExpectedPartOne = 866;
    private const long ExpectedPartTwo = 1392637195518;
    private const string Day = "_16";
    
    private static long _versionSum;
    
    public static int Main(string[] args)
    {
        var filename = "input_16.txt";
        if (args.Length > 1 && !string.IsNullOrWhiteSpace(args[1]))
            filename = args[1];

        var input = File.ReadAllText($"{filename}").AsSpan();

        List<char> list = [];
        for (var i = 0; i < input.Length - 1; i++)
            list.AddRange(GetBinary(input[i]));

        var transmission = CollectionsMarshal.AsSpan(list);

        var resultPartOne = PartOne(transmission);
        Console.WriteLine($"Day{Day} Part 1: {resultPartOne}");
        var resultPartTwo = PartTwo(transmission);
        Console.WriteLine($"Day{Day} Part 2: {resultPartTwo}");

        return resultPartOne == ExpectedPartOne && resultPartTwo == ExpectedPartTwo ? 0 : 1;
    }

    private static long PartOne(ReadOnlySpan<char> transmission)
    {
        var stopwatch = Stopwatch.StartNew();
        
        DecodeTransmission(transmission);

        stopwatch.Stop();
        Console.WriteLine($"Time elapsed (ms): {(double)stopwatch.Elapsed.Ticks / 10_000}");

        return _versionSum;
    }
    
    private static long PartTwo(ReadOnlySpan<char> transmission)
    {
        var stopwatch = Stopwatch.StartNew();

        var result = DecodeTransmission(transmission);

        stopwatch.Stop();
        Console.WriteLine($"Time elapsed (ms): {(double)stopwatch.Elapsed.Ticks / 10_000}");

        return result.value;
    }
    
    private static (long value, int ptr) DecodeTransmission(ReadOnlySpan<char> transmission)
    {
        var ptr = 0;
        List<long> numbers;
        
        var version = int.Parse(transmission.Slice(ptr, 3), NumberStyles.BinaryNumber);
        _versionSum += version;
        ptr += 3;
        var type = int.Parse(transmission.Slice(ptr, 3), NumberStyles.BinaryNumber);
        ptr += 3;
        
        if (type == 4)
            return GetLiteral(ptr, transmission);
        
        var lengthType = transmission[ptr++];
        if (lengthType == '0')
            (numbers, ptr) = GetLengthTypeZero(ptr, transmission);
        else
            (numbers, ptr) = GetLengthTypeOne(ptr, transmission);

        return (ApplyType(type, numbers), ptr);
    }

    private static (long, int) GetLiteral(int ptr, ReadOnlySpan<char> transmission)
    {
        List<char> listChar = [];
        var stopBit = '1';
        while (stopBit == '1')
        {
            stopBit = transmission[ptr++];
            listChar.AddRange(transmission.Slice(ptr, 4));
            ptr += 4;
        }

        var literal = CollectionsMarshal.AsSpan(listChar);
        return (long.Parse(literal, NumberStyles.BinaryNumber), ptr);
    }
    
    private static (List<long> numbers, int ptr) GetLengthTypeZero(int ptr, ReadOnlySpan<char> transmission)
    {
        List<long> numbers = [];
        var length = long.Parse(transmission.Slice(ptr, 15), NumberStyles.BinaryNumber);
        ptr += 15;
        while (length > 0)
        {
            var result = DecodeTransmission(transmission[ptr..]);
            numbers.Add(result.value);
            length -= result.ptr;
            ptr += result.ptr;
        }
        
        return (numbers, ptr);
    }

    private static (List<long> numbers, int ptr) GetLengthTypeOne(int ptr, ReadOnlySpan<char> transmission)
    {
        List<long> numbers = [];
        var length = long.Parse(transmission.Slice(ptr, 11), NumberStyles.BinaryNumber);
        ptr += 11;
        for (var i = 0; i < length; i++)
        {
            var result = DecodeTransmission(transmission[ptr..]);
            numbers.Add(result.value);
            ptr += result.ptr;
        }
        return (numbers, ptr);
    }
    

    private static long ApplyType(int type, List<long> numbers)
    {
        return type switch
        {
            0 => numbers.Sum(),
            1 => numbers.Product(),
            2 => numbers.Min(),
            3 => numbers.Max(),
            5 => numbers[0] > numbers[1] ? 1 : 0,
            6 => numbers[0] < numbers[1] ? 1 : 0,
            7 => numbers[0] == numbers[1] ? 1 : 0,
            _ => throw new UnreachableException($"Ooops, unknown type being handled: {type}")
        };
    }
    
    private static long Product(this IEnumerable<long> list)
    {
        var value = 1L;
        foreach (var n in list)
            value *= n;

        return value;
    }
    
    private static char[] GetBinary(char hex)
    {
        return hex switch
        {
            '0' => ['0', '0', '0', '0'],
            '1' => ['0', '0', '0', '1'],
            '2' => ['0', '0', '1', '0'],
            '3' => ['0', '0', '1', '1'],
            '4' => ['0', '1', '0', '0'],
            '5' => ['0', '1', '0', '1'],
            '6' => ['0', '1', '1', '0'],
            '7' => ['0', '1', '1', '1'],
            '8' => ['1', '0', '0', '0'],
            '9' => ['1', '0', '0', '1'],
            'A' => ['1', '0', '1', '0'],
            'B' => ['1', '0', '1', '1'],
            'C' => ['1', '1', '0', '0'],
            'D' => ['1', '1', '0', '1'],
            'E' => ['1', '1', '1', '0'],
            'F' => ['1', '1', '1', '1'],
            _ => throw new UnreachableException($"Oops, unexpected hex value:{hex}")
        };
    }
}