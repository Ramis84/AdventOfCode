namespace AdventOfCode2023.Logic;

public static class Day4
{
    private static readonly string[] Lines = File.ReadAllLines("Inputs/input_20231204.txt");

    public static string GetPart1Answer()
    {
        var cards = Lines.Select(line => new Card(line));
        var totalPoints = cards.Select(x => x.GetPoints()).Sum();
        return totalPoints.ToString();
    }

    public static string GetPart2Answer()
    {
        var cards = Lines.Select(line => new Card(line)).ToArray();
        for (var i = 0; i < cards.Length; i++)
        {
            var currentCard = cards[i];
            var winningNumbersCount = currentCard.CountWinningNumbers();

            for (var j = 0; j < winningNumbersCount; j++)
            {
                var nextCard = cards[i + j + 1];
                nextCard.Copies += currentCard.Copies;
            }
        }

        var cardsCopiesCount = cards.Select(x => x.Copies).Sum();
        return cardsCopiesCount.ToString();
    }

    private class Card
    {
        public int CardId { get; private set; }
        public HashSet<int> WinningNumbers { get; private set; }
        public HashSet<int> MyNumbers { get; private set; }
        public int Copies { get; set; } = 1;

        public Card(string line)
        {
            var segments = line.Split(new [] {':', '|' }, StringSplitOptions.TrimEntries);

            // Card 123
            var cardIdSegments = segments[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            CardId = int.Parse(cardIdSegments[1]);

            // 41 48 83 86 17
            WinningNumbers = segments[1]
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToHashSet();

            // 83 86  6 31 17  9 48 53
            MyNumbers = segments[2]
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToHashSet();
        }

        public int CountWinningNumbers()
        {
            var winningNumbers = MyNumbers.Where(number => WinningNumbers.Contains(number));
            var winningNumbersCount = winningNumbers.Count();
            return winningNumbersCount;
        }

        public int GetPoints()
        {
            var winningNumbers = MyNumbers.Where(number => WinningNumbers.Contains(number));
            var winningNumbersCount = winningNumbers.Count();
            var points = winningNumbersCount == 0 
                ? 0 
                : Math.Pow(2, winningNumbersCount - 1);
            return (int)points;
        }
    }
}