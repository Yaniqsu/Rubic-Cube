using System.Linq;
using UnityEngine;
using YNQ.Settings.SettingField;
using YNQ.Settings.SaveSystem;

namespace YNQ.Settings.Bank
{
	[CreateAssetMenu(menuName = "YNQ /Settings /Settings Group", fileName = "Settings Group")]
	public class SettingsGroup : ScriptableObject
	{
		[field: SerializeField] public string Name { get; private set; }
		[field: SerializeField] public ScriptableSettingFieldBase[] SettingFields { get; private set; }

		public void ApplyChanges()
		{
			foreach(var s in SettingFields)
			{
				s.Apply();
			}
		}

		public void RevertTemporaryChanges()
		{
			foreach(var s in SettingFields)
			{
				s.Revert();
			}
		}

		public void ResetFields()
		{
            foreach (var s in SettingFields)
            {
                s.ResetField();
            }
        }

		public void ClearEvents()
		{
			foreach(var s in SettingFields)
			{
				s.ClearEvents();
			}
		}

		public bool IsDirty() => SettingFields.Any(field => field.IsDirty());

		public SettingsGroupSave GetGroupsSave()
		{
			var dictionary = new SDictionary<string, object>();

			foreach(var s in SettingFields)
			{
				dictionary.Add(s.name, s.GetValue());
			}

			return new SettingsGroupSave() { data = dictionary };
		}

		public void Load(SettingsGroupSave save)
		{
			foreach(var s in SettingFields)
			{
				if(save.data.TryGetValue(s.GetName(), out var data))
					s.LoadValue(data);
			}
		}
	} 
}
