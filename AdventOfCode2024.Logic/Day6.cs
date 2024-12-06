namespace AdventOfCode2024.Logic;

public static class Day6
{
    private static readonly string[] Lines = File.ReadAllLines("Inputs/input_20241206.txt");
    private static readonly Maze Lab = new(Lines);

    public static string GetPart1Answer()
    {
        // Find start position
        var (visitedStates, _) = Lab.GetVisitedStates();
        var countPositions = visitedStates.DistinctBy(state => (state.X, state.Y)).Count();
        return countPositions.ToString();
    }

    public static string GetPart2Answer()
    {
        var (visitedStates, _) = Lab.GetVisitedStates();
        var visitedPositions = visitedStates.Select(x => (x.X, x.Y)).ToHashSet();
        var countLoops = 0;
        foreach (var visitedPosition in visitedPositions)
        {
            if (Lab.IsObstructed(visitedPosition.X, visitedPosition.Y))
            {
                // Skip obstructed positions (like start)
                continue;
            }

            // Try setting obstruction
            Lab.SetObstructed(visitedPosition.X, visitedPosition.Y);

            var (_, isLoop) = Lab.GetVisitedStates();
            if (isLoop)
            {
                countLoops++;
            }
            
            // Restore state
            Lab.ClearObstruction(visitedPosition.X, visitedPosition.Y);
        }

        return countLoops.ToString();
    }

    private enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    private record GuardState(int X, int Y, Direction Direction);

    private class Maze
    {
        public char[][] PositionContents { get; }
        public int Width { get; }
        public int Height { get; }

        public Maze(string[] lines)
        {
            PositionContents = lines.Select(line => line.ToCharArray()).ToArray();
            Width = lines.First().Length;
            Height = lines.Length;
        }

        public bool IsObstructed(int x, int y)
        {
            return PositionContents[y][x] switch
            {
                '#' or '^' => true,
                _ => false
            };
        }

        public void SetObstructed(int x, int y)
        {
            if (PositionContents[y][x] != '.')
            {
                throw new Exception("The position needs to be clear.");
            }
            
            PositionContents[y][x] = '#';
        }

        public void ClearObstruction(int x, int y)
        {
            if (PositionContents[y][x] != '#')
            {
                throw new Exception("The position needs to be obstructed.");
            }
            
            PositionContents[y][x] = '.';
        }

        public (HashSet<GuardState> Visited, bool IsLoop) GetVisitedStates()
        {
            var guard = Lab.GetGuardStart();
            var visitedStates = new HashSet<GuardState>();
            while (!visitedStates.Contains(guard))
            {
                visitedStates.Add(guard);
                guard = Lab.GetNextState(guard);
                if (guard == null)
                {
                    // We walked outside, the end
                    return (visitedStates, IsLoop: false);
                }
            }
            return (visitedStates, IsLoop: true);
        }

        private GuardState GetGuardStart()
        {
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    if (PositionContents[y][x] == '^')
                    {
                        return new GuardState(x, y, Direction.Up);
                    }
                }
            }
            
            throw new Exception("Maze can't find start position.");
        }

        public GuardState? GetNextState(GuardState current)
        {
            var walkDirection = current.Direction;
            var hitWallsCount = 0;
            while (hitWallsCount < 4)
            {
                var nextPosition = walkDirection switch
                {
                    Direction.Up => (current.X, Y: current.Y - 1),
                    Direction.Down => (current.X, Y: current.Y + 1),
                    Direction.Left => (X: current.X - 1, current.Y),
                    Direction.Right => (X: current.X + 1, current.Y),
                    _ => throw new ArgumentOutOfRangeException()
                };
                if (nextPosition.X < 0 ||
                    nextPosition.X >= Width ||
                    nextPosition.Y < 0 ||
                    nextPosition.Y >= Height)
                {
                    // Walk outside, the end
                    return null;
                }
                if (PositionContents[nextPosition.Y][nextPosition.X] == '#')
                {
                    // Turn 90 deg right
                    hitWallsCount++;
                    walkDirection = TurnRight(walkDirection);
                }
                else
                {
                    return new GuardState(nextPosition.X, nextPosition.Y, walkDirection);
                }
            }

            throw new Exception("Surrounded by walls");
        }

        private Direction TurnRight(Direction direction)
        {
            return direction switch
            {
                Direction.Up => Direction.Right,
                Direction.Down => Direction.Left,
                Direction.Left => Direction.Up,
                Direction.Right => Direction.Down,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }
    }
}