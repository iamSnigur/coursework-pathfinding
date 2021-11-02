using System.Threading.Tasks;
using UnityEngine;

public abstract class PathFinder : MonoBehaviour
{
    [SerializeField] protected NodeGrid _grid;

    private const int _distancePerColor = 15;
    private readonly Color _pathColor = new Color(1f, 0.639f, 0.106f);
    //private readonly Color _pathColor = new Color(0.673f, 0.560f, 0.722f);
    //private readonly Color _pathColor = new Color(0.780f, 0.831f, 0.882f);
    private readonly Color[] _blendColors = 
    { 
        new Color(1f, 0.255f, 0.490f),
        new Color(0.541f, 0.169f, 0.886f),
        new Color(0.470f, 0.843f, 1f), 
        new Color(0.239f, 1f, 0.431f),
        new Color(1f, 0.960f, 0.250f)
    };

    public abstract Task FindPath();

    protected void SetCellColorByDistance(Node node, int distance)
    {
        int colorIndex1 = (distance / _distancePerColor) % _blendColors.Length;
        int colorIndex2 = (colorIndex1 + 1) % _blendColors.Length;
        Color color = Color.Lerp(_blendColors[colorIndex1], _blendColors[colorIndex2], (distance % _distancePerColor) / (float)_distancePerColor); 
        node.SetColor(color, false);
    }

    protected async Task RetracePath()
    {
        if(_grid.TargetNode.Parent == null)
        {
            return;
        }

        var currentNode = _grid.TargetNode;

        while(currentNode != _grid.StartNode)
        {
            currentNode = currentNode.Parent;

            if(currentNode != _grid.StartNode)
            {
                currentNode.SetColor(_pathColor, false);
            }

            await Task.Delay(1);
        }
    }
}