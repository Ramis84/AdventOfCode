using MoreLinq;

namespace AdventOfCode2022.Logic.Assignments;

public static class Day11
{
    private static readonly string[] Lines = File.ReadAllLines("input_20221211.txt");

    public static string GetPart1Answer()
    {
        var monkeys = Lines
            .Segment(string.IsNullOrWhiteSpace)
            .Select(x => new Monkey(x.SkipWhile(string.IsNullOrWhiteSpace).ToArray()))
            .ToArray();

        for (var round = 0; round < 20; round++)
        {
            foreach (var monkey in monkeys)
            {
                monkey.InspectAndThrowAllItems(monkeys, true);
            }
        }
        
        var top2Inspects = monkeys
            .Select(x => x.InspectedCount)
            .OrderByDescending(x => x)
            .Take(2)
            .ToArray();
        var monkeyBusiness = top2Inspects[0] * top2Inspects[1];
        return monkeyBusiness.ToString();
    }

    public static string GetPart2Answer()
    {
        var monkeys = Lines
            .Segment(string.IsNullOrWhiteSpace)
            .Select(x => new Monkey(x.SkipWhile(string.IsNullOrWhiteSpace).ToArray()))
            .ToArray();

        var modulo = monkeys.Select(x => x.TestDivisibleBy).Aggregate(1, (previous, current) => previous * current);

        for (var round = 0; round < 10000; round++)
        {
            foreach (var monkey in monkeys)
            {
                monkey.InspectAndThrowAllItems(monkeys, false, modulo);
            }
        }

        var top2Inspects = monkeys
            .Select(x => (long)x.InspectedCount)
            .OrderByDescending(x => x)
            .Take(2)
            .ToArray();
        var monkeyBusiness = top2Inspects[0] * top2Inspects[1];
        return monkeyBusiness.ToString();
    }

    private class Monkey
    {
        public List<long> Items { get; private set; }
        public string[] Operation { get; private set; }
        public int TestDivisibleBy { get; private set; }
        public int IfTestTrueThrowToMonkey { get; private set; }
        public int IfTestFalseThrowToMonkey { get; private set; }
        public int InspectedCount { get; private set; } = 0;

        public Monkey(string[] lines)
        {
            var startingItemsSegments = lines[1].Split(new[] { ':', ',' }, StringSplitOptions.TrimEntries);
            Items = startingItemsSegments[1..].Select(long.Parse).ToList();

            Operation = lines[2].Split('=')[1].TrimStart().Split(' ');
            
            var testDivisibleByStr = lines[3].Split("by ")[1];
            TestDivisibleBy = int.Parse(testDivisibleByStr);

            var testTrueMonkey = lines[4].Split("monkey ")[1];
            IfTestTrueThrowToMonkey = int.Parse(testTrueMonkey);

            var testFalseMonkey = lines[5].Split("monkey ")[1];
            IfTestFalseThrowToMonkey = int.Parse(testFalseMonkey);
        }

        public void InspectAndThrowAllItems(Monkey[] monkeys, bool isRelieved, int? modulo = null)
        {
            while (Items.Count > 0)
            {
                InspectedCount++;
                var currentItemWorryLevel = Items[0];
                currentItemWorryLevel = RunOperationOnWorryLevel(currentItemWorryLevel);
                if (isRelieved)
                {
                    currentItemWorryLevel /= 3;
                }

                if (modulo != null)
                {
                    currentItemWorryLevel %= modulo.Value; // Reduce huge numbers
                }
                var monkeyIdToGiveTo = currentItemWorryLevel % TestDivisibleBy == 0 
                    ? IfTestTrueThrowToMonkey 
                    : IfTestFalseThrowToMonkey;
                monkeys[monkeyIdToGiveTo].Items.Add(currentItemWorryLevel);
                Items.RemoveAt(0);
            }
        }

        private long RunOperationOnWorryLevel(long oldWorryLevel)
        {
            var secondOperand =
                Operation[2] == "old"
                    ? oldWorryLevel
                    : int.Parse(Operation[2]);
            return Operation[1] switch
            {
                "+" => oldWorryLevel + secondOperand,
                "*" => oldWorryLevel * secondOperand,
                _ => throw new ArgumentException("Unknown operator")
            };
        }
    }
}