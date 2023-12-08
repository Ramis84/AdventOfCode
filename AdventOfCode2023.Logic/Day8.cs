namespace AdventOfCode2023.Logic;

public static class Day8
{
    private static readonly string[] Lines = File.ReadAllLines("Inputs/input_20231208.txt");

    private static readonly Direction[] Directions = Lines[0].Select(c => c switch
    {
        'L' => Direction.Left,
        'R' => Direction.Right,
        _ => throw new ArgumentException("Invalid direction")
    }).ToArray();

    private static readonly (string From, string Left, string Right)[] Paths = Lines[2..]
        .Select(line =>
        {
            var segments = line.Split(new[] {'=', ','}, StringSplitOptions.TrimEntries);
            return (
                From: segments[0],
                Left: segments[1].TrimStart('('),
                Right: segments[2].TrimEnd(')'));
        })
        .ToArray();
    private static readonly Dictionary<string, (string Left, string Right)> PathsByFrom = 
        Paths.ToDictionary(x => x.From, x => (x.Left, x.Right));

    public static string GetPart1Answer()
    {
        var stepsUntilZzz = GetStepsUntil("AAA", location => location == "ZZZ");
        return stepsUntilZzz.ToString();
    }

    public static string GetPart2Answer()
    {
        var startingLocations = Paths
            .Select(x => x.From)
            .Where(x => x.EndsWith('A'));
        var stepsUntilEndsWithZ = startingLocations
            .Select(startingLocation =>
                GetStepsUntil(startingLocation,
                    location => location.EndsWith('Z')));
        
        // Find least common multiple of all numbers in stepsUntilEndsWithZ
        var lcm = stepsUntilEndsWithZ.Aggregate(Lcm);

        return lcm.ToString();
    }
    private static long GetStepsUntil(string startingLocation, Func<string, bool> endingLocationPredicate)
    {
        var currentLocation = startingLocation;
        var steps = (long)0;

        while (true)
        {
            foreach (var direction in Directions)
            {
                if (endingLocationPredicate(currentLocation))
                {
                    return steps;
                }
                
                var paths = PathsByFrom[currentLocation];

                currentLocation = direction switch
                {
                    Direction.Left => paths.Left,
                    Direction.Right => paths.Right,
                    _ => throw new ArgumentException("Invalid direction")
                };
                
                steps++;
            }
        }
    }

    private static long Lcm(long x, long y)
    {
        return x * y / Gcd(x, y);
    }
    
    private static long Gcd(long x, long y)
    {
        return y == 0 
            ? x 
            : Gcd(y, x % y);
    }

    private enum Direction
    {
        Left,
        Right
    }
}