namespace AdventOfCode2023.Logic;

public static class Day10
{
    private static readonly string[] Lines = File.ReadAllLines("Inputs/input_20231210.txt");

    public static string GetPart1Answer()
    {
        var pipes = Lines
            .Select(line => line.Select(SymbolToPipe).ToArray())
            .ToArray();
        
        // Find start position
        Coordinate startPipe = default;
        for (var y = 0; y < pipes.Length; y++)
        {
            var row = pipes[y];
            for (var x = 0; x < row.Length; x++)
            {
                if (row[x] == Pipe.Start)
                {
                    startPipe = new Coordinate(x, y);
                }
            }
        }

        var previousPipe = (Coordinate?)null;
        var currentPipe = startPipe;
        var count = 0;
        do
        {
            var nextPipe = FollowPipe(pipes, currentPipe, previousPipe);
            previousPipe = currentPipe;
            currentPipe = nextPipe;
            count++;
        } while (!currentPipe.Equals(startPipe));

        var halfCount = count / 2;
        return halfCount.ToString();
    }

    public static string GetPart2Answer()
    {
        var pipes = Lines
            .Select(line => line.Select(SymbolToPipe).ToArray())
            .ToArray();
        
        // Find start position
        Coordinate startPipe = default;
        for (var y = 0; y < pipes.Length; y++)
        {
            var row = pipes[y];
            for (var x = 0; x < row.Length; x++)
            {
                if (row[x] == Pipe.Start)
                {
                    startPipe = new Coordinate(x, y);
                }
            }
        }

        FixStartSymbol(pipes, startPipe);
        
        // Get positions of all pipes connected to start
        var pipeSet = new HashSet<Coordinate>();
        var previousPipe = (Coordinate?)null;
        var currentPipe = startPipe;
        do
        {
            pipeSet.Add(currentPipe);
            var nextPipe = FollowPipe(pipes, currentPipe, previousPipe);
            previousPipe = currentPipe;
            currentPipe = nextPipe;
        } while (!currentPipe.Equals(startPipe));

        // Check all spots if they are inside or outside
        var insideCount = 0;
        for (var y = 0; y < pipes.Length; y++)
        {
            var inside = false;
            var under = false;
            var row = pipes[y];
            for (var x = 0; x < row.Length; x++)
            {
                var coordinate = new Coordinate(x, y);
                if (pipeSet.Contains(coordinate))
                {
                    var pipe = pipes[y][x];
                    if (pipe == Pipe.NorthSouth)
                    {
                        inside = !inside;
                    }
                    else if (pipe == Pipe.NorthEast)
                    {
                        under = true;
                    }
                    else if (pipe == Pipe.SouthEast)
                    {
                        under = false;
                    }
                    else if (pipe == Pipe.NorthWest)
                    {
                        if (!under)
                        {
                            inside = !inside;
                        }
                    }
                    else if (pipe == Pipe.SouthWest)
                    {
                        if (under)
                        {
                            inside = !inside;
                        }
                    }
                }
                else if (inside)
                {
                    insideCount++;
                }
            }
        }
        
        return insideCount.ToString();
    }

    private static Coordinate FollowPipe(Pipe[][] pipes, Coordinate current, Coordinate? previous)
    {
        var north = pipes[current.Y-1][current.X];
        var south = pipes[current.Y+1][current.X];
        var west = pipes[current.Y][current.X-1];
        var east = pipes[current.Y][current.X+1];
        var northCoordinate = new Coordinate(current.X, current.Y-1);
        var southCoordinate = new Coordinate(current.X, current.Y+1);
        var westCoordinate = new Coordinate(current.X-1, current.Y);
        var eastCoordinate = new Coordinate(current.X+1, current.Y);
        
        var center = pipes[current.Y][current.X];
        return center switch
        {
            Pipe.Start or Pipe.NorthSouth or Pipe.NorthEast or Pipe.NorthWest 
                when north is Pipe.Start or Pipe.NorthSouth or Pipe.SouthEast or Pipe.SouthWest && 
                     (previous == null || !northCoordinate.Equals(previous)) => northCoordinate,
            Pipe.Start or Pipe.NorthSouth or Pipe.SouthEast or Pipe.SouthWest 
                when south is Pipe.Start or Pipe.NorthSouth or Pipe.NorthEast or Pipe.NorthWest && 
                     (previous == null || !southCoordinate.Equals(previous)) => southCoordinate,
            Pipe.Start or Pipe.EastWest or Pipe.NorthWest or Pipe.SouthWest 
                when west is Pipe.Start or Pipe.EastWest or Pipe.NorthEast or Pipe.SouthEast && 
                     (previous == null || !westCoordinate.Equals(previous)) => westCoordinate,
            Pipe.Start or Pipe.EastWest or Pipe.NorthEast or Pipe.SouthEast 
                when east is Pipe.Start or Pipe.EastWest or Pipe.NorthWest or Pipe.SouthWest && 
                     (previous == null || !eastCoordinate.Equals(previous)) => eastCoordinate,
            _ => throw new ArgumentException("Invalid pipes")
        };
    }

    private static void FixStartSymbol(Pipe[][] pipes, Coordinate startPosition)
    {
        var north = pipes[startPosition.Y-1][startPosition.X];
        var south = pipes[startPosition.Y+1][startPosition.X];
        var west = pipes[startPosition.Y][startPosition.X-1];
        var east = pipes[startPosition.Y][startPosition.X+1];

        var northIsConnected = north is Pipe.NorthSouth or Pipe.SouthEast or Pipe.SouthWest;
        var southIsConnected = south is Pipe.NorthEast or Pipe.NorthSouth or Pipe.NorthWest;
        var westIsConnected = west is Pipe.EastWest or Pipe.NorthEast or Pipe.SouthEast;
        var eastIsConnected = east is Pipe.EastWest or Pipe.NorthWest or Pipe.SouthWest;

        if (northIsConnected && southIsConnected)
        {
            pipes[startPosition.Y][startPosition.X] = Pipe.NorthSouth;
        }
        else if (northIsConnected && westIsConnected)
        {
            pipes[startPosition.Y][startPosition.X] = Pipe.NorthWest;
        }
        else if (northIsConnected && eastIsConnected)
        {
            pipes[startPosition.Y][startPosition.X] = Pipe.NorthEast;
        }
        else if (westIsConnected && eastIsConnected)
        {
            pipes[startPosition.Y][startPosition.X] = Pipe.EastWest;
        }
        else if (westIsConnected && southIsConnected)
        {
            pipes[startPosition.Y][startPosition.X] = Pipe.SouthWest;
        }
        else if (southIsConnected && eastIsConnected)
        {
            pipes[startPosition.Y][startPosition.X] = Pipe.SouthEast;
        }
        else
        {
            throw new ArgumentException("Invalid start position");
        }
    }

    private static Pipe SymbolToPipe(char symbol)
    {
        return symbol switch
        {
            '|' => Pipe.NorthSouth,
            '-' => Pipe.EastWest,
            'L' => Pipe.NorthEast,
            'J' => Pipe.NorthWest,
            '7' => Pipe.SouthWest,
            'F' => Pipe.SouthEast,
            '.' => Pipe.Ground,
            'S' => Pipe.Start,
            _ => throw new ArgumentOutOfRangeException(nameof(symbol), symbol, null)
        };
    }

    private enum Pipe
    {
        NorthSouth,
        EastWest,
        NorthEast,
        NorthWest,
        SouthWest,
        SouthEast,
        Ground,
        Start
    }

    private readonly struct Coordinate(int x, int y)
    {
        public int X => x;
        public int Y => y;
        
        public override string ToString()
        {
            return $"{X},{y}";
        }
    }
}