using MoreLinq;

namespace AdventOfCode2024.Logic;

public static class Day5
{
    private static readonly string[] Lines = File.ReadAllLines("Inputs/input_20241205.txt");
    private static readonly string[][] Segments = Lines.Split(string.IsNullOrWhiteSpace, x => x.ToArray()).ToArray();
    private static readonly PageOrderingRule[] PageOrderingRules = Segments[0].Select(x => new PageOrderingRule(x)).ToArray();
    private static readonly PageNumbers[] PageUpdates = Segments[1].Select(x => new PageNumbers(x)).ToArray();

    public static string GetPart1Answer()
    {
        var allCorrectUpdates = PageUpdates.Where(update => PageOrderingRules.All(x => update.IsCorrect(x)));
        return allCorrectUpdates.Select(x => x.GetMiddleNumber()).Sum().ToString();
    }

    public static string GetPart2Answer()
    {
        var sum = 0;
        foreach (var update in PageUpdates)
        {
            var checks = 0;
            bool isCorrect;
            do
            {
                isCorrect = PageOrderingRules.Aggregate(true, (current, rule) => current & update.IsCorrect(rule, swapIfIncorrect: true));
                checks++;
            } while (!isCorrect);

            if (checks > 1)
            {
                // Only use incorrectly-ordered updates
                sum += update.GetMiddleNumber();
            }
        }
        
        return sum.ToString();
    }

    private class PageOrderingRule
    {
        public int FirstPageNr { get; }
        public int SecondPageNr { get; }

        public PageOrderingRule(string line)
        {
            var segments = line.Split('|');
            FirstPageNr = int.Parse(segments[0]);
            SecondPageNr = int.Parse(segments[1]);
        }
    }

    private class PageNumbers
    {
        public int[] Numbers { get; }

        public PageNumbers(string line)
        {
            var segments = line.Split(',');
            Numbers = segments.Select(int.Parse).ToArray();
        }

        public bool IsCorrect(PageOrderingRule rule, bool swapIfIncorrect = false)
        {
            int? indexOfFirstPage = null, indexOfSecondPage = null;
            for (var i = 0; i < Numbers.Length; i++)
            {
                if (Numbers[i] == rule.FirstPageNr)
                {
                    indexOfFirstPage = i;
                }
                if (Numbers[i] == rule.SecondPageNr)
                {
                    indexOfSecondPage = i;
                }
            }

            if (!indexOfFirstPage.HasValue || 
                !indexOfSecondPage.HasValue ||
                indexOfFirstPage.Value < indexOfSecondPage.Value)
            {
                // Rule is followed if one number does not exist, or they are in order
                return true;
            }

            // Incorrect, swap if requested
            if (swapIfIncorrect)
            {
                (Numbers[indexOfFirstPage.Value], Numbers[indexOfSecondPage.Value]) = (Numbers[indexOfSecondPage.Value], Numbers[indexOfFirstPage.Value]);
            }
            
            return false;
        }

        public int GetMiddleNumber()
        {
            return Numbers[Numbers.Length / 2];
        }
    }
}