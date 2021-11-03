using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Program : MonoBehaviour
{
    public static bool s_IsPathFinding;

    [SerializeField] private Camera _camera;
    [SerializeField] private NodeGrid _grid;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Vector2Int _boardSize;
    [SerializeField] private PathFinder[] _pathFinders;

    private Ray _touchRay => _camera.ScreenPointToRay(Input.mousePosition);
    private Node _node => _grid.GetNode(Physics2D.GetRayIntersection(_touchRay, Mathf.Infinity, _layerMask));
    private int _pathFinderIndex;

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
        if (Input.GetMouseButton(0) && _grid.IsChangableNode(_node) && !IsPointerOverUI())
        {
            _node?.SetType(NodeType.Block, false);
        }
    }

    private void HandleRightClick()
    {
        if (Input.GetMouseButton(1) && _grid.IsChangableNode(_node) && !IsPointerOverUI())
        {
            _node?.SetType(NodeType.Empty, true);
        }
    }

    private void HandleMiddleClick()
    {
        if (Input.GetMouseButtonDown(2) && !IsPointerOverUI())
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

    private bool IsPointerOverUI()
    {
        var pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, results);

        return results.Count > 0;
    }
}
