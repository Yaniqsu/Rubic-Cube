using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class SDictionaryJsonConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        foreach (var row in (IEnumerable)value)
            serializer.Serialize(writer, row);
        writer.WriteEndArray();
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
            return null;

        var genericArgs = objectType.GetGenericArguments();
        var rowType = typeof(TableRow<,>).MakeGenericType(genericArgs);
        var listType = typeof(List<>).MakeGenericType(rowType);

        var rows = (IList)serializer.Deserialize(reader, listType);

        var dict = Activator.CreateInstance(objectType);
        var addMethod = objectType.GetMethod("Add", new[] { rowType });

        foreach (var row in rows)
            addMethod.Invoke(dict, new[] { row });

        return dict;
    }

    public override bool CanConvert(Type objectType)
        => objectType.IsGenericType 
           && objectType.GetGenericTypeDefinition() == typeof(SDictionary<,>);
}
