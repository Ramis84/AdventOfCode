namespace AdventOfCode2023.Logic;

public static class Day7
{
    private static readonly string[] Lines = File.ReadAllLines("Inputs/input_20231207.txt");
    private static readonly (Hand Hand, int Bid)[] Hands = Lines
        .Select(line =>
        {
            var segments = line.Split(' ');
            var hand = new Hand(segments[0]);
            var bid = int.Parse(segments[1]);
            return (Hand: hand, Bid: bid);
        })
        .ToArray();

    public static string GetPart1Answer()
    {
        var totalWinnings = GetTotalWinnings(Hands, new Part1HandComparer());
        return totalWinnings.ToString();
    }

    public static string GetPart2Answer()
    {
        var totalWinnings = GetTotalWinnings(Hands, new Part2HandComparer());
        return totalWinnings.ToString();
    }

    private static int GetTotalWinnings(
        IEnumerable<(Hand Hand, int Bid)> hands,
        IComparer<Hand> handComparer)
    {
        var handsWithRanks = hands
            .OrderBy(x => x.Hand, handComparer)
            .Select((hand, index) => new
            {
                hand.Hand,
                hand.Bid,
                Rank = index + 1 
            });
        var totalWinnings = handsWithRanks.Select(x => x.Bid * x.Rank).Sum();
        return totalWinnings;
    }

    private class Part1HandComparer : IComparer<Hand>
    {
        public int Compare(Hand? x, Hand? y)
        {
            ArgumentNullException.ThrowIfNull(x);
            return x.CompareTo(y, GetHandType, GetCardStrength);
        }
        
        private static HandType GetHandType(Hand hand)
        {
            var handType = Hand.GetHandType(hand.Cards);
            return handType;
        }

        private static int GetCardStrength(char card)
        {
            return card switch
            {
                'A' => 12,
                'K' => 11,
                'Q' => 10,
                'J' => 9,
                'T' => 8,
                '9' => 7,
                '8' => 6,
                '7' => 5,
                '6' => 4,
                '5' => 3,
                '4' => 2,
                '3' => 1,
                '2' => 0,
                _ => throw new ArgumentOutOfRangeException(nameof(card), card, null)
            };
        }
    }

    private class Part2HandComparer : IComparer<Hand>
    {
        public int Compare(Hand? x, Hand? y)
        {
            ArgumentNullException.ThrowIfNull(x);
            return x.CompareTo(y, GetHandType, GetCardStrength);
        }
        
        private static HandType GetHandType(Hand hand)
        {
            var jokerCount = hand.Cards.Count(x => x == 'J');
            if (jokerCount == 5)
            {
                return HandType.FiveOfAKind;
            }

            var cardsExceptJokers = hand.Cards.Where(x => x != 'J');
            var currentHandType = Hand.GetHandType(cardsExceptJokers);
            
            for (var i = 0; i < jokerCount; i++)
            {
                // Upgrade
                currentHandType = currentHandType switch
                {
                    HandType.HighCard => HandType.OnePair,
                    HandType.OnePair => HandType.ThreeOfAKind,
                    HandType.TwoPair => HandType.FullHouse,
                    HandType.ThreeOfAKind => HandType.FourOfAKind,
                    HandType.FourOfAKind => HandType.FiveOfAKind,
                    _ => throw new ArgumentException("Invalid cards")
                };
            }

            return currentHandType;
        }

        private static int GetCardStrength(char card)
        {
            return card switch
            {
                'A' => 12,
                'K' => 11,
                'Q' => 10,
                'T' => 9,
                '9' => 8,
                '8' => 7,
                '7' => 6,
                '6' => 5,
                '5' => 4,
                '4' => 3,
                '3' => 2,
                '2' => 1,
                'J' => 0,
                _ => throw new ArgumentOutOfRangeException(nameof(card), card, null)
            };
        }
    }

    private class Hand(string cards)
    {
        public string Cards { get; } = cards;
        
        public int CompareTo(
            Hand? other,
            Func<Hand, HandType> handTypeSelector,
            Func<char, int> cardStrengthSelector)
        {
            ArgumentNullException.ThrowIfNull(other);

            var handTypeComparison = handTypeSelector(this) - handTypeSelector(other);
            if (handTypeComparison != 0)
                return handTypeComparison;
            
            for (var i = 0; i < 5; i++)
            {
                var cardStrengthComparison = cardStrengthSelector(cards[i]) - cardStrengthSelector(other.Cards[i]);
                if (cardStrengthComparison != 0)
                    return cardStrengthComparison;
            }

            return 0;
        } 

        public static HandType GetHandType(IEnumerable<char> cards)
        {
            var cardsGroupedWithCount = cards
                .GroupBy(card => card)
                .Select(x => new { Card = x.Key, Count = x.Count() })
                .OrderByDescending(x => x.Count)
                .ToArray();
            return cardsGroupedWithCount[0].Count switch
            {
                5 => HandType.FiveOfAKind,
                4 => HandType.FourOfAKind,
                3 when cardsGroupedWithCount.Length > 1 && 
                       cardsGroupedWithCount[1].Count == 2 => HandType.FullHouse,
                3 => HandType.ThreeOfAKind,
                2 when cardsGroupedWithCount.Length > 1 && 
                       cardsGroupedWithCount[1].Count == 2 => HandType.TwoPair,
                2 => HandType.OnePair,
                _ => HandType.HighCard
            };
        }
    }

    private enum HandType
    {
        HighCard,
        OnePair,
        TwoPair,
        ThreeOfAKind,
        FullHouse,
        FourOfAKind,
        FiveOfAKind
    }
}