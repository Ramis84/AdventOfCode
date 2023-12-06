namespace AdventOfCode2023.Logic;

public static class Day2
{
    private static readonly string[] Lines = File.ReadAllLines("Inputs/input_20231202.txt");
    private static readonly Game[] Games = Lines.Select(line => new Game(line)).ToArray();

    public static string GetPart1Answer()
    {
        var possibleGames = Games.Where(game =>
        {
            var maximumColorCounts = game.GetMaximumColorCounts();
            return maximumColorCounts.CountByColor.GetValueOrDefault("red") <= 12 &&
                   maximumColorCounts.CountByColor.GetValueOrDefault("green") <= 13 &&
                   maximumColorCounts.CountByColor.GetValueOrDefault("blue") <= 14;
        });
        var possibleGamesIdSum = possibleGames.Select(x => x.Id).Sum();
        return possibleGamesIdSum.ToString();
    }

    public static string GetPart2Answer()
    {
        var powers = Games.Select(x =>
        {
            var minimumPossibleCounts = x.GetMaximumColorCounts().CountByColor.Values;
            var power = minimumPossibleCounts.Aggregate(1, (currentPower, currentCount) => currentPower * currentCount);
            return power;
        });
        var powerSum = powers.Sum();
        return powerSum.ToString();
    }

    private class Game
    {
        public int Id { get; }
        public Subset[] Subsets { get; }

        public Game(string line) // Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
        {
            var segments = line.Split(": ");
            Id = int.Parse(segments[0].Split(' ')[1]); // Game 123
            var subsetStrings = segments[1].Split("; "); // 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
            Subsets = subsetStrings.Select(x => new Subset(x)).ToArray();
        }

        public Subset GetMaximumColorCounts()
        {
            var mergedSubsets = Subsets.Aggregate(Subset.EmptySubset,
                (mergedSubsets, currentSubset) => mergedSubsets.MergeWith(currentSubset));
            return mergedSubsets;
        }
    }

    private class Subset
    {
        public Dictionary<string, int> CountByColor { get; }

        public Subset(string subset) // 3 blue, 4 red
        {
            var countColorStrings = subset.Split(", ");
            CountByColor = countColorStrings
                .Select(x => x.Split(' ')) // 3 blue
                .ToDictionary(x => x[1], x => int.Parse(x[0]));
        }

        public static Subset EmptySubset => new(new Dictionary<string, int>());

        private Subset(Dictionary<string, int> countByColor)
        {
            CountByColor = countByColor;
        }

        public Subset MergeWith(Subset other)
        {
            var mergedDictionary = new Dictionary<string, int>(CountByColor);
            foreach (var colorAndCount in other.CountByColor)
            {
                mergedDictionary[colorAndCount.Key] =
                    mergedDictionary.TryGetValue(colorAndCount.Key, out var count)
                        ? int.Max(count, colorAndCount.Value)
                        : colorAndCount.Value;
            }
            return new Subset(mergedDictionary);
        }
    }
}