using UnityEngine;

namespace YNQ.Settings.SettingField
{
    [CreateAssetMenu(fileName = "Int Setting Field", menuName = "YNQ /Settings /Fields /Int")]
    public class IntSettingField : AScriptableSettingField<int>
{
        [field: SerializeField] public int MinValue { get; private set; }
        [field: SerializeField] public int MaxValue { get; private set; }

        
        public override string GetValueDisplay() => TempValue.ToString();

        protected override int ValidateValue(int value) => Mathf.Clamp(value, MinValue, MaxValue);
    } 
}
