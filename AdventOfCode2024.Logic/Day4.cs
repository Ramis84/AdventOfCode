namespace AdventOfCode2024.Logic;

public static class Day4
{
    private static readonly string[] Lines = File.ReadAllLines("Inputs/input_20241204.txt");
    private static readonly int Width = Lines.First().Length;
    private static readonly int Height = Lines.Length;

    public static string GetPart1Answer()
    {
        var count = 0;
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                foreach (var dir in Enum.GetValues<Direction>())
                {
                    if (ContainsWord((x, y), dir, "XMAS")) count++;
                }
            }
        }
        return count.ToString();
    }

    public static string GetPart2Answer()
    {
        var count = 0;
        for (var y = 1; y < Height-1; y++)
        {
            for (var x = 1; x < Width-1; x++)
            {
                if (HasXMas((x, y)))
                {
                    count++;
                }
            }
        }
        return count.ToString();
    }

    private static bool ContainsWord((int X, int Y) coordinate, Direction dir, string word)
    {
        if (word.Length == 0)
        {
            return true;
        }
        
        if (coordinate.X >= Width ||
            coordinate.Y >= Height ||
            coordinate.X < 0 ||
            coordinate.Y < 0)
        {
            return false;
        }

        if (Lines[coordinate.Y][coordinate.X] != word[0])
        {
            return false;
        }
        
        var next = GetNextCoordinate(coordinate, dir);
        return ContainsWord(next, dir, word[1..]);
    }

    private static (int X, int Y) GetNextCoordinate((int X, int Y) coordinate, Direction dir)
    {
        return dir switch
        {
            Direction.Up => (X: coordinate.X, Y: coordinate.Y - 1),
            Direction.UpRight => (X: coordinate.X + 1, Y: coordinate.Y - 1),
            Direction.Right => (X: coordinate.X + 1, Y: coordinate.Y),
            Direction.RightDown => (X: coordinate.X + 1, Y: coordinate.Y + 1),
            Direction.Down => (X: coordinate.X, Y: coordinate.Y + 1),
            Direction.DownLeft => (X: coordinate.X - 1, Y: coordinate.Y + 1),
            Direction.Left => (X: coordinate.X - 1, Y: coordinate.Y),
            Direction.UpLeft => (X: coordinate.X - 1, Y: coordinate.Y - 1),
            _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
        };
    }

    private static bool HasXMas((int X, int Y) coordinate)
    {
        if (Lines[coordinate.Y][coordinate.X] != 'A')
        {
            return false;
        }
        
        var ul = Lines[coordinate.Y-1][coordinate.X-1];
        var ur = Lines[coordinate.Y-1][coordinate.X+1];
        var bl = Lines[coordinate.Y+1][coordinate.X-1];
        var br = Lines[coordinate.Y+1][coordinate.X+1];
        
        if (!(ul == 'M' && br == 'S') && 
            !(ul == 'S' && br == 'M'))
        {
            return false;
        }
        
        if (!(ur == 'M' && bl == 'S') && 
            !(ur == 'S' && bl == 'M'))
        {
            return false;
        }

        return true;
    }

    private enum Direction
    {
        Up,
        UpRight,
        Right,
        RightDown,
        Down,
        DownLeft,
        Left,
        UpLeft
    }
}