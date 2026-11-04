using System.Linq;
using UnityEngine;
using YNQ.Settings.SettingField;

namespace YNQ.Settings.InputController
{
    public class Vector2InputController : AInputFieldController<SelectVector2SettingField, InputField_Vector2Spinner>
    {
        public override void ConnectValues(SelectVector2SettingField settingField)
        {
            inputField.SetValues(settingField.Options);
            inputField.SetIndex(FindIndex(settingField.Options, settingField.Value));

            inputField.OnValueChanged.AddListener(settingField.SetIndex);
        }

        public override void Disconnect(SelectVector2SettingField settingField)
        {
            inputField.OnValueChanged.RemoveListener(settingField.SetIndex);
        }

        private int FindIndex(Vector2[] array, Vector2 element)
        {
            for(var i = 0; i < array.Length; i++)
            {
                if (array[i] == element)
                    return i;
            }

            return -1;
        }
    }  
}
