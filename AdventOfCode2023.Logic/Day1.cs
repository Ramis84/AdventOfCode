using System.Text.RegularExpressions;

namespace AdventOfCode2023.Logic;

public static class Day1
{
    private static readonly string[] Lines = File.ReadAllLines("Inputs/input_20231201.txt");
    private const string RegexWords = "1|2|3|4|5|6|7|8|9|one|two|three|four|five|six|seven|eight|nine";
    private static readonly Regex RegexWordsLeftToRight = new(RegexWords);
    private static readonly Regex RegexWordsRightToLeft = new(RegexWords, RegexOptions.RightToLeft);

    public static string GetPart1Answer()
    {
        var sumPt1 = Lines.Aggregate(0, (sum, line) =>
        {
            var firstDigit = line.First(char.IsDigit);
            var lastDigit = line.Last(char.IsDigit);
            var number = int.Parse(new string(new[] { firstDigit, lastDigit }));
            return sum + number;
        });
        return sumPt1.ToString();
    }

    public static string GetPart2Answer()
    {
        var sumPt2 = Lines.Aggregate(0, (sum, line) =>
        {
            var firstMatch = RegexWordsLeftToRight.Match(line).Value;
            var lastMatch = RegexWordsRightToLeft.Match(line).Value;
            var firstDigit = WordToDigit(firstMatch);
            var lastDigit = WordToDigit(lastMatch);
            var number = int.Parse(new string(new[] { firstDigit, lastDigit }));
            return sum + number;
        });
        return sumPt2.ToString();
    }

    private static char WordToDigit(string word)
    {
        return word switch
        {
            "1" or "one" => '1',
            "2" or "two" => '2',
            "3" or "three" => '3',
            "4" or "four" => '4',
            "5" or "five" => '5',
            "6" or "six" => '6',
            "7" or "seven" => '7',
            "8" or "eight" => '8',
            "9" or "nine" => '9',
            _ => throw new ArgumentOutOfRangeException(nameof(word), word, "Unhandled case")
        };
    }
}