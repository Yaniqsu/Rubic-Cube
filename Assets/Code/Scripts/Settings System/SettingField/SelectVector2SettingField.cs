using UnityEngine;

namespace YNQ.Settings.SettingField
{
	[CreateAssetMenu(fileName = "Select Vector2 Setting Field", menuName = "YNQ /Settings /Fields /Select /Vector2")]
	public class SelectVector2SettingField : ASelectSettingField<Vector2>
	{
		protected override string FormatDisplay(Vector2 value) => $"{value.x} x {value.y}";
    } 
}
