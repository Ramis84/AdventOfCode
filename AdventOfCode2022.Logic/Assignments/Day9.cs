namespace AdventOfCode2022.Logic.Assignments;

public static class Day9
{
    private static readonly string[] Lines = File.ReadAllLines("input_20221209.txt");

    public static string GetPart1Answer()
    {
        var head = new Position(0, 0);
        var tail = new Position(0, 0);
        var tailVisitedCoordinates = new HashSet<Position>();

        foreach (var line in Lines)
        {
            var direction = line[0];
            var moveCount = int.Parse(line[2..]);

            while (moveCount-- > 0)
            {
                head = head.Move(direction);
                tail = tail.Follow(head);
                tailVisitedCoordinates.Add(tail);
            }
        }

        var tailVisitedCoordinatesCount = tailVisitedCoordinates.Count;
        return tailVisitedCoordinatesCount.ToString();
    }

    public static string GetPart2Answer()
    {
        var snake = new Position[10];
        var tailVisitedCoordinates = new HashSet<Position>();
        foreach (var line in Lines)
        {
            var direction = line[0];
            var moveCount = int.Parse(line[2..]);

            while (moveCount-- > 0)
            {
                snake[0] = snake[0].Move(direction);

                for (int i = 1; i < snake.Length; i++)
                {
                    snake[i] = snake[i].Follow(snake[i - 1]);
                }
                tailVisitedCoordinates.Add(snake[9]);
            }
        }

        var tailVisitedCoordinatesCount = tailVisitedCoordinates.Count;
        return tailVisitedCoordinatesCount.ToString();
    }

    public struct Position(int x, int y)
    {
        public int X => x;
        public int Y => y;

        public Position Follow(Position otherPosition)
        {
            var differenceX = otherPosition.X - x;
            var differenceY = otherPosition.Y - y;

            if (Math.Abs(differenceX) <= 1 &&
                Math.Abs(differenceY) <= 1)
            {
                return this; // Unchanged
            }

            var moveX = differenceX == 0 ? 0 : Math.Sign(differenceX);
            var moveY = differenceY == 0 ? 0 : Math.Sign(differenceY);

            return new Position(x + moveX, y + moveY);
        }

        public Position Move(char direction)
        {
            switch (direction)
            {
                case 'R':
                    return new Position(x+1, y);
                case 'L':
                    return new Position(x-1, y);
                case 'U':
                    return new Position(x, y+1);
                case 'D':
                    return new Position(x, y-1);
                default:
                    throw new ArgumentException("Unknown direction", nameof(direction));
            }
        }
    }
}