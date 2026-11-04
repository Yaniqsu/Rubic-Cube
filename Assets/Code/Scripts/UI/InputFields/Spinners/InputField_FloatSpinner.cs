using NaughtyAttributes;

using UnityEngine;
using UnityEngine.Events;

namespace YNQ.InputField
{
    public class InputField_FloatSpinner : InputField_ASpinner<float>
    {
        [SerializeField, BoxGroup(BOX_SETTINGS)] private float _minValue = 0f;
        [SerializeField, BoxGroup(BOX_SETTINGS)] private float _maxValue = 1f;
        [SerializeField, BoxGroup(BOX_SETTINGS)] private float _value = 0f;
        [SerializeField, BoxGroup(BOX_SETTINGS)] private float _increment = .1f;
        [SerializeField, BoxGroup(BOX_SETTINGS)] private int _precision = 1;
        [SerializeField, BoxGroup(BOX_SETTINGS)] private bool _allowWrapping;

        public override float Value => _value;

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

        public void SetValue(float value)
        {
            _value = Mathf.Clamp(value, _minValue, _maxValue);

            UpdateValue();
        }

        public void SetValue(double value)
        {
            _value = Mathf.Clamp((float)value, _minValue, _maxValue);

            UpdateValue();
        }

        public void SetMinValue(float value)
        {
            _minValue = value;
            _value = Mathf.Clamp(value, _minValue, _maxValue);

            RoundValue();
            UpdateValue();
        }

        public void SetMaxValue(float value)
        {
            _maxValue = value;
            _value = Mathf.Clamp(value, _minValue, _maxValue);

            RoundValue();
            UpdateValue();
        }

        public void SetIncrement(float value)
        {
            _increment = value;

            UpdateValue();
        }

        public override void Next()
        {
            _value += _increment;

            if (_value > _maxValue)
                _value = _allowWrapping ? _minValue + (_value - _increment - _maxValue) : _maxValue;

            UpdateValue();
        }

        public override void Previous()
        {
            _value -= _increment;

            if (_value < _minValue)
                _value = _allowWrapping ? _maxValue - (_value + _increment + _minValue) : _minValue;

            UpdateValue();
        }

        private void RoundValue()
        {
            _value = Mathf.Round(_value * Mathf.Pow(10, _precision)) / Mathf.Pow(10, _precision);
        }

        protected override void UpdateValue()
        {
            RoundValue();
            OnValueChanged.Invoke(_value);

            valueDisplay.text = _value.ToString();
        }
    } 
}
