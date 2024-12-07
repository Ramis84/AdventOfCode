namespace AdventOfCode2024.Logic;

public static class Day7
{
    private static readonly string[] Lines = File.ReadAllLines("Inputs/input_20241207.txt");
    private static readonly Equation[] Equations = Lines.Select(x => new Equation(x)).ToArray();

    public static string GetPart1Answer()
    {
        var possibleEquations = Equations.Where(x => x.IsPossible());
        return possibleEquations.Select(x => x.Result).Sum().ToString();
    }

    public static string GetPart2Answer()
    {
        var possibleEquations = Equations.Where(x => x.IsPossible(allowConcatination: true));
        return possibleEquations.Select(x => x.Result).Sum().ToString();
    }

    private class Equation
    {
        public long Result { get; }
        public long[] Numbers { get; }
        
        public Equation(string line)
        {
            var segments = line.Split(": ");
            Result = long.Parse(segments[0]);
            var numbersStrings = segments[1].Split(' ');
            Numbers = numbersStrings.Select(long.Parse).ToArray();
        }

        public bool IsPossible(bool allowConcatination = false)
        {
            return GetAllPossibleResults(Numbers.Length-1, allowConcatination).Any(x => x == Result);
        }

        private IEnumerable<long> GetAllPossibleResults(int toIndex, bool allowConcatination)
        {
            var last = Numbers[toIndex];
            if (toIndex == 0)
            {
                yield return last;
            }
            else
            {
                var previousResults = GetAllPossibleResults(toIndex-1, allowConcatination);
                foreach (var previousResult in previousResults)
                {
                    yield return previousResult * last;
                    yield return previousResult + last;

                    if (allowConcatination)
                    {
                        yield return long.Parse($"{previousResult.ToString()}{last.ToString()}");
                    }
                }
            }
        }
    }
}