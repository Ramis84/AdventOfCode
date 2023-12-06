using MoreLinq;

namespace AdventOfCode2023.Logic.Assignments;

public static class Day3
{
    private static readonly string[] Lines = File.ReadAllLines("input_20231203.txt");
    private static readonly Symbol[] Symbols = GetSymbols(Lines);
    private static readonly Number[] Numbers = GetNumbers(Lines);

    public static string GetPart1Answer()
    {
        var allAdjacentNumbers = Symbols
            .SelectMany(symbol =>
                Numbers.Where(number => symbol.GetAdjacentCoordinates().Any(number.Includes)));
        return allAdjacentNumbers.Select(x => x.Value).Sum().ToString();
    }

    public static string GetPart2Answer()
    {
        var gearsAndAdjacentNumbers = Symbols
            .Where(x => x.Character == '*')
            .Select(symbol => new
            {
                Symbol = symbol,
                AdjacentNumbers = Numbers.Where(number => symbol.GetAdjacentCoordinates().Any(number.Includes)).ToArray()
            });
        var gearRatios = gearsAndAdjacentNumbers
            .Where(x => x.AdjacentNumbers.Length == 2)
            .Select(x => x.AdjacentNumbers[0].Value * x.AdjacentNumbers[1].Value);
        var gearRatiosSum = gearRatios.Sum();
        return gearRatiosSum.ToString();
    }

    private class Coordinate
    {
        public int RowIndex { get; init; }
        public int ColumnIndex { get; init; }
    }

    private static Symbol[] GetSymbols(string[] lines)
    {
        var symbols = lines.SelectMany((row, rowIndex) =>
            row.Select((c, columnIndex) => new Symbol
            {
                Character = c,
                RowIndex = rowIndex,
                ColumnIndex = columnIndex
            })).Where(x => !char.IsDigit(x.Character) && x.Character != '.');
        return symbols.ToArray();
    }

    private static Number[] GetNumbers(string[] lines)
    {
        var numbers = lines.SelectMany((row, rowIndex) =>
                row.Select((c, columnIndex) => new Symbol
                    {
                        Character = c,
                        RowIndex = rowIndex,
                        ColumnIndex = columnIndex
                    }).Where(x => char.IsDigit(x.Character))
                    .Segment((currentSymbol, previousSymbol, _) => currentSymbol.ColumnIndex != previousSymbol.ColumnIndex + 1)
                    .Select(x => x.ToArray())
                    .Select(segment => new Number
                    {
                        RowIndex = rowIndex,
                        ColumnIndices = new RangeInclusive
                        {
                            From = segment.Select(x => x.ColumnIndex).Min(),
                            To = segment.Select(x => x.ColumnIndex).Max()
                        },
                        Value = int.Parse(new string(segment.Select(x => x.Character).ToArray()))
                    }));
        return numbers.ToArray();
    }

    private class Symbol
    {
        public char Character { get; init; }
        public int RowIndex { get; init; }
        public int ColumnIndex { get; init; }

        public IEnumerable<Coordinate> GetAdjacentCoordinates()
        {
            return Enumerable.Range(RowIndex - 1, 3)
                .SelectMany(rowIndex =>
                    Enumerable.Range(ColumnIndex - 1, 3)
                        .Select(
                            columnIndex =>
                                new Coordinate { RowIndex = rowIndex, ColumnIndex = columnIndex }));
        }
    }

    private class Number
    {
        public int Value { get; init; }
        public int RowIndex { get; init; }
        public required RangeInclusive ColumnIndices { get; init; }

        public bool Includes(Coordinate coordinate)
        {
            return coordinate.RowIndex == RowIndex && ColumnIndices.Includes(coordinate.ColumnIndex);
        }
    }

    private class RangeInclusive
    {
        public int From { get; init; }
        public int To { get; init; }

        public bool Includes(int value)
        {
            return value >= From && value <= To;
        }
    }
}