namespace AdventOfCode2022.Logic.Assignments;

public static class Day6
{
    private static readonly string[] Lines = File.ReadAllLines("input_20221206.txt");
    private static readonly string Stream = Lines[0];

    public static string GetPart1Answer()
    {
        var streamSpan = Stream.AsSpan();
        for (int i = 0; i < streamSpan.Length; i++)
        {
            var hasDuplicates = HasDuplicateCharacters(streamSpan.Slice(i, 4));
            if (!hasDuplicates)
            {
                return (i + 4).ToString();
            }
        }

        throw new Exception("Cannot find marker");
    }

    public static string GetPart2Answer()
    {
        var streamSpan = Stream.AsSpan();
        for (int i = 0; i < streamSpan.Length; i++)
        {
            var hasDuplicates = HasDuplicateCharacters(streamSpan.Slice(i, 14));
            if (!hasDuplicates)
            {
                return (i + 14).ToString();
            }
        }

        throw new Exception("Cannot find marker");
    }

    private static bool HasDuplicateCharacters(ReadOnlySpan<char> str)
    {
        var hashset = new HashSet<char>();
        foreach (var c in str)
        {
            if (!hashset.Add(c)) return true;
        }
        return false;
    }
}