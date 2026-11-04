using System;
using YNQ.Settings.SettingField;

namespace YNQ.Settings.InputController
{
	public abstract class AInputController<T> : InputControllerBase where T : ScriptableSettingFieldBase
	{
        public override void Connect(ScriptableSettingFieldBase settingField)
        {
			if (settingField is T typedSettingField)
				Connect(typedSettingField);
        }

        public override void Disconnect(ScriptableSettingFieldBase settingField)
        {
            if (settingField is T typedSettingField)
                Disconnect(typedSettingField);
        }

        public override Type GetTypeAssociated() => typeof(T);


        public abstract void Connect(T settingField);
		public abstract void Disconnect(T settingField);
	} 
}
