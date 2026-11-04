using System.Collections.Generic;
using UnityEngine;
using YNQ.Settings.Bank;
using YNQ.Settings.InputController;
using YNQ.Settings.SettingField;

namespace YNQ.Settings.UI
{
	public class SettingFieldsController : MonoBehaviour
	{
		[SerializeField] private SettingFieldInputDatabase database;
		[SerializeField] private SettingsFieldInfo settingsFieldInfo;
		[SerializeField] private RectTransform context;

		private readonly Dictionary<ScriptableSettingFieldBase, InputControllerBase> _inputControllers = new();
		private readonly Dictionary<string, InputControllerBase> _controllerPrefabs = new();

        private void Start()
        {
			var controllers = database.GetControllers();
			foreach(var c in controllers)
			{
				if (c == null)
					continue;

				_controllerPrefabs.Add(c.GetTypeAssociated().Name, c);
			}
        }

        public void ChangeGroup(SettingsGroup group)
		{
			ClearInputControllers();
			CreateInputControllers(group.SettingFields);
		}

		public void ClearInputControllers()
		{
			foreach(var (s, c) in _inputControllers)
			{
				c.Disconnect(s);
				Destroy(c.gameObject);
			}

			_inputControllers.Clear();
		}

		private void CreateInputControllers(ScriptableSettingFieldBase[] settings)
		{
            foreach (var setting in settings)
			{
				var settingField = setting;
				var type = setting.GetType();
				if (!_controllerPrefabs.TryGetValue(type.Name, out var controllerPrefab))
					continue;

				if(controllerPrefab == null)
				{
					continue;
				}


				var inputController = Instantiate(controllerPrefab, context);
				inputController.Connect(setting);

				inputController.OnEnter += () => settingsFieldInfo.ShowInfo(settingField);
				inputController.OnExit += () => settingsFieldInfo.HideInfo();

				_inputControllers.Add(setting, inputController);
			}
		}
	} 
}
