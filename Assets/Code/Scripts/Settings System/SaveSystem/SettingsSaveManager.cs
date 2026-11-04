using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using YNQ.Settings.SettingField;

namespace YNQ.Settings.SaveSystem
{
	public static class SettingsSaveManager
	{
		private const string SETTINGS_SAVE_KEY = "Settings Save";
		
		public static void Save(SettingsSave save)
		{
            var serializer = GetSerializer();

            var json = JToken.FromObject(save, serializer);
			PlayerPrefs.SetString(SETTINGS_SAVE_KEY, json.ToString());
		}

		public static SettingsSave Load()
		{
            var settings = GetGenericSettings();

            var json = PlayerPrefs.GetString(SETTINGS_SAVE_KEY);
			var save = JsonConvert.DeserializeObject<SettingsSave>(json, settings);

			return save;
		}

		public static bool CanLoad() => PlayerPrefs.HasKey(SETTINGS_SAVE_KEY);

		private static JsonSerializerSettings GetGenericSettings()
		{
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new CustomSerializerConverter());

			return settings;
        }

		private static JsonSerializer GetSerializer()
		{
            var settings = GetGenericSettings();
            settings.TypeNameHandling = TypeNameHandling.Objects;
            settings.Formatting = Formatting.Indented;
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            return JsonSerializer.Create(settings);
        }
	} 
}
