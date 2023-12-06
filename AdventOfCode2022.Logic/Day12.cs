using MoreLinq;

namespace AdventOfCode2022.Logic;

public static class Day12
{
    private static readonly string[] Lines = File.ReadAllLines("Inputs/input_20221212.txt");

    public static string GetPart1Answer()
    {
        var width = Lines[0].Length;
        var height = Lines.Length;
        var nodes = new Node[width, height];
        Node? startingNode = null;
        Node? endingNode = null;
        var unvisitedNodes = new List<Node>();
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var newChar = Lines[y][x];
                var newNode = new Node(x, y, newChar);
                unvisitedNodes.Add(newNode);
                if (newChar == 'S')
                {
                    startingNode = newNode;
                    startingNode.Distance = 0;
                }
                if (newChar == 'E')
                {
                    endingNode = newNode;
                }
                nodes[x, y] = newNode;
                if (x - 1 >= 0)
                {
                    var leftNode = nodes[x - 1, y];
                    if (leftNode.Height - newNode.Height <= 1)
                    {
                        newNode.Connections.Add(new Edge(leftNode, 1));
                    }
                    if (newNode.Height - leftNode.Height <= 1)
                    {
                        leftNode.Connections.Add(new Edge(newNode, 1));
                    }
                }
                if (y - 1 >= 0)
                {
                    var upNode = nodes[x, y - 1];
                    if (upNode.Height - newNode.Height <= 1)
                    {
                        newNode.Connections.Add(new Edge(upNode, 1));
                    }
                    if (newNode.Height - upNode.Height <= 1)
                    {
                        upNode.Connections.Add(new Edge(newNode, 1));
                    }
                }
            }
        }

        var currentNode = startingNode!;
        while (unvisitedNodes.Count > 0)
        {
            foreach (var edge in currentNode.Connections)
            {
                var neighbor = edge.Node;
                var newDistance = currentNode.Distance + edge.Length;
                if (newDistance < neighbor.Distance)
                {
                    neighbor.Distance = newDistance;
                }
            }

            currentNode.Visited = true;
            unvisitedNodes.Remove(currentNode);

            if (endingNode!.Visited)
            {
                break;
            }

            currentNode = unvisitedNodes.MinBy(x => x.Distance);
        }

        return endingNode?.Distance.ToString() ?? "";
    }

    public static string GetPart2Answer()
    {
        var width = Lines[0].Length;
        var height = Lines.Length;
        var nodes = new Node[width, height];
        Node? startingNode = null;
        var unvisitedNodes = new List<Node>();
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var newChar = Lines[y][x];
                var newNode = new Node(x, y, newChar);
                unvisitedNodes.Add(newNode);
                if (newChar == 'E')
                {
                    startingNode = newNode;
                    startingNode.Distance = 0;
                }
                nodes[x, y] = newNode;
                if (x - 1 >= 0)
                {
                    var leftNode = nodes[x - 1, y];
                    if (newNode.Height - leftNode.Height <= 1)
                    {
                        newNode.Connections.Add(new Edge(leftNode, 1));
                    }
                    if (leftNode.Height - newNode.Height <= 1)
                    {
                        leftNode.Connections.Add(new Edge(newNode, 1));
                    }
                }
                if (y - 1 >= 0)
                {
                    var upNode = nodes[x, y - 1];
                    if (newNode.Height - upNode.Height <= 1)
                    {
                        newNode.Connections.Add(new Edge(upNode, 1));
                    }
                    if (upNode.Height - newNode.Height <= 1)
                    {
                        upNode.Connections.Add(new Edge(newNode, 1));
                    }
                }
            }
        }

        var allLowestPoints = unvisitedNodes.Where(x => x.Height == 0).ToArray();
        
        var currentNode = startingNode!;
        while (unvisitedNodes.Count > 0)
        {
            foreach (var edge in currentNode.Connections)
            {
                var neighbor = edge.Node;
                var newDistance = currentNode.Distance + edge.Length;
                if (newDistance < neighbor.Distance)
                {
                    neighbor.Distance = newDistance;
                }
            }

            currentNode.Visited = true;
            unvisitedNodes.Remove(currentNode);

            currentNode = unvisitedNodes.MinBy(x => x.Distance);
            if (currentNode.Distance == int.MaxValue)
                break;
        }

        var lowPointWithShortestDistance = allLowestPoints.MinBy(x => x.Distance);
        return lowPointWithShortestDistance.Distance.ToString();
    }

    private class Node(int x, int y, char heightChar)
    {
        public int X { get; } = x;
        public int Y { get; } = y;
        public char HeightChar { get; } = heightChar;
        public bool Visited { get; set; } = false;
        public int Height { get; } = heightChar switch
        {
            'S' => 0,
            'E' => 'z' - 'a',
            _ => heightChar - 'a'
        };

        public List<Edge> Connections { get; } = new();
        public int Distance { get; set; } = int.MaxValue;

    }

    private class Edge(Node node, int length)
    {
        public Node Node { get; } = node;
        public int Length { get; } = length;
    }
}