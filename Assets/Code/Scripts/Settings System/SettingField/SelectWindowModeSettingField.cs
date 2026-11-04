using UnityEngine;

namespace YNQ.Settings.SettingField
{
    public enum WindowMode
    {
        FullScreen = 0,
        Windowed = 1,
        WindowedBorderless = 2
    }
    
    [CreateAssetMenu(fileName = "Select WindowMode Setting Field", menuName = "YNQ /Settings /Fields /Select /Window")]
    public class SelectWindowModeSettingField : AEnumSettingField<WindowMode>
	{

	} 
}
