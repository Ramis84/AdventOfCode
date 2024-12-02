namespace AdventOfCode2024.Logic;

public static class Day1
{
    private static readonly string[] Lines = File.ReadAllLines("Inputs/input_20241201.txt");
    private static readonly (int, int)[] Pairs = Lines
        .Select(x =>
        {
            var segments = x.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var numbers = segments.Select(int.Parse).ToList();
            return (numbers[0], numbers[1]);
        }).ToArray();

    public static string GetPart1Answer()
    {
        var firstNumbersOrdered = Pairs.Select(x => x.Item1).Order().ToList();
        var secondNumbersOrdered = Pairs.Select(x => x.Item2).Order().ToList();
        var sum = 0;
        for (var i = 0; i < firstNumbersOrdered.Count; i++)
        {
            var firstNumber = firstNumbersOrdered[i];
            var secondNumber = secondNumbersOrdered[i];
            sum += (firstNumber > secondNumber) 
                ? firstNumber - secondNumber 
                : secondNumber - firstNumber;
        }

        return sum.ToString();
    }

    public static string GetPart2Answer()
    {
        var secondNumbersCount = Pairs
            .Select(x => x.Item2)
            .GroupBy(x => x)
            .ToDictionary(x => x.Key, x => x.Count());
        var similarityScores = Pairs
            .Select(x => x.Item1)
            .Select(first => first * secondNumbersCount.GetValueOrDefault(first));
        return similarityScores.Sum().ToString();
    }
}