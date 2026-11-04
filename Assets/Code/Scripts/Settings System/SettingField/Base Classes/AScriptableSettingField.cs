using System;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace YNQ.Settings.SettingField
{
	public abstract class AScriptableSettingField<T> : ScriptableSettingFieldBase, ISettingField<T>
	{
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField, TextArea(3, 10)] public string Description { get; private set; }

        [SerializeField] private T defaultValue;
        [SerializeField] private bool autoApply;
        
        private T m_value;
        private T m_tempValue;

        public T TempValue => m_tempValue;
        public T Value=> m_value;

        public event Action<T> OnValueChanged;


        public override void ResetField()
        {
            Set(defaultValue);
            Apply();
        }

        public override void Initialize()
        {
            m_tempValue = m_value;
        }

        public void Set(T value)
        {
            m_tempValue = ValidateValue(value);

            if(autoApply) 
                Apply();
        }

        public override void Apply()
        {
            m_value = m_tempValue;
            OnValueChanged?.Invoke(TempValue);
        }

        public override void Revert()
        {
            m_tempValue = m_value;
        }

        public override void ClearEvents()
        {
            OnValueChanged = null;
        }
        public override object GetValue() => m_value;

        public override void LoadValue(object value)
        {
            JToken token = JToken.FromObject(value);
            var data = (T)token.ToObject(typeof(T));
            Set(data);
            Apply();
        }

        public override bool IsDirty() => !m_value.Equals(m_tempValue);

        public override string GetName() => Name;
        public override string GetDescription() => Description;

        public abstract string GetValueDisplay();
        protected abstract T ValidateValue(T value);
    } 
}
