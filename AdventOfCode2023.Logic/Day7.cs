﻿namespace AdventOfCode2023.Logic;

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
        var totalWinnings = GetTotalWinnings(Hands, new Part1Comparer());
        return totalWinnings.ToString();
    }

    public static string GetPart2Answer()
    {
        var totalWinnings = GetTotalWinnings(Hands, new Part2Comparer());
        return totalWinnings.ToString();
    }

    private static int GetTotalWinnings(
        IEnumerable<(Hand Hand, int Bid)> hands,
        IComparer<Hand> comparer)
    {
        var handsWithRanks = hands
            .OrderBy(x => x.Hand, comparer)
            .Select((hand, index) => (
                hand.Hand,
                hand.Bid,
                Rank: index + 1));
        var totalWinnings = handsWithRanks.Select(x => x.Bid * x.Rank).Sum();
        return totalWinnings;
    }

    private class Hand(string cards)
    {
        public string Cards { get; } = cards;
    }

    private enum CardType
    {
        HighCard,
        OnePair,
        TwoPair,
        ThreeOfAKind,
        FullHouse,
        FourOfAKind,
        FiveOfAKind
    }

    private class Part1Comparer : IComparer<Hand>
    {
        public int Compare(Hand? x, Hand? y)
        {
            ArgumentNullException.ThrowIfNull(x);
            ArgumentNullException.ThrowIfNull(y);

            var xType = GetHandType(x);
            var yType = GetHandType(y);
            var typeComparison = xType - yType;
            if (typeComparison != 0)
            {
                return typeComparison;
            }
            
            for (var i = 0; i < 5; i++)
            {
                var comparison = GetCardValue(x.Cards[i]) - GetCardValue(y.Cards[i]);
                if (comparison != 0)
                    return comparison;
            }

            return 0;
        }
        
        private static CardType GetHandType(Hand hand)
        {
            var cardsGroupedWithCount = hand.Cards
                .GroupBy(card => card)
                .Select(x => new { Card = x.Key, Count = x.Count() })
                .OrderByDescending(x => x.Count)
                .ToArray();
            return cardsGroupedWithCount[0].Count switch
            {
                5 => CardType.FiveOfAKind,
                4 => CardType.FourOfAKind,
                3 when cardsGroupedWithCount[1].Count == 2 => CardType.FullHouse,
                3 => CardType.ThreeOfAKind,
                2 when cardsGroupedWithCount[1].Count == 2 => CardType.TwoPair,
                2 => CardType.OnePair,
                _ => CardType.HighCard
            };
        }

        private static int GetCardValue(char card)
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

    private class Part2Comparer : IComparer<Hand>
    {
        public int Compare(Hand? x, Hand? y)
        {
            ArgumentNullException.ThrowIfNull(x);
            ArgumentNullException.ThrowIfNull(y);

            var xType = GetHandType(x);
            var yType = GetHandType(y);
            var typeComparison = xType - yType;
            if (typeComparison != 0)
            {
                return typeComparison;
            }
            
            for (var i = 0; i < 5; i++)
            {
                var comparison = GetCardValue(x.Cards[i]) - GetCardValue(y.Cards[i]);
                if (comparison != 0)
                    return comparison;
            }

            return 0;
        }
        
        private static CardType GetHandType(Hand hand)
        {
            var jokerCount = hand.Cards.Count(x => x == 'J');
            if (jokerCount == 5)
            {
                return CardType.FiveOfAKind;
            }
            
            var cardsGroupedWithCount = hand.Cards
                .Where(x => x != 'J')
                .GroupBy(card => card)
                .Select(x => new { Card = x.Key, Count = x.Count() })
                .OrderByDescending(x => x.Count)
                .ToArray();
            
            var currentType = cardsGroupedWithCount[0].Count switch
            {
                5 => CardType.FiveOfAKind,
                4 => CardType.FourOfAKind,
                3 when cardsGroupedWithCount.Length > 1 && 
                       cardsGroupedWithCount[1].Count == 2 => CardType.FullHouse,
                3 => CardType.ThreeOfAKind,
                2 when cardsGroupedWithCount.Length > 1 && 
                       cardsGroupedWithCount[1].Count == 2 => CardType.TwoPair,
                2 => CardType.OnePair,
                _ => CardType.HighCard
            };
            for (var i = 0; i < jokerCount; i++)
            {
                // Upgrade
                currentType = currentType switch
                {
                    CardType.HighCard => CardType.OnePair,
                    CardType.OnePair => CardType.ThreeOfAKind,
                    CardType.TwoPair => CardType.FullHouse,
                    CardType.ThreeOfAKind => CardType.FourOfAKind,
                    CardType.FourOfAKind => CardType.FiveOfAKind,
                    _ => throw new ArgumentException("Invalid cards")
                };
            }

            return currentType;
        }

        private static int GetCardValue(char card)
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
}