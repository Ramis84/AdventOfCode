using MoreLinq;

namespace AdventOfCode2022.Logic.Assignments;

public static class Day1
{
    private static readonly string[] Lines = File.ReadAllLines("input_20221201.txt");

    private static readonly int[][] Elves = Lines
        .Segment(string.IsNullOrWhiteSpace)
        .Select(group => group.Skip(1).Select(int.Parse).ToArray())
        .ToArray();

    public static string GetPart1Answer()
    {
        var maxSum = Elves.Select(elf => elf.Sum()).Max();
        return maxSum.ToString();
    }

    public static string GetPart2Answer()
    {
        var top3Sum = Elves
            .Select(elf => elf.Sum())
            .OrderByDescending(x => x)
            .Take(3)
            .Sum();
        return top3Sum.ToString();
    }
}