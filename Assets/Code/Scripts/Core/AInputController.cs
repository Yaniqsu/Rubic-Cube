using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AdaptivePerformance;
using UnityEngine.InputSystem;

public abstract class AInputController : MonoBehaviour
{
    private struct InputActionActions
    {
        public Action<InputAction.CallbackContext> startedAction;
        public Action<InputAction.CallbackContext> cancelledAction;
    }
    
    [SerializeField] private InputActionAsset inputActionAsset;
    [SerializeField] private string mapName;

    private Dictionary<string, InputActionActions> _inputActionsEvents = new();

    protected InputActionMap Map { get; private set; }

    private void Awake()
    {
        Map = inputActionAsset.FindActionMap(mapName);
    }

    private void OnEnable()
    {
        EnableInput();
    }

    private void OnDestroy()
    {
        DisableInput();
    }

    private void OnDisable()
    {
        DisableInput();
    }

    private void EnableInput()
    {
        Map.Enable();
        
        EnableInputActions();
    }

    private void DisableInput()
    {
        Map.Disable();

        foreach(var (name, events) in _inputActionsEvents)
        {
            RemoveActionEvents(Map[name], events);
        }
    }

    protected void RegisterInputAction(
        string name, 
        Action<InputAction.CallbackContext> startedAction = null,
        Action<InputAction.CallbackContext> cancelledAction = null)
    {
        var inputAction = Map.FindAction(name);

        if (inputAction == null)
            return;

        var events = new InputActionActions()
        {
            startedAction = startedAction,
            cancelledAction = cancelledAction
        };

        _inputActionsEvents.TryAdd(name, events);
        AddActionEvents(inputAction, events);
    }

    private void AddActionEvents(InputAction inputAction, InputActionActions events)
    {
        if (events.startedAction != null) inputAction.started += events.startedAction;
        if (events.cancelledAction != null) inputAction.canceled += events.cancelledAction;
    }

    private void RemoveActionEvents(InputAction inputAction, InputActionActions events)
    {
        if (events.startedAction != null) inputAction.started -= events.startedAction;
        if (events.cancelledAction != null) inputAction.canceled -= events.cancelledAction;
    }

    protected abstract void EnableInputActions();
}
