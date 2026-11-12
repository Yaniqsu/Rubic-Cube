using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInputController : AInputController
{
    private enum MoveMode
    {
        MoveSegment = 0,
        RotateCube = 1
    }
    
    [SerializeField] private string w;
    [SerializeField] private string s;
    [SerializeField] private string a;
    [SerializeField] private string d;

    [SerializeField] private string leftArrow;
    [SerializeField] private string rightArrow;
    [SerializeField] private string upArrow;
    [SerializeField] private string downArrow;

    [SerializeField] private string tiltLeft;
    [SerializeField] private string tiltRight;

    [SerializeField] private string toggleMode;

    private MoveMode _currentMode = MoveMode.MoveSegment;

    public UnityEvent<Vector3Int, bool> OnMoveCube;
    public UnityEvent<Vector3> OnMoveSegment;
    public UnityEvent<Vector3> OnMoveSelectedElement;

    public UnityEvent OnTiltLeft;
    public UnityEvent OnTiltRight;
    
    protected override void EnableInputActions()
    {
        RegisterInputAction(toggleMode, _ => _currentMode = MoveMode.RotateCube, _ => _currentMode = MoveMode.MoveSegment);

        RegisterInputAction(w, _ => OnWSADClicked(Vector3Int.up));
        RegisterInputAction(s, _ => OnWSADClicked(Vector3Int.down));
        RegisterInputAction(a, _ => OnWSADClicked(Vector3Int.left));
        RegisterInputAction(d, _ => OnWSADClicked(Vector3Int.right));

        RegisterInputAction(upArrow, _ => OnArrowClicked(Vector3Int.up));
        RegisterInputAction(downArrow, _ => OnArrowClicked(Vector3Int.down));
        RegisterInputAction(leftArrow, _ => OnArrowClicked(Vector3Int.left));
        RegisterInputAction(rightArrow, _ => OnArrowClicked(Vector3Int.right));

        RegisterInputAction(tiltLeft, _ => OnTiltLeft.Invoke());
        RegisterInputAction(tiltRight, _ => OnTiltRight.Invoke());
        
    }

    private void OnWSADClicked(Vector3Int direction)
    {
        switch (_currentMode)
        {
            case MoveMode.MoveSegment:
                OnMoveSegment.Invoke(direction);
                break;
            case MoveMode.RotateCube:
                OnMoveCube.Invoke(direction, true);
                break;
        }
    }

    private void OnArrowClicked(Vector3Int direction)
    {
        switch (_currentMode)
        {
            case MoveMode.MoveSegment:
                OnMoveSelectedElement.Invoke(direction);
                break;
            case MoveMode.RotateCube:
                OnMoveCube.Invoke(direction, false);
                break;
        }
    }
}
