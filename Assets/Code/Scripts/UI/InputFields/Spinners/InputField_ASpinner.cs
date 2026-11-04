using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace YNQ.InputField
{
    public abstract class InputField_ASpinner<T_EventValue> : AInputField
    {
        [SerializeField, BoxGroup(BOX_COMPONENTS)] protected TextMeshProUGUI valueDisplay;

        [BoxGroup(BOX_EVENTS)] public UnityEvent<T_EventValue> OnValueChanged;


        public abstract T_EventValue Value { get; }

        public abstract void Next();
        public abstract void Previous();
        protected abstract void UpdateValue();

        public override void ResetEvents()
        {
            base.ResetEvents();

            OnValueChanged.RemoveAllListeners();
        }
    }

}