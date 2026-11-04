using UnityEngine.Events;
using UnityEngine.InputSystem;
using YNQ.InputField;
using YNQ.Settings.SettingField;

public class InputActionInputController : AInputFieldController<InputBindingSettingField, InputField_KeyMap>
{
    private UnityAction<InputAction> onRebindAction;
    
    public override void ConnectValues(InputBindingSettingField settingField)
    {
        onRebindAction = action => settingField.Set(action.SaveBindingOverridesAsJson());

        inputField.Initialize(settingField.InputAction, settingField.CanRebind);
        inputField.OnRebind.AddListener(onRebindAction);
    }

    public override void Disconnect(InputBindingSettingField settingField)
    {
        inputField.OnRebind.RemoveListener(onRebindAction);        
    }
}
