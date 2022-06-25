using System.Collections;
using UnityEngine;

public class Node : MonoBehaviour, IHeapItem<Node>
{
    public Color EmptyColor => _emptyColor;
    public Color BlockColor => _blockColor;
    public Color StartColor => _startColor;
    public Color TargetColor => _targetColor;

    public NodeType Type { get; private set; }
    public Node Parent;
    public bool IsVisited;
    public int X { get; private set; }
    public int Y { get; private set; }
    public int HeapIndex
    {
        get
        {
            return _heapIndex;
        }

        set
        {
            _heapIndex = value;
        }
    }
    public int GCost;
    public int HCost;
    public int FCost => GCost + HCost;
    public int DistanceCost;

    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Color _emptyColor;
    [SerializeField] private Color _blockColor;
    [SerializeField] private Color _startColor;
    [SerializeField] private Color _targetColor;    

    private Color _color
    {
        get
        {
            return _spriteRenderer.color;
        }

        set
        {
            _spriteRenderer.color = value;
        }
    }
    private Color _previosColor;
    private int _heapIndex;

    public void Initialize(Color gridColor, int x, int y)
    {
        X = x;
        Y = y;
        Type = NodeType.Empty;
        _color = gridColor;
        _previosColor = gridColor;
    }

    public void SetType(NodeType type, bool forceChange)
    {
        Type = type;

        var newColor = new Color();

        switch(type)
        {
            case NodeType.Empty:
                newColor = _emptyColor;
                break;
            case NodeType.Block:
                newColor = _blockColor;
                break;
            case NodeType.Start:
                newColor = _startColor;
                break;
            case NodeType.Target:
                newColor = _targetColor;
                break;
        }

        SetColor(newColor, forceChange);
    }

    public void SetColor(Color color, bool forceChange)
    {
        StopAllCoroutines();

        if(forceChange)
        {
            _color = color;
            _previosColor = color;
        }
        else
        {
            StartCoroutine(BlendColor(color));
        }
    }

    public int CompareTo(Node other)
    {
        var compare = FCost.CompareTo(other.FCost);

        if (compare == 0)
        {
            compare = HCost.CompareTo(other.HCost);
        }

        return -compare;
    }

    private IEnumerator BlendColor(Color color)
    {
        float colorDelta = 0f;

        while (!(colorDelta >= 1f))
        {
            colorDelta += 0.006f;
            colorDelta = Mathf.Clamp01(colorDelta);
            _color = Color.Lerp(_previosColor, color, colorDelta);

            yield return new WaitForSeconds(0.001f);
        }

        _previosColor = color;
    }      
}
