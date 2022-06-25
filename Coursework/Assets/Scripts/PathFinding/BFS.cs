using System.Collections.Generic;
using System.Threading.Tasks;

public class BFS : PathFinder
{
    public override async Task FindPath()
    {
        if (_grid.StartNode == null || _grid.TargetNode == null)
        {
            return;
        }

        _grid.Clear(false, false);

        StopAllCoroutines();

        Program.s_IsInteractable = false;

        await BFSAlgorithm();

        await RetracePath();

        Program.s_IsInteractable = true;
    }

    private async Task BFSAlgorithm()
    {     
        Queue<Node> nodeQueue = new Queue<Node>();
        nodeQueue.Enqueue(_grid.StartNode);

        int currentDistance = 0;

        while (nodeQueue.Count > 0)
        {
            Node node = nodeQueue.Dequeue();
            node.IsVisited = true;

            if (node == _grid.TargetNode)
            {
                break;
            }

            List<Node> neighbours = _grid.GetNodeNeighbours(node);

            if (neighbours.Count == 0)
            {
                continue;
            }

            foreach (var n in neighbours)
            {
                if (!n.IsVisited)
                {
                    if (n.Type == NodeType.Block)
                    {
                        continue;
                    }

                    n.IsVisited = true;
                    n.Parent = node;
                    n.DistanceCost = node.DistanceCost + 1;
                    nodeQueue.Enqueue(n);

                    if (n != _grid.TargetNode)
                    {
                        SetCellColorByDistance(n, n.DistanceCost);
                    }
                }
            }

            if (currentDistance < node.DistanceCost)
            {
                currentDistance++;
                await Task.Delay(50);
            }
        }
    }
}
