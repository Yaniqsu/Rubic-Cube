using YNQ.InputField;
using YNQ.Settings.SettingField;

namespace YNQ.Settings.InputController
{
    public class FloatInputController : AInputFieldController<FloatSettingField, InputField_Slider>
    {
        public override void Disconnect(FloatSettingField settingField)
        {
            inputField.onValueChanged.RemoveListener(settingField.Set);
        }

        public override void ConnectValues(FloatSettingField settingField)
        {
            inputField.SetMinValue(settingField.MinValue);
            inputField.SetMaxValue(settingField.MaxValue);
            inputField.SetIsIntegers(false);
            inputField.SetValue(settingField.Value);

            inputField.onValueChanged.AddListener(settingField.Set);
        }
    }
}