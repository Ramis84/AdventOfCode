using System.Text;

namespace AdventOfCode2022.Logic.Assignments;

public static class Day10
{
    private static readonly string[] Lines = File.ReadAllLines("input_20221210.txt");

    public static string GetPart1Answer()
    {
        var currentX = 1;
        var currentCycle = 1;
        var currentSignalStrengthSum = 0;

        foreach (var line in Lines)
        {
            if (currentCycle % 40 == 20)
            {
                currentSignalStrengthSum += currentCycle * currentX;
            }

            var segments = line.Split(' ');
            if (segments[0] == "addx")
            {
                var value = int.Parse(segments[1]);

                currentCycle++;

                if (currentCycle % 40 == 20)
                {
                    currentSignalStrengthSum += currentCycle * currentX;
                }

                currentX += value;
            }
            currentCycle++;
        }
        return currentSignalStrengthSum.ToString();
    }

    public static string GetPart2Answer()
    {
        var sb = new StringBuilder();
        var currentX = 1;
        var currentCycle = 0;

        foreach (var line in Lines)
        {
            PrintPixel(sb, ++currentCycle, currentX);

            var segments = line.Split(' ');
            if (segments[0] != "addx") continue; // noop

            PrintPixel(sb, ++currentCycle, currentX);

            var value = int.Parse(segments[1]);
            currentX += value;
        }
        return sb.ToString();
    }

    public static void PrintPixel(StringBuilder sb, int currentCycle, int currentX)
    {
        if (currentCycle > 1 && currentCycle % 40 == 1)
        {
            sb.Append(Environment.NewLine);
        }

        sb.Append(Math.Abs(currentX - (currentCycle - 1) % 40) <= 1 ? "#" : ".");
    }
}