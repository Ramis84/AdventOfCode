using MoreLinq;

namespace AdventOfCode2022.Logic;

public static class Day5
{
    private static readonly string[] Lines = File.ReadAllLines("Inputs/input_20221205.txt");

    public static string GetPart1Answer()
    {
        var segments = Lines.Segment(string.IsNullOrWhiteSpace).ToArray();
        var initialStateLines = segments[0].ToArray();
        var moveLines = segments[1].Skip(1);

        var stacks = new CrateStacks(initialStateLines);
        var moves = moveLines.Select(line => new Move(line));

        foreach (var move in moves)
        {
            stacks.ExecuteMove(move);
        }

        var topItems = new string(stacks.Stacks.Select(x => x.Peek()).ToArray());
        return topItems;
    }

    public static string GetPart2Answer()
    {
        var segments = Lines.Segment(string.IsNullOrWhiteSpace).ToArray();
        var initialStateLines = segments[0].ToArray();
        var moveLines = segments[1].Skip(1);

        var stacks = new CrateStacks(initialStateLines);
        var moves = moveLines.Select(line => new Move(line));

        foreach (var move in moves)
        {
            stacks.ExecuteStackMove(move);
        }

        var topItems = new string(stacks.Stacks.Select(x => x.Peek()).ToArray());
        return topItems;
    }

    public class CrateStacks
    {
        public List<Stack<char>> Stacks { get; private set; } = new();

        public CrateStacks(string[] initialStateLines)
        {
            for (int columnIndex = 1; columnIndex < initialStateLines[0].Length; columnIndex += 4)
            {
                var stack = new Stack<char>();
                for (int rowIndex = initialStateLines.Length - 2; rowIndex >= 0; rowIndex--)
                {
                    var crate = initialStateLines[rowIndex][columnIndex];
                    if (crate == ' ') break;
                    stack.Push(crate);
                }
                Stacks.Add(stack);
            }
        }

        public void ExecuteMove(Move move)
        {
            var fromStack = Stacks[move.FromNumber - 1];
            var toStack = Stacks[move.ToNumber - 1];
            for (int i = 0; i < move.Count; i++)
            {
                var crate = fromStack.Pop();
                toStack.Push(crate);
            }
        }

        public void ExecuteStackMove(Move move)
        {
            var fromStack = Stacks[move.FromNumber - 1];
            var toStack = Stacks[move.ToNumber - 1];
            var crates = new Stack<char>();
            for (int i = 0; i < move.Count; i++)
            {
                var crate = fromStack.Pop();
                crates.Push(crate);
            }
            for (int i = 0; i < move.Count; i++)
            {
                var crate = crates.Pop();
                toStack.Push(crate);
            }
        }
    }

    public class Move
    {
        public int FromNumber { get; private set; }
        public int ToNumber { get; private set; }
        public int Count { get; private set; }

        public Move(string line)
        {
            var segments = line.Split(' ');
            Count = int.Parse(segments[1]);
            FromNumber = int.Parse(segments[3]);
            ToNumber = int.Parse(segments[5]);
        }
    }
}