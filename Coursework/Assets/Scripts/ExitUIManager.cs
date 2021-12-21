using UnityEngine;

public class ExitUIManager : MonoBehaviour
{
    private const string _isHidden = "IsHidden";

    [SerializeField] private Animator _animator;

    public void CloseExitMenu()
    {
        _animator.SetBool(_isHidden, true);
    }

    public void ExitApplication()
    {
        Application.Quit();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            _animator.SetBool(_isHidden, false);
    }
}
