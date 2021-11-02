using System.Collections.Generic;
using System.Threading.Tasks;

public class DFS : PathFinder
{
    private Queue<Node> _visualizingNodes;

    private void Start()
    {
        _visualizingNodes = new Queue<Node>();
    }

    public override async Task FindPath()
    {
        if (_grid.StartNode == null || _grid.TargetNode == null)
        {
            return;
        }

        _grid.Clear(false);

        Program.s_IsPathFinding = true;

        DFSAlgorithm(_grid.StartNode);

        await VisualizeSearching();

        await RetracePath();

        Program.s_IsPathFinding = false;
    }

    private bool DFSAlgorithm(Node currentNode)
    {
        if(currentNode == _grid.TargetNode)
        {
            return true;
        }

        currentNode.IsVisited = true;

        if(currentNode != _grid.StartNode)
        {
            _visualizingNodes.Enqueue(currentNode);
        }

        foreach (var n in _grid.GetNodeNeighbours(currentNode))
        {
            if(!n.IsVisited)
            {
                if (n.Type == NodeType.Block)
                {
                    continue;
                }

                n.Parent = currentNode;
                n.DistanceCost = currentNode.DistanceCost + 1;
                
                if(DFSAlgorithm(n))
                {
                    return true;
                }
            }            
        }

        return false;
    }

    private async Task VisualizeSearching()
    {
        while (_visualizingNodes.Count > 0)
        {
            var node = _visualizingNodes.Dequeue();
            SetCellColorByDistance(node, node.DistanceCost);

            await Task.Delay(50);
        }
    }
}
