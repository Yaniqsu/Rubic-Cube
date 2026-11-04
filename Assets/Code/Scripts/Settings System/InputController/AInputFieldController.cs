using UnityEngine;
using YNQ.InputField;
using YNQ.Settings.InputController;
using YNQ.Settings.SettingField;

public abstract class AInputFieldController<T, TInputField> : AInputController<T> 
    where T: ScriptableSettingFieldBase
    where TInputField : AInputField
{
    [SerializeField] protected TInputField inputField;

    private void Awake()
    {
        inputField.OnEnter += CallOnEnter;
        inputField.OnExit += CallOnExit;
    }

    public override void Connect(T settingField)
    {
        inputField.SetName(settingField.GetName());
        ConnectValues(settingField);
    }

    public abstract void ConnectValues(T settingField);
}
