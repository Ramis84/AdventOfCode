using MoreLinq;

namespace AdventOfCode2023.Logic;

public static class Day9
{
    private static readonly string[] Lines = File.ReadAllLines("Inputs/input_20231209.txt");
    private static readonly int[][] Histories = Lines
        .Select(line => line
            .Split(' ')
            .Select(int.Parse)
            .ToArray())
        .ToArray();

    public static string GetPart1Answer()
    {
        var sum = 0;
        
        foreach (var history in Histories)
        {
            var seriesDifferences = GetSeriesDifferences(history);

            for (var seriesIndex = seriesDifferences.Count-1; seriesIndex > 0; seriesIndex--)
            {
                var previousSeries = seriesDifferences[seriesIndex-1];
                previousSeries.Add(previousSeries[^1] + seriesDifferences[seriesIndex][^1]); // ^1 = Last element
            }

            sum += seriesDifferences[0][^1];
        }
        return sum.ToString();
    }

    public static string GetPart2Answer()
    {
        var sum = (long)0;
        
        foreach (var history in Histories)
        {
            var seriesDifferences = GetSeriesDifferences(history);

            for (var seriesIndex = seriesDifferences.Count-1; seriesIndex > 0; seriesIndex--)
            {
                var previousSeries = seriesDifferences[seriesIndex-1];
                previousSeries.Insert(0, previousSeries[0] - seriesDifferences[seriesIndex][0]);
            }

            sum += seriesDifferences[0][0];
        }
        return sum.ToString();
    }

    private static List<List<int>> GetSeriesDifferences(IEnumerable<int> series)
    {
        var differencesList = new List<List<int>>();
        var currentSeries = series.ToList();
        differencesList.Add(currentSeries);
        while (currentSeries.Any(x => x != 0))
        {
            currentSeries = currentSeries.Pairwise((left, right) => right - left).ToList();
            differencesList.Add(currentSeries);
        }

        return differencesList;
    }
}