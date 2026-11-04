using UnityEngine;

namespace YNQ.Settings.SettingField
{
    [CreateAssetMenu(fileName = "Switch Setting Field", menuName = "YNQ /Settings /Fields /Switch")]
    public class SwitchSettingField : AScriptableSettingField<bool>
	{
        public override string GetValueDisplay() => TempValue ? "on" : "off";

        protected override bool ValidateValue(bool value) => value;
    } 
}
