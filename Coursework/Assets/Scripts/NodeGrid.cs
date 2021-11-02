using System.Collections.Generic;
using UnityEngine;

public class NodeGrid : MonoBehaviour
{
    private const int _yOffset = 2;

    public int MaxSize => _size.x * _size.y;
    public Node StartNode { get; private set; }
    public Node TargetNode { get; private set; }
    public Node[] Grid { get; private set; }
    public bool IsChangableNode(Node node) => node != StartNode && node != TargetNode;

    [SerializeField] private Node _nodePrefab;
    [SerializeField] private Color _gridColor;

    private Vector2Int _size;
    private float _halfNodeScale => _nodePrefab.transform.localScale.x / 2f;

    public void Initialize(Vector2Int size)
    {
        _size = size;
        Grid = new Node[_size.x * _size.y];
        GenereteGrid();
    }   

    public Node GetNode(int x, int y)
    {   
        return Grid[y * _size.x + x];
    }

    public Node GetNode(RaycastHit2D hit)
    {
        if (hit.collider != null)
        {
            int x = (int)(hit.point.x + _size.x * 0.5f);
            int y = (int)(hit.point.y + _size.y * 0.5f);
           
            if(x >= 0 && x < _size.x && y >= 0 && y < _size.y)
            {
                return GetNode(x, y);
            }
        }

        return null;
    }

    public List<Node> GetNodeNeighbours(Node node)
    {
        var neighbours = new List<Node>();

        for(int x = -1; x <= 1; x++)
        {
            for(int y = -1; y <= 1; y++)
            {
                if((x == 0 && y == 0) || (Mathf.Abs(x) == 1 && Mathf.Abs(y) == 1))
                {
                    continue;
                }

                int tmpX = node.X + x;
                int tmpY = node.Y + y;

                if (tmpX >= 0 && tmpX < _size.x && tmpY >= 0 && tmpY < _size.y)
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

    public void Clear(bool clearBlocks)
    {
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
        var offset = new Vector2((_size.x - 1) * 0.5f, (_size.y - 1) * 0.5f);

        for (int i = 0, y = 0; y < _size.y; y++)
        {
            for (int x = 0; x < _size.x; x++, i++)
            {
                Node node = Grid[i] = Instantiate(_nodePrefab);

                node.Initialize(_gridColor, x, y);

                node.transform.SetParent(transform, false);
                node.transform.localPosition = new Vector2(x - offset.x, y - offset.y);
            }
        }
    }
}
