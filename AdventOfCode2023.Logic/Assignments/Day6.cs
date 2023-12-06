namespace AdventOfCode2023.Logic.Assignments;

public static class Day6
{
    private static readonly string[] Lines = File.ReadAllLines("input_20231206.txt");

    public static string GetPart1Answer()
    {
        var segmentedLines = Lines.Select(x => x.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).ToArray()).ToArray();

        var games = new List<Game>();
        for (int columnIndex = 1; columnIndex < segmentedLines[0].Length; columnIndex++)
        {
            var time = long.Parse(segmentedLines[0][columnIndex]);
            var distance = long.Parse(segmentedLines[1][columnIndex]);
            games.Add(new Game { GameTime = time, RecordDistance = distance });
        }

        var winningCasesCounts = games.Select(x => x.GetWinningCasesCount());
        var product = winningCasesCounts.Aggregate((long)1, (product, currentValue) => product * currentValue);
        return product.ToString();
    }

    public static string GetPart2Answer()
    {
        var segmentedLines = Lines.Select(x => x.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).ToArray()).ToArray();

        var timeString = string.Join("", segmentedLines[0][1..]);
        var time = long.Parse(timeString);
        var distanceString = string.Join("", segmentedLines[1][1..]);
        var distance = long.Parse(distanceString);
        var game = new Game { GameTime = time, RecordDistance = distance };

        var winningCasesCount = game.GetWinningCasesCount();
        return winningCasesCount.ToString();
    }

    private class Game
    {
        /// <summary>
        /// Game time
        /// </summary>
        public long GameTime { get; init; }
        
        /// <summary>
        /// Record distance to beat
        /// </summary>
        public long RecordDistance { get; init; }

        public long GetWinningCasesCount()
        {
            // Formula for pressTime -> travelledDistance:
            // pressTime * (GameTime - pressTime) = travelledDistance;
            
            // Find pressTime, so that:
            // pressTime * (GameTime - pressTime) = RecordDistance;
            
            // Rearrange to standard form, ax^2+bx+c=0, where x is pressTime:
            // -1*pressTime^2 * GameTime*pressTime - RecordDistance = 0;
            // a = -1, b = GameTime, c = -RecordDistance
            
            // Quadratic formula: x = (-b +- sqrt(b^2 - 4ac)) / 2a
            // pressTime = (-GameTime +- sqrt(GameTime^2 - 4*-1*-RecordDistance)) / (2*-1)
            // pressTime = (-GameTime +- sqrt(GameTime^2 - 4*RecordDistance)) / -2

            var pressTimeMin = (-GameTime + Math.Sqrt(GameTime*GameTime - 4 * RecordDistance)) / -2;
            var pressTimeMax = (-GameTime - Math.Sqrt(GameTime*GameTime - 4 * RecordDistance)) / -2;
            var pressTimeCount = Math.Ceiling(pressTimeMax-1) - Math.Floor(pressTimeMin+1) + 1;
            return (long)pressTimeCount;
        }
    }
}