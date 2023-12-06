namespace AdventOfCode2022.Logic;

public static class Day2
{
    private static readonly string[] Lines = File.ReadAllLines("Inputs/input_20221202.txt");

    public static string GetPart1Answer()
    {
        var rounds = Lines
            .Select(Round.ParsePart1)
            .ToArray();
        var myTotalScore = rounds.Select(x => x.MyScore).Sum();
        return myTotalScore.ToString();
    }

    public static string GetPart2Answer()
    {
        var rounds = Lines
            .Select(Round.ParsePart2)
            .ToArray();
        var myTotalScore = rounds.Select(x => x.MyScore).Sum();
        return myTotalScore.ToString();
    }

    private enum Hand
    {
        Rock = 1,
        Paper = 2,
        Scissors = 3
    }

    private enum Outcome
    {
        Lose = 0,
        Draw = 3,
        Win = 6
    }

    private class Round
    {
        private Hand Opponent { get; init; }
        private Hand Me { get; set; }
        private Outcome Outcome { get; set; }

        public static Round ParsePart1(string line)
        {
            var round = new Round
            {
                Opponent = CharToHand(line[0]),
                Me = CharToHand(line[2])
            };

            if (round.Me == round.Opponent)
            {
                round.Outcome = Outcome.Draw;
            }
            else if (round is { Me: Hand.Rock, Opponent: Hand.Scissors } or 
                     { Me: Hand.Paper, Opponent: Hand.Rock } or 
                     { Me: Hand.Scissors, Opponent: Hand.Paper })
            {
                round.Outcome = Outcome.Win;
            }
            else
            {
                round.Outcome = Outcome.Lose;
            }

            return round;
        }

        public static Round ParsePart2(string line)
        {
            var round = new Round
            {
                Opponent = CharToHand(line[0]),
                Outcome = CharToOutcome(line[2])
            };

            if (round.Outcome == Outcome.Draw)
            {
                round.Me = round.Opponent;
            }
            else if (round.Outcome == Outcome.Win)
            {
                round.Me = round.Opponent switch
                {
                    Hand.Rock => Hand.Paper,
                    Hand.Paper => Hand.Scissors,
                    Hand.Scissors => Hand.Rock,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
            else // Lose
            {
                round.Me = round.Opponent switch
                {
                    Hand.Rock => Hand.Scissors,
                    Hand.Paper => Hand.Rock,
                    Hand.Scissors => Hand.Paper,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }

            return round;
        }

        private static Hand CharToHand(char c)
        {
            return c switch
            {
                'A' or 'X' => Hand.Rock,
                'B' or 'Y' => Hand.Paper,
                'C' or 'Z' => Hand.Scissors,
                _ => throw new ArgumentOutOfRangeException(nameof(c), c, "Invalid hand character")
            };
        }

        private static Outcome CharToOutcome(char c)
        {
            return c switch
            {
                'X' => Outcome.Lose,
                'Y' => Outcome.Draw,
                'Z' => Outcome.Win,
                _ => throw new ArgumentOutOfRangeException(nameof(c), c, "Invalid outcome character")
            };
        }

        public int MyScore => (int)Me + (int)Outcome;
    }
}