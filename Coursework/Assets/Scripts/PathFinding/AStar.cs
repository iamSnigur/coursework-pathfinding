using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AStar : PathFinder
{
    private const int _diagonalStep = 14;
    private const int _horizontalStep = 10;

    public override async Task FindPath()
    {
        if (_grid.StartNode == null || _grid.TargetNode == null)
        {
            return;
        }

        _grid.Clear(false, false);

        Program.s_IsInteractable = false;

        await AStarAlgorithm();

        await RetracePath();

        Program.s_IsInteractable = true;
    }

    private async Task AStarAlgorithm()
    {
        var openSet = new Heap<Node>(_grid.MaxSize);
        var closedSet = new HashSet<Node>();

        openSet.Add(_grid.StartNode);

        while (openSet.Count > 0)
        {
            var currentNode = openSet.RemoveFirst();
            closedSet.Add(currentNode);

            if(currentNode != _grid.StartNode && currentNode != _grid.TargetNode)
            {
                SetCellColorByDistance(currentNode, currentNode.DistanceCost);
            }

            if (currentNode == _grid.TargetNode)
            {
                return;
            }

            foreach (var neighbour in _grid.GetNodeNeighbours(currentNode))
            {
                if (neighbour.Type == NodeType.Block || closedSet.Contains(neighbour))
                {
                    continue;
                }

                var newMovementCostToNeighbour = currentNode.GCost + GetDistance(currentNode, neighbour);

                if (newMovementCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour))
                {
                    neighbour.GCost = newMovementCostToNeighbour;
                    neighbour.HCost = GetDistance(neighbour, _grid.TargetNode);
                    neighbour.Parent = currentNode;
                    neighbour.DistanceCost = currentNode.DistanceCost + 1;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }

            await Task.Delay(10);
        }
    }

    private int GetDistance(Node nodeA, Node nodeB)
    {
        var dstX = Mathf.Abs(nodeA.X - nodeB.X);
        var dstY = Mathf.Abs(nodeA.Y - nodeB.Y);

        if (dstX > dstY)
        {
            return _diagonalStep * dstY + _horizontalStep * (dstX - dstY);
        }

        return _diagonalStep * dstX + _horizontalStep * (dstY - dstX);
    }
}
