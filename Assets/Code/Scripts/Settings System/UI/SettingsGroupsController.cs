using UnityEngine;
using UnityEngine.Events;
using YNQ.Settings.Bank;

namespace YNQ.Settings.UI
{
	public class SettingsGroupsController : MonoBehaviour
	{
		[SerializeField] private RectTransform context;
		[SerializeField] private SettingsGroupButton groupButtonPrefab;

		private SettingsGroupButton[] groupButtons;

		public UnityEvent<int> onGroupChanged;

		public void Initialize(SettingsGroup[] groups)
		{
			groupButtons = new SettingsGroupButton[groups.Length]; 
			
			for(int i = 0; i < groups.Length; i++)
			{
				var index = i;

				var button = Instantiate(groupButtonPrefab, context);
				button.Text = groups[i].Name;
				button.onClick.AddListener(() => CallOnGroupChanged(index));

				groupButtons[i] = button;
			}
		}

		private void CallOnGroupChanged(int index) 
		{
			for (int i = 0; i < groupButtons.Length; i++)
			{
				groupButtons[i].Selected = i == index;
			}
			
			onGroupChanged?.Invoke(index); 
		}

		public void DeselectAll()
		{
			foreach(var button in groupButtons)
			{
				button.Selected = false;
			}
		}
	} 
}
