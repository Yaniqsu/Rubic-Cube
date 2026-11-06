using System.Collections;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace YNQ.InputField
{
	public class InputField_Slider : AInputField
	{
		[SerializeField, BoxGroup(BOX_COMPONENTS)] private Slider slider;
		[SerializeField, BoxGroup(BOX_COMPONENTS)] private TextMeshProUGUI valueDisplay;
		[SerializeField, BoxGroup(BOX_SETTINGS)] private float value;
		[SerializeField, BoxGroup(BOX_SETTINGS)] private float minValue;
		[SerializeField, BoxGroup(BOX_SETTINGS)] private float maxValue;
		[SerializeField, BoxGroup(BOX_SETTINGS)] private bool isInteger;

		public UnityEvent<float> onValueChanged;

		private Coroutine _waitForMouseUpCoroutine;
		private bool _waiting;

		public float FloatValue => value;
		public int IntValue => Mathf.RoundToInt(value);

        protected override void OnValidate()
        {
            base.OnValidate();

			SetMinValue(minValue);
			SetMaxValue(maxValue);
			SetIsIntegers(isInteger);
			SetValue(value);
        }

        protected override void Awake()
        {
            base.Awake();

			slider.onValueChanged.AddListener(UpdateValue);
        }

		public void SetValue(float value)
		{
			slider.value = value;
			UpdateValue(value);
		}

		public void SetMinValue(float minValue)
		{
			slider.minValue = minValue;
		}

		public void SetMaxValue(float maxValue)
		{
			slider.maxValue = maxValue;
		}

		public void SetIsIntegers(bool integer)
		{
			slider.wholeNumbers = integer;
		}

		private void UpdateValue(float value)
		{
			if(Application.isPlaying && gameObject.activeInHierarchy)
				TakeInputSelection();
			this.value = value;
			if(valueDisplay) valueDisplay.text = $"{value}";
			NotifyValueChanged(value);
		}

		private void NotifyValueChanged(float sliderValue)
		{
            onValueChanged.Invoke(sliderValue);
        }

		private void TakeInputSelection()
		{
			if (_waiting)
				return;

			InputFieldSelection.current = this;
			_waiting = true;
			_waitForMouseUpCoroutine = StartCoroutine(WaitForMouseUp());
		}

		private IEnumerator WaitForMouseUp()
		{
			yield return new WaitWhile(() => Mouse.current.leftButton.isPressed);
			InputFieldSelection.current = null;
			_waiting = false;
		}
	}
}