using System;

namespace YNQ.Settings.SettingField
{
	public interface ISettingField<T>
	{
		public void Set(T value);
		public string GetValueDisplay();

		public event Action<T> OnValueChanged;
	} 
}
