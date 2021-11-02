using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public UnityEvent<int> OnPathFinderChanged;

    [SerializeField] private Dropdown _pathFinders;

    public void OnValueChnaged()
    {
        OnPathFinderChanged?.Invoke(_pathFinders.value);
    }
}
