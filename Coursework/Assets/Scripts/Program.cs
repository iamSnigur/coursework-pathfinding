using UnityEngine;

public class Program : MonoBehaviour
{
    public static bool s_IsPathFinding;

    [SerializeField] private Camera _camera;
    [SerializeField] private NodeGrid _grid;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Vector2Int _boardSize;
    [SerializeField] private PathFinder[] _pathFinders;

    private Vector2 _touchPosition => _camera.ScreenToWorldPoint(Input.mousePosition);
    private Node _node => _grid.GetNode(Physics2D.Raycast(_touchPosition, Vector2.zero, Mathf.Infinity, _layerMask));
    private int _pathFinderIndex;

    private void Start()
    {
        _grid.Initialize(_boardSize);
    }

    private void Update()
    {  
        if(!s_IsPathFinding)
        {   
            HandleLeftClick();
            HandleMiddleClick();
            HandleRightClick();
        }             
    }

    private void HandleLeftClick()
    {
        if (Input.GetMouseButton(0) && _grid.IsChangableNode(_node))
        {
            _node?.SetType(NodeType.Block, false);
        }
    }

    private void HandleRightClick()
    {
        if (Input.GetMouseButton(1) && _grid.IsChangableNode(_node))
        {
            _node?.SetType(NodeType.Empty, true);
        }
    }

    private void HandleMiddleClick()
    {
        if (Input.GetMouseButtonDown(2))
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                _grid.SetStartNode(_node);
            }
            else
            {
                _grid.SetTargetNode(_node);
            }            
        }
    }

    public void ClearGrid()
    {
        if (!s_IsPathFinding)
        {
            _grid.Clear(true);
        }        
    }

    public void StartPathFinding()
    {
        if (!s_IsPathFinding)
        {
            _pathFinders[_pathFinderIndex].FindPath();
        }
    }

    public void ChangePathFinder(int index)
    {
        _pathFinderIndex = index;
    }
}
