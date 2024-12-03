using System.Buffers;

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
        return Reports
            .Select(x => x.GetDampenedReports())
            .Count(dampenedReports => dampenedReports.Any(report => report.IsSafe()))
            .ToString();
    }

    private class Report
    {
        private readonly int[] _levels;
        
        public Report(string line)
        {
            var segments = line.Split(' ');
            _levels = segments.Select(int.Parse).ToArray();
        }

        private Report(IEnumerable<int> levels)
        {
            _levels = levels.ToArray();
        }

        public bool IsSafe()
        {
            // Empty report is safe
            if (_levels.Length == 0) return true;
            
            // Unchanged level is unsafe
            if (_levels[0] == _levels[1]) return false;
            
            // The report needs to be continuing increasing/decreasing
            var increasing = _levels[1] > _levels[0];
            
            var previousLevel = _levels[0];
            for (var i = 1; i < _levels.Length; i++)
            {
                var currentLevel = _levels[i];
                var difference = currentLevel - previousLevel;
                
                // Check if not increasing/decreasing
                if (increasing && difference is < 1 or > 3) return false;
                if (!increasing && difference is < -3 or > -1) return false;
                
                previousLevel = currentLevel;
            }

            // No unsafe levels is found, so report is safe
            return true;
        }

        public IEnumerable<Report> GetDampenedReports()
        {
            // Generate all possible reports where a single level is removed
            for (var i = 0; i < _levels.Length; i++)
            {
                var before = _levels[..i];
                var after = _levels[(i+1)..];
                yield return new Report(before.Concat(after));
            }
        }
    }
}