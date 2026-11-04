using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace YNQ.Settings.SettingField
{
    public sealed class CustomSerializerConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(ICustomFieldSerializer)
                .IsAssignableFrom(objectType);
        }

        public override void WriteJson(
            JsonWriter writer,
            object value,
            JsonSerializer serializer)
        {
            var custom = (ICustomFieldSerializer)value;
            custom.Serialize(serializer).WriteTo(writer);
        }

        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            if (existingValue is not ICustomFieldSerializer custom)
            {
                throw new JsonSerializationException(
                    $"An existing instance is required for {objectType.Name}.");
            }

            var token = JToken.Load(reader);
            custom.Deserialize(token, serializer);

            return existingValue;
        }
    }
}