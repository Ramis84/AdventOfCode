using MoreLinq;

namespace AdventOfCode2022.Logic.Assignments;

public static class Day3
{
    private static readonly string[] Lines = File.ReadAllLines("input_20221203.txt");
    private static readonly Rucksack[] Rucksacks = Lines.Select(line => new Rucksack(line)).ToArray();

    public static string GetPart1Answer()
    {
        var sumPriorities = Rucksacks.Select(x => x.GetMisplacedItemPriority()).Sum();
        return sumPriorities.ToString();
    }

    public static string GetPart2Answer()
    {
        var groups = Rucksacks.Batch(3);
        var sumBadgePriorities = groups.Select(Rucksack.FindBadgePriority).Sum();
        return sumBadgePriorities.ToString();
    }

    private class Rucksack
    {
        public string AllItems { get; private set; }
        public string FirstCompartment { get; private set; }
        public string SecondCompartment { get; private set; }

        public Rucksack(string line)
        {
            AllItems = line;

            if (line.Length % 2 != 0)
            {
                throw new ArgumentException("Number of items should be even");
            }

            FirstCompartment = line[..(line.Length / 2)];
            SecondCompartment = line[(line.Length / 2)..];
        }

        public int GetMisplacedItemPriority()
        {
            var duplicatedItem = FirstCompartment.Intersect(SecondCompartment).Single();
            return GetItemPriority(duplicatedItem);
        }

        public static int FindBadgePriority(IEnumerable<Rucksack> groupRucksacks)
        {
            var groupItems = groupRucksacks
                .Select(x => x.AllItems.ToHashSet())
                .ToArray();
            var badge = groupItems[1..]
                .Aggregate(
                    groupItems[0],
                    (intersectingItems, currentItems) =>
                    {
                        intersectingItems.IntersectWith(currentItems);
                        return intersectingItems;
                    })
                .Single();
            return GetItemPriority(badge);
        }

        private static int GetItemPriority(char item)
        {
            int priority;
            if (char.IsUpper(item))
            {
                priority = 26;
                item = char.ToLower(item);
            }
            else
            {
                priority = 0;
            }

            priority += item switch
            {
                'a' => 1,
                'b' => 2,
                'c' => 3,
                'd' => 4,
                'e' => 5,
                'f' => 6,
                'g' => 7,
                'h' => 8,
                'i' => 9,
                'j' => 10,
                'k' => 11,
                'l' => 12,
                'm' => 13,
                'n' => 14,
                'o' => 15,
                'p' => 16,
                'q' => 17,
                'r' => 18,
                's' => 19,
                't' => 20,
                'u' => 21,
                'v' => 22,
                'w' => 23,
                'x' => 24,
                'y' => 25,
                'z' => 26,
                _ => throw new ArgumentOutOfRangeException(nameof(item), item, "Invalid item")
            };

            return priority;
        }
    }
}