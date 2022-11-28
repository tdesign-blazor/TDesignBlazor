using System.ComponentModel;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TDesign;
class EnumDescriptionConverter<T> : JsonConverter<T> where T : IComparable, IFormattable, IConvertible
{
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string jsonValue = reader.GetString();

        foreach (var fi in typeToConvert.GetFields())
        {
            var name = fi.Name.ToLower();
            if (fi.TryGetCustomAttribute<DescriptionAttribute>(out var attribute))
            {
                name = attribute.Description;
            }

            if (name == jsonValue)
            {
                return (T)fi.GetValue(null);
            }
        }
        throw new JsonException($"string {jsonValue} was not found as a description in the enum {typeToConvert}");
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            return;
        }

        FieldInfo fi = value.GetType().GetField(value.ToString());


        var name = fi.Name.ToLower();
        if (fi.TryGetCustomAttribute<DescriptionAttribute>(out var attribute))
        {
            name = attribute.Description;
        }

        writer.WriteStringValue(name);
    }
}
