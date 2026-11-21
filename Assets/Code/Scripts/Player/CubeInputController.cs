using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CubeInputController : AInputController
{
    private enum MoveMode
    {
        MoveSegment = 0,
        RotateCube = 1
    }
    
    [SerializeField] private string rotate;

    [SerializeField] private string moveVertical;
    [SerializeField] private string moveHorizontal;

    [SerializeField] private string tilt;

    [SerializeField] private string toggleMode;

    private MoveMode _currentMode = MoveMode.MoveSegment;
    private Vector2Int _arrowsDirection;

    public UnityEvent<Vector2Int, bool> OnMoveCube;
    public UnityEvent<Vector2> OnMoveSegment;
    public UnityEvent<Vector2> OnMoveSelectedElement;

    public UnityEvent<float, bool> OnTilt;
    
    protected override void EnableInputActions()
    {
        RegisterInputAction(toggleMode, _ => _currentMode = MoveMode.RotateCube, _ => _currentMode = MoveMode.MoveSegment);

        RegisterInputAction(rotate, OnRotateStarted);

        RegisterInputAction(moveVertical, OnMoveVerticalStarted);
        RegisterInputAction(moveHorizontal, OnMoveHorizontalStarted);

        RegisterInputAction(tilt, CallOnTilt);
    }

    private void Update()
    {
        if(_currentMode == MoveMode.RotateCube)
        {
            var vertical = Map[moveVertical].ReadValue<float>();
            var horizontal = Map[moveHorizontal].ReadValue<float>();

            var direction = new Vector2(horizontal, vertical).ToVector2Int();

            if(direction.magnitude > 0)
                OnMoveCube.Invoke(direction, false);
        }
    }

    private void OnRotateStarted(InputAction.CallbackContext context)
    {
        var inputValue = context.ReadValue<Vector2>();
        var direction = inputValue.ToVector2Int();

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

    private void CallOnTilt(InputAction.CallbackContext context)
    {
        var direction = context.ReadValue<float>();
        
        OnTilt.Invoke(direction, true);
    }

    private void OnMoveVerticalStarted(InputAction.CallbackContext context)
    {
        var inputValue = context.ReadValue<float>();
        var direction = new Vector2(0, inputValue).ToVector2Int();
        
        switch (_currentMode)
        {
            case MoveMode.MoveSegment:
                OnMoveSelectedElement.Invoke(direction);
                break;
        }
    }

    private void OnMoveHorizontalStarted(InputAction.CallbackContext context)
    {
        var inputValue = context.ReadValue<float>();
        var direction = new Vector2(inputValue, 0).ToVector2Int();

        switch (_currentMode)
        {
            case MoveMode.MoveSegment:
                OnMoveSelectedElement.Invoke(direction);
                break;
        }
    }
}
