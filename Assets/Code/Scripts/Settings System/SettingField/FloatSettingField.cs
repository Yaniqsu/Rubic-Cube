using UnityEngine;

namespace YNQ.Settings.SettingField
{
    [CreateAssetMenu(fileName = "Float Setting Field", menuName = "YNQ /Settings /Fields /Float")]
    public class FloatSettingField : AScriptableSettingField<float>
	{
        [field: SerializeField] public float MinValue { get; private set; }
        [field: SerializeField] public float MaxValue { get; private set; }
        [field: SerializeField] public float Step{ get; private set; }

        public override string GetValueDisplay() => TempValue.ToString();

        protected override float ValidateValue(float value)
        {
            var modulo = value % Step;
            var roundedValue = value - modulo;
            if (modulo >= Step / 2) roundedValue += Step;
            return Mathf.Clamp(roundedValue, MinValue, MaxValue);
        }
    } 
}
