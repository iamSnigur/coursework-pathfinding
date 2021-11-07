using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public abstract class MazeGenerator : MonoBehaviour
{
    [SerializeField] protected NodeGrid _grid;

    public abstract Task StartMazeGeneration();

    protected List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();
        int x = node.X;
        int y = node.Y;

        if (x >= 2)
        {
            neighbours.Add(_grid.GetNode(x - 2, y));
        }

        if (x < _grid.Size.x - 2)
        {
            neighbours.Add(_grid.GetNode(x + 2, y));
        }

        if (y >= 2)
        {
            neighbours.Add(_grid.GetNode(x, y));
        }

        if (y < _grid.Size.y - 2)
        {
            neighbours.Add(_grid.GetNode(x, y + 2));
        }

        return neighbours;
    }

    protected List<Node> GetNeighboursByType(Node node, NodeType type)
    {
        List<Node> neighbours = new List<Node>();
        int x = node.X;
        int y = node.Y;

        if (x >= 2)
        {
            Node n = _grid.GetNode(x - 2, y);
            if (n.Type == type)
            { 
                neighbours.Add(n);
            }
        }

        if (x < _grid.Size.x - 2)
        {
            Node n = _grid.GetNode(x + 2, y);
            if (n.Type == type)
            {
                neighbours.Add(n);
            }
        }

        if (y >= 2)
        {
            Node n = _grid.GetNode(x, y - 2);
            if (n.Type == type)
            {
                neighbours.Add(n);
            }
        }

        if (y < _grid.Size.y - 2)
        {
            Node n = _grid.GetNode(x, y + 2);
            if (n.Type == type)
            {
                neighbours.Add(n);
            }
        }

        return neighbours;
    }
}
