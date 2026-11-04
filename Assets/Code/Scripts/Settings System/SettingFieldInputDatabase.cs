using System;
using System.Linq;
using UnityEngine;
using YNQ.Settings.InputController;
using YNQ.Settings.SettingField;

[CreateAssetMenu(fileName = "Setting Field Input Database", menuName = "YNQ /Settings /Input Database")]
public class SettingFieldInputDatabase : ScriptableObject
{
    [SerializeField] private SDictionary<string, InputControllerBase> inputControllers = new();

    public AInputController<T> GetInputController<T>() where T : ScriptableSettingFieldBase
    {
        if (inputControllers.TryGetValue(typeof(T).AssemblyQualifiedName, out var controller)
            && controller is AInputController<T> c)
            return c;

        return null;
    }

    public InputControllerBase GetInputController(Type type)
    {
        Debug.Log(type.AssemblyQualifiedName);   
        Debug.Log(string.Join(", ", inputControllers.ToDictionary().Keys));
        
        if (inputControllers.TryGetValue(type.AssemblyQualifiedName, out var controller))
            return controller;

        return null;
    }

    public InputControllerBase[] GetControllers() => inputControllers.Values.ToArray();
}
