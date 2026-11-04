using UnityEngine;

namespace YNQ.Settings.SettingField
{
	public abstract class ASelectSettingField<T> : AScriptableSettingField<T>
	{
		[field: SerializeField] public T[] Options { get; private set; }


		public void SetIndex(int index)
		{
			index = Mathf.Clamp(index, 0, Options.Length - 1);
			Set(Options[index]);
		}

		
		public override string GetValueDisplay() => FormatDisplay(TempValue);

		protected override T ValidateValue(T value) => value;

		protected abstract string FormatDisplay(T value);
	} 
}
