namespace AdventOfCode2022.Logic;

public static class Day14
{
    private static readonly string[] Lines = File.ReadAllLines("Inputs/input_20221214.txt");
    private static readonly RockEdges[] Rocks = Lines.Select(line => new RockEdges(line)).ToArray();
    private static readonly Coordinate SandSpawnPoint = new(500, 0);

    public static string GetPart1Answer()
    {
        var cave = new World();
        foreach (var rock in Rocks)
        {
            cave.Draw(rock);
        }

        bool landed;
        do
        {
            landed = cave.ProduceSand(SandSpawnPoint);
        } while (landed);

        var sandCount = cave.Pixels.Values.Count(x => x == PixelState.Sand);
        return sandCount.ToString();
    }

    public static string GetPart2Answer()
    {
        var cave = new World();
        foreach (var rock in Rocks)
        {
            cave.Draw(rock);
            cave.BottomIsFloor = true;
        }

        var sandSpawnPoint = new Coordinate(500, 0);
        bool landed;
        do
        {
            landed = cave.ProduceSand(sandSpawnPoint);
        } while (landed);

        var sandCount = cave.Pixels.Values.Count(x => x == PixelState.Sand);
        return sandCount.ToString();
    }

    private enum PixelState
    {
        Air = 0,
        Rock,
        Sand
    }

    private class RockEdges
    {
        public Coordinate[] Vertices { get; }

        public RockEdges(string line)
        {
            var segments = line.Split(" -> ");
            Vertices = segments.Select(x => new Coordinate(x)).ToArray();
        }
    }

    private class World
    {
        public readonly Dictionary<Coordinate, PixelState> Pixels = new();

        public int BottomY { get; private set; }
        
        public bool BottomIsFloor { get; set; }

        public void Draw(RockEdges rocks)
        {
            var previousVertex = rocks.Vertices[0];
            Pixels[previousVertex] = PixelState.Rock;
            for (var i = 1; i < rocks.Vertices.Length; i++)
            {
                var currentVertex = rocks.Vertices[i];
                foreach (var point in previousVertex.GetAllPointsAlong(currentVertex))
                {
                    Pixels[point] = PixelState.Rock;
                    BottomY = int.Max(BottomY, point.Y + 2);
                }

                previousVertex = currentVertex;
            }
        }

        public bool ProduceSand(Coordinate sandSpawnPoint)
        {
            var spawnPointState = GetState(sandSpawnPoint);
            if (spawnPointState != PixelState.Air)
                return false;
            
            var current = sandSpawnPoint;
            while (current.Y < BottomY)
            {
                var down = current.Add(0, 1);
                var downPixel = GetState(down);
                if (downPixel == PixelState.Air)
                {
                    current = down;
                    continue;
                }

                var downLeft = current.Add(-1, 1);
                var downLeftPixel = GetState(downLeft);
                if (downLeftPixel == PixelState.Air)
                {
                    current = downLeft;
                    continue;
                }

                var downRight = current.Add(1, 1);
                var downRightPixel = GetState(downRight);
                if (downRightPixel == PixelState.Air)
                {
                    current = downRight;
                    continue;
                }

                Pixels[current] = PixelState.Sand;
                return true;
            }
            
            return false; // Into the abyss
        }

        private PixelState GetState(Coordinate point)
        {
            if (point.Y >= BottomY && BottomIsFloor)
            {
                return PixelState.Rock;
            }
            
            var state = Pixels.GetValueOrDefault(point);
            return state;
        }
    }

    private readonly struct Coordinate
    {
        public int X { get; }
        public int Y { get; }

        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Coordinate(string commaSeparated)
        {
            var segments = commaSeparated.Split(',');
            X = int.Parse(segments[0]);
            Y = int.Parse(segments[1]);
        }

        public Coordinate Add(int offsetX, int offsetY)
        {
            return new Coordinate(X + offsetX, Y + offsetY);
        }

        public IEnumerable<Coordinate> GetAllPointsAlong(Coordinate other)
        {
            if (X != other.X && Y != other.Y)
                throw new ArgumentException("Only supports getting vertices along a straight line");
            
            if (X == other.X)
            {
                var minY = int.Min(Y, other.Y);
                var maxY = int.Max(Y, other.Y);
                for (var y = minY; y <= maxY; y++)
                {
                    yield return new Coordinate(X, y);
                }
            }
            else if (Y == other.Y)
            {
                var minX = int.Min(X, other.X);
                var maxX = int.Max(X, other.X);
                for (var x = minX; x <= maxX; x++)
                {
                    yield return new Coordinate(x, Y);
                }
            }
        }

        public override string ToString()
        {
            return $"{X},{Y}";
        }
    }
}