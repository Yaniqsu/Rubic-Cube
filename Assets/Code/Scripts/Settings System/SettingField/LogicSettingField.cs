using UnityEngine;

namespace YNQ.Settings.SettingField
{
    [CreateAssetMenu(fileName = "Logic Setting Field", menuName = "YNQ /Settings /Fields /Logic")]
    public class LogicSettingField : AScriptableSettingField<bool>
	{
        public override string GetValueDisplay() => TempValue.ToString();

        protected override bool ValidateValue(bool value) => value;
    } 
}
