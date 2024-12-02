namespace AdventOfCode2024.Logic;

public static class Day2
{
    private static readonly string[] Lines = File.ReadAllLines("Inputs/input_20241202.txt");
    private static readonly Report[] Reports = Lines.Select(x => new Report(x)).ToArray();

    public static string GetPart1Answer()
    {
        return Reports.Count(x => x.IsSafe()).ToString();
    }

    public static string GetPart2Answer()
    {
        return Reports.Count(x => x.IsSafe(true)).ToString();
    }

    private class Report
    {
        private readonly int[] _levels;
        
        public Report(string line)
        {
            var segments = line.Split(' ');
            _levels = segments.Select(int.Parse).ToArray();
        }

        public bool IsSafe(bool tolerateSingleError = false)
        {
            var allLevels = !tolerateSingleError ? [_levels] : GetDampenedLevels();
            return allLevels.Any(IsSafe);
        }

        private static bool IsSafe(IEnumerable<int> levels)
        {
            if (levels.Count() == 0) return true;
            if (levels.ElementAt(0) == levels.ElementAt(1)) return false;
            
            var increasing = levels.ElementAt(1) > levels.ElementAt(0);
            
            var previousLevel = levels.ElementAt(0);
            for (var i = 1; i < levels.Count(); i++)
            {
                var currentLevel = levels.ElementAt(i);
                var difference = currentLevel - previousLevel;
                if (increasing && (difference < 1 || difference > 3)) return false;
                if (!increasing && (difference < -3 || difference > -1)) return false;
                previousLevel = currentLevel;
            }

            return true;
        }

        private IEnumerable<IEnumerable<int>> GetDampenedLevels()
        {
            for (var i = 0; i < _levels.Length; i++)
            {
                var before = _levels.Take(i);
                var after = _levels.Skip(i+1);
                yield return before.Concat(after);
            }
        }
    }
}