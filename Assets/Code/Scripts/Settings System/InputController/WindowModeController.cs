using UnityEngine;
using YNQ.InputField;
using YNQ.Settings.SettingField;

namespace YNQ.Settings.InputController
{
	public class WindowModeController : AInputFieldController<SelectWindowModeSettingField, InputField_WindowModeSpinner>
	{
        public override void ConnectValues(SelectWindowModeSettingField settingField)
        {
            inputField.SetIndex((int)settingField.Value);
            
            inputField.OnValueChanged.AddListener(settingField.Set);
        }

        public override void Disconnect(SelectWindowModeSettingField settingField)
        {
            inputField.OnValueChanged.RemoveListener(settingField.Set);
        }
    } 
}
