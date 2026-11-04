using System;
using UnityEngine;

namespace YNQ.InputField
{
	public class InputField_AEnumSpinner<TEnum> : InputField_ASpinner<TEnum>
		where TEnum : Enum
	{
        [SerializeField] private int defaultValue;
        
        private int _currentIndex;
        private TEnum _currentValue;
        private int _length;
        
        public override TEnum Value => _currentValue;

        protected override void OnValidate()
        {
            base.OnValidate();

            SetLength();
            defaultValue = Mathf.Clamp(defaultValue, 0, _length - 1);
            SetIndex(defaultValue);
        }

        protected override void Awake()
        {
            base.Awake();

            SetLength();
        }

        public override void Next()
        {
            ChangeIndex(1);
        }

        public override void Previous()
        {
            ChangeIndex(-1);
        }

        public void SetIndex(int index)
        {
            _currentIndex = index;

            UpdateValue();
        }

        private void ChangeIndex(int modifier)
        {
            _currentIndex = (_currentIndex + modifier) % _length;

            UpdateValue();
        }

        protected override void UpdateValue()
        {
            _currentIndex = Mathf.Clamp(_currentIndex, 0, _length - 1);
            _currentValue = (TEnum)(object)_currentIndex;

            valueDisplay.text = _currentValue.ToString();
        }

        private void SetLength()
        {
            _length = Enum.GetNames(typeof(TEnum)).Length;
        }
    } 
}
