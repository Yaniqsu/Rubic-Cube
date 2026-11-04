using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using YNQ.Settings.SettingField;

public class ControlsRuntimeManager: ASettingsRuntimeManager
{
    [SerializeField] private InputBindingSettingField[] bindings;

    private List<Action<string>> _actions = new();

    protected override void Connect()
    {
        foreach(var b in bindings)
        {
            Action<string> action = (value) => b.InputAction.action.LoadBindingOverridesFromJson(value);
            b.OnValueChanged += action;
        }
    }

    protected override void Disconnect()
    {
        foreach (var b in bindings)
        {
            foreach(var a in _actions)
                b.OnValueChanged -= a;
        }
        _actions.Clear();
    }
}
