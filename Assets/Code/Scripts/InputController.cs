using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private string rotateName;
    [SerializeField] private string mouseDelta;
    [SerializeField] private string interactName;
    [SerializeField] private string scrollName;

    public UnityEvent<Vector2> onRotateObject;
    public UnityEvent<Vector2> onMove;
    public UnityEvent<float> onScroll;
    public UnityEvent onMoveStart;
    public UnityEvent onMoveEnd;
    private InputActionMap _map;
    private bool _rpm;
    private bool _lpm;

    private void OnEnable()
    {
        EnableInput();   
    }

    private void OnDestroy()
    {
        DisableInput();
    }

    private void Update()
    {
        CheckRotation();

        CheckScroll();
    }

    private void EnableInput()
    {
        inputActions.Enable();

        _map = inputActions.FindActionMap("Camera");

        _map[rotateName].started += CallOnRPMDown;
        _map[rotateName].canceled += CallOnRPMUp;

        _map[interactName].started += CallOnInteractStart;
        _map[interactName].canceled += CallOnInteractEnd;
    }

    private void DisableInput()
    {
        inputActions.Disable();

        _map[rotateName].started -= CallOnRPMDown;
        _map[rotateName].canceled -= CallOnRPMUp;

        _map[interactName].started -= CallOnInteractStart;
        _map[interactName].canceled -= CallOnInteractEnd;
    }

    private void CallOnRPMDown(InputAction.CallbackContext context) => _rpm = true;
    private void CallOnRPMUp(InputAction.CallbackContext context) => _rpm = false;
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
        var val = _map[mouseDelta].ReadValue<Vector2>();

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
        var val = _map[scrollName].ReadValue<Vector2>();

        onScroll.Invoke(val.y);
        Debug.Log(val.y);
    }
}
