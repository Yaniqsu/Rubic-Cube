using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace YNQ.Settings.SettingField
{
	public interface ICustomFieldSerializer
	{
        JToken Serialize(JsonSerializer serializer);

        void Deserialize(
            JToken token,
            JsonSerializer serializer);
    }
}