using System;

namespace YNQ.Settings.SettingField
{
	public class AEnumSettingField<T> : AScriptableSettingField<T> where T : Enum
	{
        public override string GetValueDisplay() => Enum.GetName(typeof(T), TempValue);

        protected override T ValidateValue(T value) => value;
    } 
}
