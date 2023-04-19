using System;
using Newtonsoft.Json;

namespace Nuclei.Helpers.Utilities.BindableProperty;

public class BindablePropertyJsonConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeof(BindableProperty<>);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var valueType = objectType.GetGenericArguments()[0];
        var bindablePropertyType = typeof(BindableProperty<>).MakeGenericType(valueType);
        var value = serializer.Deserialize(reader, valueType);
        return Activator.CreateInstance(bindablePropertyType, value);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var bindableProperty = value as dynamic;
        serializer.Serialize(writer, bindableProperty.Value);
    }
}