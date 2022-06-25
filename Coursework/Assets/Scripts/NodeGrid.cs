using System.Collections.Generic;
using UnityEngine;

public class NodeGrid : MonoBehaviour
{
    public int MaxSize => Size.x * Size.y;
    public Node StartNode { get; private set; }
    public Node TargetNode { get; private set; }
    public Node[] Grid { get; private set; }
    public Vector2Int Size { get; private set; }
    public bool IsChangableNode(Node node) => node != StartNode && node != TargetNode;

    [SerializeField] private Node _nodePrefab;
    [SerializeField] private Color _gridColor;

    public void Initialize(Vector2Int size)
    {
        Size = size;
        Grid = new Node[Size.x * Size.y];
        GenereteGrid();
    }   

    public Node GetNode(int x, int y)
    {   
        return Grid[y * Size.x + x];
    }

    public Node GetNode(RaycastHit2D hit)
    {
        if (hit.collider != null)
        {
            int x = (int)(hit.point.x + Size.x * 0.5f);
            int y = (int)(hit.point.y + Size.y * 0.5f);

            if(x >= 0 && x < Size.x && y >= 0 && y < Size.y)
            {
                return GetNode(x, y);
            }
        }

        return null;
    }

    public List<Node> GetNodeNeighbours(Node node)
    {
        var neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if ((x == 0 && y == 0) || (Mathf.Abs(x) == 1 && Mathf.Abs(y) == 1))
                {
                    continue;
                }

                int tmpX = node.X + x;
                int tmpY = node.Y + y;

                if (tmpX >= 0 && tmpX < Size.x && tmpY >= 0 && tmpY < Size.y)
                {
                    neighbours.Add(GetNode(tmpX, tmpY));
                }
            }
        }

        return neighbours;
    }

    public void SetStartNode(Node start)
    {
        if (start == TargetNode)
        {
            TargetNode = null;
        }

        StartNode?.SetType(NodeType.Empty, true);
        StartNode = start;
        start?.SetType(NodeType.Start, false);
    }

    public void SetTargetNode(Node target)
    {
        if(target == StartNode)
        {
            StartNode = null;
        }

        TargetNode?.SetType(NodeType.Empty, true);
        TargetNode = target;
        target?.SetType(NodeType.Target, false);
    }

    public void Clear(bool clearBlocks, bool clearStartTarget)
    {
        if(clearStartTarget)
        {
            SetStartNode(null);
            SetTargetNode(null);
        }        

        foreach (Node n in Grid)
        {
            n.Parent = null;
            n.IsVisited = false;

            if (n.Type == NodeType.Empty || (n.Type == NodeType.Block && clearBlocks))
            {
                n.SetType(NodeType.Empty, true);
            }
        }
    }

    private void GenereteGrid()
    {
        var offset = new Vector2((Size.x - 1) * 0.5f, (Size.y - 1) * 0.5f);

        for (int i = 0, y = 0; y < Size.y; y++)
        {
            for (int x = 0; x < Size.x; x++, i++)
            {
                Node node = Grid[i] = Instantiate(_nodePrefab);

                node.Initialize(_gridColor, x, y);

                node.transform.SetParent(transform, false);
                node.transform.localPosition = new Vector2(x - offset.x, y - offset.y);
            }
        }
    }
}