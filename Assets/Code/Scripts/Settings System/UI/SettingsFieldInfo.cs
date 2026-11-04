using TMPro;
using UnityEngine;
using YNQ.Settings.SettingField;

namespace YNQ.Settings.UI
{
	public class SettingsFieldInfo : MonoBehaviour
	{
		[SerializeField] private GameObject infoObject;
		[SerializeField] private TextMeshProUGUI tmpName;
		[SerializeField] private TextMeshProUGUI tmpDescription;

        private void OnEnable()
        {
			HideInfo();
        }

        public void ShowInfo(ScriptableSettingFieldBase settingField)
		{
			tmpName.text = settingField.GetName();
			tmpDescription.text = settingField.GetDescription();

			infoObject.SetActive(true);
		}

		public void HideInfo()
		{
			tmpName.text = string.Empty;
			tmpDescription.text = string.Empty;
			infoObject.SetActive(false);
		}
	} 
}
