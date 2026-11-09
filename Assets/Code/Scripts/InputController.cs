using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputController : AInputController
{
    [SerializeField] private string rotateName;
    [SerializeField] private string mouseDelta;
    [SerializeField] private string interactName;
    [SerializeField] private string scrollName;
    [SerializeField] private string undoName;

    public UnityEvent<Vector2> onRotateObject;
    public UnityEvent<Vector2> onMove;
    public UnityEvent<float> onScroll;
    public UnityEvent onMoveStart;
    public UnityEvent onMoveEnd;
    public UnityEvent onUndo;
    private bool _rpm;
    private bool _lpm;

    private void Update()
    {
        CheckRotation();

        CheckScroll();
    }

    protected override void EnableInputActions()
    {
        RegisterInputAction(rotateName, _ => _rpm = true, _ => _rpm = false);
        RegisterInputAction(interactName, CallOnInteractStart, CallOnInteractEnd);
        RegisterInputAction(undoName, _ => onUndo.Invoke());
    }

    private void CallOnInteractStart(InputAction.CallbackContext context)
    {
        _lpm = true;
        onMoveStart.Invoke();
    }
    private void CallOnInteractEnd(InputAction.CallbackContext context)
    {
        _lpm = false;
        onMoveEnd.Invoke();
    }

    private void CheckRotation()
    {
        var val = Map[mouseDelta].ReadValue<Vector2>();

        if (_rpm)
        {
            onRotateObject.Invoke(val);
        }
        
        if(_lpm)
        {
            onMove.Invoke(val);
        }
    }

    private void CheckScroll()
    {
        var val = Map[scrollName].ReadValue<Vector2>();

        onScroll.Invoke(val.y);
    }
}
