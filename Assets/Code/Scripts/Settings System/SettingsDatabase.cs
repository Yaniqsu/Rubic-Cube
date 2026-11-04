using System.Linq;
using UnityEngine;
using YNQ.Settings.Bank;
using YNQ.Settings.SaveSystem;

namespace YNQ.Settings
{
	[CreateAssetMenu(fileName = "Settings Database", menuName = "YNQ /Settings /Settings Database")]
	public class SettingsDatabase : ScriptableObject
	{
		[field: SerializeField]
		public SettingsGroup[] Groups { get; private set; }

		public void ApplyChanges()
		{
			foreach (var group in Groups)
			{
				group.ApplyChanges();
			}
		}

		public void RevertTemporaryChanges()
		{
			foreach(var group in Groups)
			{
				group.RevertTemporaryChanges();
			}
		}

        public void ResetGroups()
        {
            foreach (var group in Groups)
            {
                group.ResetFields();
            }
        }

        public void ClearEvents()
        {
            foreach (var group in Groups)
            {
                group.ClearEvents();
            }
        }

        public bool IsDirty() => Groups.Any(group => group.IsDirty());

		public SettingsSave GetSave()
		{
			var dictionary = new SDictionary<string, SettingsGroupSave>();

			foreach(var g in Groups)
			{
				dictionary.Add(g.name, g.GetGroupsSave());
			}

			return new SettingsSave() { groupsData = dictionary };
		}

		public void LoadSave(SettingsSave save)
		{
			foreach(var g in Groups)
			{
				if(save.groupsData.TryGetValue(g.Name, out var data))
					g.Load(data);
			}
		}
    } 
}
