using MoreLinq;

namespace AdventOfCode2023.Logic;

public static class Day11
{
    private static readonly string[] Lines = File.ReadAllLines("Inputs/input_20231211.txt");

    private static IEnumerable<Coordinate> GalaxyLocations => Lines
        .SelectMany((row, rowIndex) =>
            row.Select((c, columnIndex) => new
                {
                    Location = new Coordinate(columnIndex, rowIndex),
                    Type = c == '#' ? SpaceType.Galaxy : SpaceType.Empty
                })
                .Where(x => x.Type == SpaceType.Galaxy)
                .Select(x => x.Location));
    
    public static string GetPart1Answer()
    {
        var galaxiesExpanding = GalaxyLocations.ToList();

        ExpandEmptySpaceByMultiple(galaxiesExpanding, 2);

        var galaxyPairs = GetAllPairs(galaxiesExpanding);
        var distanceSum = galaxyPairs.Select(x => GetDistance(x.Item1, x.Item2)).Sum();
        return distanceSum.ToString();
    }

    public static string GetPart2Answer()
    {
        var galaxiesExpanding = GalaxyLocations.ToList();

        ExpandEmptySpaceByMultiple(galaxiesExpanding, 1000000);

        var galaxyPairs = GetAllPairs(galaxiesExpanding);
        var distanceSum = galaxyPairs.Select(x => GetDistance(x.Item1, x.Item2)).Sum();
        return distanceSum.ToString();
    }

    private static void ExpandEmptySpaceByMultiple(List<Coordinate> galaxies, int multiple)
    {
        // Expand columns
        var galaxiesX = galaxies
            .Select(x => x.X)
            .Distinct()
            .OrderBy(x => x)
            .ToList();
        var previousX = -1;
        var currentXExpansion = 0;
        foreach (var currentX in galaxiesX)
        {
            var currentXExpanded = currentX + currentXExpansion;
            var spacingLeft = currentXExpanded - previousX - 1;
            if (spacingLeft > 0)
            {
                var offset = spacingLeft * (multiple - 1);
                var galaxiesHereAndToTheRight = galaxies.Where(galaxy => galaxy.X >= currentXExpanded);
                foreach (var galaxy in galaxiesHereAndToTheRight)
                {
                    galaxy.X += offset;
                }

                currentXExpansion += offset;
                previousX = currentXExpanded + offset;
            }
            else
            {
                previousX = currentXExpanded;
            }
        }
        
        // Expand rows
        var galaxiesY = galaxies
            .Select(x => x.Y)
            .Distinct()
            .OrderBy(x => x)
            .ToList();
        var previousY = -1;
        var currentYExpansion = 0;
        foreach (var currentY in galaxiesY)
        {
            var currentYExpanded = currentY + currentYExpansion;
            var spacingUp = currentYExpanded - previousY - 1;
            if (spacingUp > 0)
            {
                var offset = spacingUp * (multiple - 1);
                var galaxiesHereDownwards = galaxies.Where(galaxy => galaxy.Y >= currentYExpanded);
                foreach (var galaxy in galaxiesHereDownwards)
                {
                    galaxy.Y += offset;
                }

                currentYExpansion += offset;
                previousY = currentYExpanded + offset;
            }
            else
            {
                previousY = currentYExpanded;
            }
        }
    }

    private static IEnumerable<(Coordinate, Coordinate)> GetAllPairs(IEnumerable<Coordinate> positions)
    {
        var positionsList = positions.ToList();
        for (var i = 0; i < positionsList.Count - 1; i++)
        {
            for (var j = i + 1; j < positionsList.Count; j++)
            {
                yield return (positionsList[i], positionsList[j]);
            }
        }
    }

    private static long GetDistance(Coordinate first, Coordinate second)
    {
        var distanceX = Math.Abs(first.X - second.X);
        var distanceY = Math.Abs(first.Y - second.Y);
        return (long)distanceX + (long)distanceY;
    }

    private enum SpaceType
    {
        Empty,
        Galaxy
    }
    
    private class Coordinate(int x, int y)
    {
        public int X { get; set; } = x;
        public int Y { get; set; } = y;
    }
}