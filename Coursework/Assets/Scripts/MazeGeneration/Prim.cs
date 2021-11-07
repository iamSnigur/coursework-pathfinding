using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Prim : MazeGenerator
{
    [SerializeField] private Color _emptyNeighbourColor;

    public override async Task StartMazeGeneration()
    {
        Program.s_IsInteractable = false;

        await GenerateMaze();

        Program.s_IsInteractable = true;
    }

    private async Task GenerateMaze()
    {
        List<Node> frontier = new List<Node>();
        _grid.Clear(true, true);

        Node startNode = _grid.GetNode(1, 1);
        startNode.SetType(NodeType.Block, false);
        frontier.AddRange(GetNeighbours(startNode));

        while (frontier.Count > 0)
        {
            Node currentNode = frontier[Random.Range(0, frontier.Count)];
            frontier.Remove(currentNode);

            if(currentNode.Type == NodeType.Block)
            {
                continue;
            }

            currentNode.SetType(NodeType.Block, false);

            List<Node> neighbours = GetNeighboursByType(currentNode, NodeType.Block);

            if (neighbours.Count > 0)
            {
                await Task.Delay(1);

                Node node = neighbours[Random.Range(0, neighbours.Count)];
                node.SetType(NodeType.Block, false);
                _grid.GetNode(node.X - (node.X - currentNode.X) / 2, node.Y - (node.Y - currentNode.Y) / 2).SetType(NodeType.Block, false);

                await Task.Delay(1);
            }

            List<Node> emptyNeighbours = GetNeighboursByType(currentNode, NodeType.Empty);

            foreach (Node n in emptyNeighbours)
            {
                if (frontier.Contains(n))
                {
                    frontier.Remove(n);
                }
            }

            frontier.AddRange(emptyNeighbours);

            foreach (Node n in emptyNeighbours)
            {
                n.SetColor(_emptyNeighbourColor, false);
            }

            await Task.Delay(10);
        }
    }
}