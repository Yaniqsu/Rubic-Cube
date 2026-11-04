using UnityEngine;
using YNQ.InputField;
using YNQ.Settings.SettingField;

namespace YNQ.Settings.InputController
{
    public class IntInputController : AInputFieldController<IntSettingField, InputField_Slider>
    {
        private IntSettingField _currentField;
        
        public override void Disconnect(IntSettingField settingField)
        {
            inputField.onValueChanged.RemoveListener(SetSettingValue);
        }

        public override void ConnectValues(IntSettingField settingField)
        {
            _currentField = settingField;
            
            inputField.SetMinValue(settingField.MinValue);
            inputField.SetMaxValue(settingField.MaxValue);
            inputField.SetValue(settingField.TempValue);
            inputField.SetIsIntegers(true);

            inputField.onValueChanged.AddListener(SetSettingValue);
        }

        private void SetSettingValue(float value)
        {
            _currentField.Set((int)value);
        }
    }
}