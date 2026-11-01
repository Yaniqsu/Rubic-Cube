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

    public UnityEvent<Vector2> onRotate;
    private InputActionMap _map;
    private bool _rpm;

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
    }

    private void EnableInput()
    {
        inputActions.Enable();

        _map = inputActions.FindActionMap("Camera");

        _map[rotateName].started += CallOnRPMDown;
        _map[rotateName].canceled += CallOnRPMUp;
    }

    private void DisableInput()
    {
        inputActions.Disable();

        _map[rotateName].started -= CallOnRPMDown;
        _map[rotateName].canceled -= CallOnRPMUp;
    }

    private void CallOnRPMDown(InputAction.CallbackContext context) => _rpm = true;
    private void CallOnRPMUp(InputAction.CallbackContext context) => _rpm = false;

    private void CheckRotation()
    {
        if (!_rpm)
            return;
        
        var val = _map[mouseDelta].ReadValue<Vector2>();

        onRotate.Invoke(val);
    }
}
