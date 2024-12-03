using System.Text.RegularExpressions;

namespace AdventOfCode2024.Logic;

public static class Day3
{
    private static readonly string[] Lines = File.ReadAllLines("Inputs/input_20241203.txt");
    
    private static readonly Regex MulRegex = new Regex(@"mul\((\d+){1,3},(\d+){1,3}\)");
    private static readonly Regex MulDoDontRegex = new Regex(@"mul\((\d+){1,3},(\d+){1,3}\)|do\(\)|don't\(\)");

    public static string GetPart1Answer()
    {
        var sum = 0;
        foreach (var line in Lines)
        {
            var matches = MulRegex.Matches(line);
            foreach (Match match in matches)
            {
                var leftFactor = int.Parse(match.Groups[1].Value);
                var rightFactor = int.Parse(match.Groups[2].Value);
                sum += leftFactor * rightFactor;
            }
        }
        return sum.ToString();
    }

    public static string GetPart2Answer()
    {
        var sum = 0;
        var mulIsEnabled = true;
        foreach (var line in Lines)
        {
            var matches = MulDoDontRegex.Matches(line);
            foreach (Match match in matches)
            {
                if (match.Value == "do()")
                {
                    mulIsEnabled = true;
                }
                else if (match.Value == "don't()")
                {
                    mulIsEnabled = false;
                }
                else if (mulIsEnabled)
                {
                    // mul
                    var leftFactor = int.Parse(match.Groups[1].Value);
                    var rightFactor = int.Parse(match.Groups[2].Value);
                    sum += leftFactor * rightFactor;
                }
            }
        }
        return sum.ToString();
    }
}