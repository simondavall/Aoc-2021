using System.Diagnostics;
using AocHelper;

namespace _18;

internal static class Program
{
    private const long ExpectedPartOne = 4033;
    private const long ExpectedPartTwo = 4864;
    private const string Day = "_18";

    public static int Main(string[] args)
    {
        var filename = "input_18.txt";
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
        var stopwatch = Stopwatch.StartNew();
        
        var tree = BuildTree(input[0]);
        
        foreach (var line in input.Skip(1))
        {
            tree = tree.Add(BuildTree(line));
            Reduce(tree);
        }

        var tally = Magnitude(tree);
        
        stopwatch.Stop();
        Console.WriteLine($"Time elapsed (ms): {(double)stopwatch.Elapsed.Ticks / 10_000}");

        return tally;
    }

    private static long PartTwo(string[] input)
    {
        var stopwatch = Stopwatch.StartNew();
        
        long maxSum = 0;

        var trees = new Node[input.Length];
        for (var i = 0; i < input.Length; i++)
        {
            trees[i] = BuildTree(input[i]);
        }
        
        for (var i = 0; i < input.Length - 1; i++)
        {
            for (var j = i + 1; j < input.Length; j++)
            {
                var sum = GetAdditionMagnitude(trees[i], trees[j]);
                maxSum = Math.Max(maxSum, sum);
                
                sum = GetAdditionMagnitude(trees[j], trees[i]);
                maxSum = Math.Max(maxSum, sum);
            }
        }
        
        stopwatch.Stop();
        Console.WriteLine($"Time elapsed (ms): {(double)stopwatch.Elapsed.Ticks / 10_000}");

        return maxSum;
    }

    private static long GetAdditionMagnitude(Node first, Node second)
    {
        var tree = Clone(first).Add(Clone(second));
        Reduce(tree);
        return Magnitude(tree);
    }
    
    private static void Reduce(Node node)
    {
        while (Explode(node, 0) || Split(node)) ;
    }

    private static long Magnitude(Node node)
    {
        if (node is NullNode)
            return 0;

        var left = node.Left.Value == -1 ? Magnitude(node.Left) : node.Left.Value;
        var right = node.Right.Value == -1 ? Magnitude(node.Right) : node.Right.Value;
        
        return left * 3 + right * 2;
    }
    
    private static bool Explode(Node node, int depth)
    {
        if (node is NullNode)
            return false;
        
        if (Explode(node.Left, depth + 1))
            return true;
        
        if (node != node.Parent.Right && Explode(node.Parent.Right, depth))
            return true;

        if (depth <= 4) 
            return false;
        
        AddLeft(node.Parent.Left);
        AddRight(node.Parent.Right);
        node.Parent.Left = _nullNode;
        node.Parent.Right = _nullNode;
        node.Parent.Value = 0;
        return true;
    }

    private static readonly HashSet<Node> Seen = [];
    
    private static void AddLeft(Node node)
    {
        Seen.Clear();
        Seen.Add(node);
        var current = node;
        while (true)
        {
            if (current is { Parent: NullNode, Left: not NullNode } && Seen.Contains(current.Left))
                return;

            if (current.Parent.Left is NullNode || Seen.Contains(current.Parent.Left))
            {
                current = current.Parent;
                Seen.Add(current);
                continue;
            }

            current = current.Parent.Left;
            break;
        }

        while (current.Right is not NullNode)
            current = current.Right;

        current.Value += node.Value;
    }
    
    private static void AddRight(Node node)
    {
        Seen.Clear();
        Seen.Add(node);
        var current = node;
        while (true)
        {
            if (current is { Parent: NullNode, Right: not NullNode } && Seen.Contains(current.Right))
                return;

            if (current.Parent.Right is NullNode || Seen.Contains(current.Parent.Right))
            {
                current = current.Parent;
                Seen.Add(current);
                continue;
            }

            current = current.Parent.Right;
            break;
        }

        while (current.Left is not NullNode)
            current = current.Left;

        current.Value += node.Value;
    }
    
    private static bool Split(Node node)
    {
        if (node is NullNode)
            return false;
        
        if (node.Value > 9)
        {
            var value = node.Value / 2;
            node.Left = new Node { Value = value, Parent = node };
            node.Right = new Node { Value = node.Value - value, Parent = node };
            node.Value = -1;
            return true;
        }
        
        return Split(node.Left) || Split(node.Right);
    }
    
    private static Node Add(this Node node, Node other)
    {
        var newNode = new Node { Left = node, Right = other};
        newNode.Left.Parent = newNode;
        newNode.Right.Parent = newNode;
        return newNode;
    }
    
    private static Node BuildTree(string input)
    {
        if (input[0] != '[')
        {
            return new Node { Value = input.ToInt()};
        }

        var node = new Node();
        var depth = 0;
        
        for (var i = 0; i < input.Length; i++)
        {
            if (input[i] == '[')
                depth++;

            if (input[i] == ']')
                depth--;

            if (input[i] == ',' && depth == 1)
            {
                node = new Node { Left = BuildTree(input[1..i]), Right = BuildTree(input[(i + 1)..^1])};
                node.Left.Parent = node;
                node.Right.Parent = node;
            }
        }

        return node;
    }

    private static void PrintTree(Node[] nodes)
    {
        while (true)
        {
            if (nodes.Length == 0) return;

            List<Node> nextLevel = [];

            foreach (var node in nodes)
            {
                Console.Write(node.Value == -1 ? "  *  " : $" {node.Value} ");

                if (node.Left is not NullNode) nextLevel.Add(node.Left);
                if (node.Right is not NullNode) nextLevel.Add(node.Right);
            }

            Console.WriteLine();
            nodes = nextLevel.ToArray();
        }
    }

    private class Node
    {
        public Node Left { get; set; } = _nullNode;
        public Node Right { get; set; } = _nullNode;
        public int Value { get; set; } = -1;
        public Node Parent { get; set; } = _nullNode;
    }

    private static Node Clone(this Node node, Node? parent = null)
    {
        if (node is NullNode)
            return node;

        parent ??= _nullNode;
        
        var clone = new Node { Left = _nullNode, Right = _nullNode, Parent = parent, Value = node.Value };
        clone.Left = Clone(node.Left, clone);
        clone.Right = Clone(node.Right, clone);

        return clone;
    }

    private static NullNode _nullNode = new(){Left = new NullNode(), Right = new NullNode()};
    
    private class NullNode : Node
    {
    }
}