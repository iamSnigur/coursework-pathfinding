using UnityEngine;
using UnityEngine.Tilemaps;

public class HandleInput : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask _mask;
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private Tile _tmp;
    [SerializeField] private Color _tileColor;

    private Vector3 _touch => _camera.ScreenToWorldPoint(Input.mousePosition);

    private void Update()
    {
        HandleTouch();
    }

    private void HandleTouch()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(_touch, Vector2.zero, Mathf.Infinity, _mask);
            
            if (hit.collider != null)
            {                
                Vector3Int cell = _tilemap.WorldToCell(_touch);
                ChangeTile(cell);
            }
        }
    }

    public delegate void OnColorChange(Vector3Int pos, Color color);

    private void ChangeTile(Vector3Int cell)
    {
        //Tile tile = ScriptableObject.CreateInstance("Node");
        //_tilemap.SetTile(new Vector3Int(cell.x, cell.y, 0), tile);
        //OnColorChange on = _tilemap.SetColor;
        //Node n = (Node)_tilemap.GetTile(new Vector3Int(cell.x, cell.y, 0));
        //StartCoroutine(n.BlendColor(on, _tileColor, cell.x, cell.y));
    }
}
