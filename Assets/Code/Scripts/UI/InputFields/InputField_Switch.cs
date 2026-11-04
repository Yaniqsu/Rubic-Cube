using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace YNQ.InputField
{
	public class InputField_Switch : AInputField
	{
		[SerializeField, BoxGroup(BOX_COMPONENTS)] private Image cube;

		[SerializeField, BoxGroup(BOX_SETTINGS)] private bool defaultValue;

		[SerializeField, BoxGroup(BOX_SETTINGS)] private Vector2 offPos;
		[SerializeField, BoxGroup(BOX_SETTINGS)] private Vector2 onPos;

		[SerializeField, BoxGroup(BOX_SETTINGS)] private Color offColor;
		[SerializeField, BoxGroup(BOX_SETTINGS)] private Color onColor;

		[SerializeField, BoxGroup(BOX_EVENTS)] public UnityEvent<bool> OnValueChanged;

		public bool Value { get; private set; }

        protected override void OnValidate()
        {
			base.OnValidate();
			
			SetValue(defaultValue);
        }

        public void SetValue(bool value)
		{
			Value = value;
			UpdateDisplay();

			OnValueChanged?.Invoke(value);
		}

		public void Toggle() => SetValue(!Value);

		private void UpdateDisplay()
		{
			cube.rectTransform.anchoredPosition = Value ? onPos : offPos;
			cube.color = Value ? onColor : offColor;
		}
	}
}