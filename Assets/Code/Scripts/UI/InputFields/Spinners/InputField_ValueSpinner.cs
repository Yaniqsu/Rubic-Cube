using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace YNQ.InputField
{
    public abstract class InputField_ValueSpinner<T> : InputField_ASpinner<int>
    {
        [SerializeField, BoxGroup(BOX_SETTINGS)] protected T[] values;

        protected int currentIndex;
        public override int Value => currentIndex;

        public T[] Values => values;


        protected override void OnValidate()
        {
            base.OnValidate();

            UpdateValue();
        }

        public void SetIndex(int index)
        {
            currentIndex = Mathf.Clamp(index, 0, Values.Length - 1);
            UpdateValue();
        }

        public void SetValues(params T[] values)
        {
            this.values = values;
            currentIndex = 0;
            UpdateValue();
        }

        public override void Next()
        {
            if (values.Length == 0)
                return;

            currentIndex = (currentIndex + 1) % values.Length;

            OnValueChanged?.Invoke(currentIndex);
            UpdateValue();
        }

        public override void Previous()
        {
            if (values.Length == 0)
                return;

            currentIndex = (currentIndex - 1 + values.Length) % values.Length;

            OnValueChanged?.Invoke(currentIndex);
            UpdateValue();
        }

        protected override void UpdateValue()
        {
            if (valueDisplay != null)
                valueDisplay.text = values.Length > 0 ? FormatValueDisplay(values[currentIndex]) : string.Empty;
        }

        protected virtual string FormatValueDisplay(T value) => value.ToString();
    } 
}
