namespace AdventOfCode2022.Logic.Assignments;

public static class Day4
{
    private static readonly string[] Lines = File.ReadAllLines("input_20221204.txt");
    private static readonly ElfPair[] ElfPairs = Lines.Select(line => new ElfPair(line)).ToArray();

    public static string GetPart1Answer()
    {
        var fullyOverlappingPairs = ElfPairs.Where(x => x.First.FullyOverlaps(x.Second));
        var fullyOverlappingPairsCount = fullyOverlappingPairs.Count();
        return fullyOverlappingPairsCount.ToString();
    }

    public static string GetPart2Answer()
    {
        var overlappingPairs = ElfPairs.Where(x => x.First.Overlaps(x.Second));
        var overlappingPairsCount = overlappingPairs.Count();
        return overlappingPairsCount.ToString();
    }

    private class ElfPair
    {
        public RangeInclusive First { get; private set; }
        public RangeInclusive Second { get; private set; }

        public ElfPair(string line)
        {
            var segments = line.Split(',');
            First = new RangeInclusive(segments[0]);
            Second = new RangeInclusive(segments[1]);
        }
    }

    private class RangeInclusive
    {
        public int From { get; private set; }
        public int To { get; private set; }

        public RangeInclusive(string str)
        {
            var segments = str.Split('-');
            From = int.Parse(segments[0]); 
            To = int.Parse(segments[1]);
        }

        public bool FullyOverlaps(RangeInclusive other)
        {
            return From <= other.From && To >= other.To ||
                   other.From <= From && other.To >= To;
        }

        public bool Overlaps(RangeInclusive other)
        {
            return From <= other.To && other.From <= To;
        }
    }
}