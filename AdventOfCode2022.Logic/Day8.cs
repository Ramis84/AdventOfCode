namespace AdventOfCode2022.Logic;

public static class Day8
{
    private static readonly string[] Lines = File.ReadAllLines("Inputs/input_20221208.txt");
    private static readonly int Width = Lines[0].Length;
    private static readonly int Height = Lines.Length;

    public static string GetPart1Answer()
    {
        var visibleTreeCoordinates = new HashSet<(int X, int Y)>();
        int previousHeight;

        for (var y = 0; y < Height; y++)
        {
            previousHeight = -1;
            for (var x = 0; x < Width; x++)
            {
                var currentHeight = int.Parse(Lines[y][x].ToString());
                if (currentHeight > previousHeight)
                {
                    visibleTreeCoordinates.Add((X: x, Y: y));
                    previousHeight = currentHeight;
                }

                if (currentHeight == 9)
                {
                    break;
                }
            }

            previousHeight = -1;
            for (var x = Width-1; x >= 0; x--)
            {
                var currentHeight = int.Parse(Lines[y][x].ToString());
                if (currentHeight > previousHeight)
                {
                    visibleTreeCoordinates.Add((X: x, Y: y));
                    previousHeight = currentHeight;
                }

                if (currentHeight == 9)
                {
                    break;
                }
            }
        }

        for (var x = 0; x < Width; x++)
        {
            previousHeight = -1;
            for (var y = 0; y < Width; y++)
            {
                var currentHeight = int.Parse(Lines[y][x].ToString());
                if (currentHeight > previousHeight)
                {
                    visibleTreeCoordinates.Add((X: x, Y: y));
                    previousHeight = currentHeight;
                }

                if (currentHeight == 9)
                {
                    break;
                }
            }

            previousHeight = -1;
            for (var y = Width - 1; y >= 0; y--)
            {
                var currentHeight = int.Parse(Lines[y][x].ToString());
                if (currentHeight > previousHeight)
                {
                    visibleTreeCoordinates.Add((X: x, Y: y));
                    previousHeight = currentHeight;
                }

                if (currentHeight == 9)
                {
                    break;
                }
            }
        }

        var visibleTreesCount = visibleTreeCoordinates.Count;
        return visibleTreesCount.ToString();
    }

    public static string GetPart2Answer()
    {
        var bestScenicScore = 0;
        for (var x = 1; x < Width - 1; x++)
        {
            for (var y = 1; y < Height - 1; y++)
            {
                var currentScenicScore = GetScenicScore(x, y);
                bestScenicScore = int.Max(bestScenicScore, currentScenicScore);
            }
        }

        return bestScenicScore.ToString();
    }

    private static int GetScenicScore(int consideredTreeX, int consideredTreeY)
    {
        var consideredTreeHeight = int.Parse(Lines[consideredTreeY][consideredTreeX].ToString());

        // Look to the right
        var viewingDistanceRight = 0;
        for (var x = consideredTreeX + 1; x < Width; x++)
        {
            viewingDistanceRight++;

            var currentHeight = int.Parse(Lines[consideredTreeY][x].ToString());
            if (currentHeight >= consideredTreeHeight)
            {
                break;
            }
        }

        // Look to the left
        var viewingDistanceLeft = 0;
        for (var x = consideredTreeX - 1; x >= 0; x--)
        {
            viewingDistanceLeft++;

            var currentHeight = int.Parse(Lines[consideredTreeY][x].ToString());
            if (currentHeight >= consideredTreeHeight)
            {
                break;
            }
        }

        // Look down
        var viewingDistanceDown = 0;
        for (var y = consideredTreeY + 1; y < Height; y++)
        {
            viewingDistanceDown++;

            var currentHeight = int.Parse(Lines[y][consideredTreeX].ToString());
            if (currentHeight >= consideredTreeHeight)
            {
                break;
            }
        }

        // Look up
        var viewingDistanceUp = 0;
        for (var y = consideredTreeY - 1; y > 0; y--)
        {
            viewingDistanceUp++;

            var currentHeight = int.Parse(Lines[y][consideredTreeX].ToString());
            if (currentHeight >= consideredTreeHeight)
            {
                break;
            }
        }

        return viewingDistanceRight *
               viewingDistanceLeft *
               viewingDistanceDown *
               viewingDistanceUp;
    }
}