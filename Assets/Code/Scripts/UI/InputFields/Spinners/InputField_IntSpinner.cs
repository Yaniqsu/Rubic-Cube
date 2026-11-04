using NaughtyAttributes;

using UnityEngine;

namespace YNQ.InputField
{
    public class InputField_IntSpinner : InputField_ASpinner<int>
    {
        [SerializeField, BoxGroup(BOX_SETTINGS)] private int _minValue = 0;
        [SerializeField, BoxGroup(BOX_SETTINGS)] private int _maxValue = 10;
        [SerializeField, BoxGroup(BOX_SETTINGS)] private int _value = 0;
        [SerializeField, BoxGroup(BOX_SETTINGS)] private bool _allowWrapping;

        public override int Value => _value;

        protected override void OnValidate()
        {
            base.OnValidate();

            if (_minValue > _maxValue)
            {
                (_minValue, _maxValue) = (_maxValue, _minValue);
            }

            _value = Mathf.Clamp(_value, _minValue, _maxValue);

            if (valueDisplay != null)
            {
                valueDisplay.text = _value.ToString();
            }
        }

        public void SetValue(int value)
        {
            _value = Mathf.Clamp(value, _minValue, _maxValue);

            OnValueChanged.Invoke(_value);
            UpdateValue();
        }

        public void SetMinValue(int value)
        {
            _minValue = value;
            _value = Mathf.Clamp(value, _minValue, _maxValue);
            UpdateValue();
        }

        public void SetMaxValue(int value)
        {
            _maxValue = value;
            _value = Mathf.Clamp(value, _minValue, _maxValue);
            UpdateValue();
        }

        public override void Next()
        {
            if (_value == _maxValue)
                _value = _allowWrapping ? _minValue : _maxValue;
            else
                _value++;

            OnValueChanged.Invoke(_value);
            UpdateValue();
        }

        public override void Previous()
        {
            if (_value == _minValue)
                _value = _allowWrapping ? _maxValue : _minValue;
            else
                _value--;

            OnValueChanged.Invoke(_value);
            UpdateValue();
        }

        protected override void UpdateValue()
        {
            valueDisplay.text = _value.ToString();
        }
    }

}