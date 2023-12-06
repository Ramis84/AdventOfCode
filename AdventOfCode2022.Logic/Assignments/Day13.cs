using MoreLinq;

namespace AdventOfCode2022.Logic.Assignments;

public static class Day13
{
    private static readonly string[] Lines = File.ReadAllLines("input_20221213.txt");

    private static readonly (PacketList, PacketList)[] ListPairs = Lines
        .Segment(string.IsNullOrWhiteSpace)
        .Select(lineGroup => lineGroup
            .SkipWhile(string.IsNullOrWhiteSpace)
            .Select(PacketList.Parse)
            .ToArray())
        .Select(x => (x[0], x[1]))
        .ToArray();

    public static string GetPart1Answer()
    {
        var pairsWithIndex = ListPairs.Select((pair, index) => new
        {
            Pair = pair,
            Index = index + 1
        });
        var pairInOrder = pairsWithIndex.Where(x => x.Pair.Item1.CompareTo(x.Pair.Item2) < 0);
        var sumIndices = pairInOrder.Select(x => x.Index).Sum();
        return sumIndices.ToString();
    }

    public static string GetPart2Answer()
    {
        var firstDividerPacket = PacketList.Parse("[[2]]");
        var secondDividerPacket = PacketList.Parse("[[6]]");
        var allPackets = ListPairs
            .SelectMany(x => new[] { x.Item1, x.Item2 })
            .Append(firstDividerPacket)
            .Append(secondDividerPacket)
            .ToList();
        allPackets.Sort();
        var firstIndex = allPackets.IndexOf(firstDividerPacket) + 1;
        var secondIndex = allPackets.IndexOf(secondDividerPacket) + 1;
        var product = firstIndex * secondIndex;
        return product.ToString();
    }
    
    private interface IPacket : IComparable<IPacket>
    {
        PacketList AsList();
    }

    private class PacketList(List<IPacket> elements) : IPacket
    {
        public List<IPacket> Elements { get; } = elements;

        public static PacketList Parse(string input)
        {
            var (packet, remaining) = TakeElement(input);
            if (!string.IsNullOrEmpty(remaining))
            {
                throw new ArgumentException("Invalid list format");
            }

            if (packet is PacketList list)
            {
                return list;
            }

            throw new ArgumentException("Not a list");
        }

        public int CompareTo(IPacket? other)
        {
            var otherList = other?.AsList();

            using var myElements = elements.GetEnumerator();
            using var otherElements = otherList.Elements.GetEnumerator();

            while (true)
            {
                var meHasElements = myElements.MoveNext();
                var otherHasElements = otherElements.MoveNext();

                if (!meHasElements && otherHasElements)
                    return -1;
                if (!meHasElements && !otherHasElements)
                    return 0;
                if (meHasElements && !otherHasElements)
                    return 1;

                var elementComparison = myElements.Current.CompareTo(otherElements.Current);
                if (elementComparison != 0)
                {
                    return elementComparison;
                }
            }
        }

        public PacketList AsList()
        {
            return this;
        }

        public override string ToString()
        {
            return $"[{string.Join(",", Elements.Select(x => x.ToString()))}]";
        }
        
        private static (IPacket Packet, string Remaining) TakeElement(string input)
        {
            if (input[0] == '[')
            {
                var elements = new List<IPacket>();
                var remaining = input[1..]; // Trim beginning '['
                while (remaining[0] != ']')
                {
                    var (packet, nextRemaining) = TakeElement(remaining);
                    elements.Add(packet);
                    remaining = nextRemaining[0] == ',' ? nextRemaining[1..] : nextRemaining;
                }
                return (
                    Packet: new PacketList(elements), 
                    Remaining: remaining[1..]); // Trim ending ']'
            }

            if (char.IsDigit(input[0]))
            {
                var indexOfFirstNonDigit = input.IndexOfAny(new[] { ',', '[', ']' });
                var valueString = input[..indexOfFirstNonDigit];
                var value = int.Parse(valueString);
                return (
                    Packet: new PacketInteger { Value = value }, 
                    Remaining: input[indexOfFirstNonDigit..]);
            }

            throw new ArgumentException("Invalid element");
        }
    }

    private class PacketInteger : IPacket
    {
        public int Value { get; init; }

        public int CompareTo(IPacket? other)
        {
            if (other is PacketInteger otherInteger)
            {
                return Value - otherInteger.Value;
            }

            return AsList().CompareTo(other);
        }

        public PacketList AsList()
        {
            return new PacketList(new List<IPacket> { this });
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}