using UnityEngine;
using YNQ.InputField;
using YNQ.Settings.SettingField;

namespace YNQ.Settings.InputController
{
	public class SwitchInputController : AInputFieldController<SwitchSettingField, InputField_Switch>
	{
        public override void Disconnect(SwitchSettingField settingField)
        {
            inputField.OnValueChanged.RemoveListener(settingField.Set);
        }

        public override void ConnectValues(SwitchSettingField settingField)
        {
            inputField.SetValue(settingField.Value);

            inputField.OnValueChanged.AddListener(settingField.Set);
        }
    } 
}
