using UnityEngine;

namespace YNQ.Settings.SettingField
{
	public abstract class ScriptableSettingFieldBase : ScriptableObject
	{
        public abstract void ResetField();
        public abstract void Initialize();
        public abstract void Apply();
        public abstract void Revert();
        public abstract void ClearEvents();
        public abstract string GetName();
        public abstract string GetDescription();
        public abstract bool IsDirty();
        public abstract object GetValue();
        public abstract void LoadValue(object value);
    } 
}
