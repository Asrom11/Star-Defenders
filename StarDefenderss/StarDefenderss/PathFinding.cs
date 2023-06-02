using System;
using System.Collections.Generic;
using System.Linq;

namespace StarDefenderss;

public static class PathFinding
{
    public static List<Node> AStar(Node startNode, Node endNode)
    {
        var openNodes  = new HashSet<Node>() { startNode };
        var closedNodes  = new HashSet<Node>();
        startNode.MovementCost = 0;
        startNode.TotalCost = startNode.MovementCost + Heuristic(startNode, endNode);

        while (openNodes .Count > 0)
        {
            var currentNode = openNodes .OrderBy(n => n.TotalCost).First();

            if (currentNode == endNode)
                return ReconstructPath(currentNode);

            openNodes .Remove(currentNode);
            closedNodes .Add(currentNode);

            foreach (var neighbor in currentNode.Neighbors)
            {
                if (closedNodes .Contains(neighbor))
                    continue;

                var pathCost = currentNode.MovementCost + Distance(currentNode, neighbor);

                if (!openNodes .Contains(neighbor))
                    openNodes .Add(neighbor);
                else if (pathCost >= neighbor.MovementCost)
                    continue;

                neighbor.PreviousNode = currentNode;
                neighbor.MovementCost = pathCost;
                neighbor.TotalCost = neighbor.MovementCost + Heuristic(neighbor, endNode);
            }
        }

        return null;
    }

    private  static List<Node> ReconstructPath(Node node)
    {
        var path = new List<Node> { node };

        while (node.PreviousNode != null)
        {
            node = node.PreviousNode;
            path.Insert(0, node);
        }

        return path;
    }

    private static double Distance(Node firstNode, Node secondNode)
    {
        return Math.Sqrt(Math.Pow(firstNode.X - secondNode.X, 2) + Math.Pow(firstNode.Y - secondNode.Y, 2));
    }

    private  static  double Heuristic(Node firstNode, Node secondNode)
    {
        return Distance(firstNode, secondNode);
    }
}
public class Node
{
    public int X { get; set; }
    public int Y { get; set; }
    public double MovementCost { get; set; }
    public double TotalCost { get; set; }
    public Node PreviousNode { get; set; }
    public HashSet<Node> Neighbors { get; set; }

    public Node(int x, int y)
    {
        X = x;
        Y = y;
        Neighbors = new HashSet<Node>();
    }
}