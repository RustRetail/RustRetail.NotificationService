using RustRetail.SharedKernel.Domain.Enums;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RustRetail.NotificationService.API.Configuration.Json
{
    internal class EnumerationJsonConverter<TEnum> : JsonConverter<TEnum>
        where TEnum : Enumeration
    {
        public override TEnum? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var name = reader.GetString();
                return Enumeration.FromName<TEnum>(name!);
            }

            if (reader.TokenType == JsonTokenType.Number && reader.TryGetInt32(out var value))
            {
                return Enumeration.FromValue<TEnum>(value);
            }

            throw new JsonException($"Unable to convert JSON to {typeof(TEnum).Name}.");
        }

        public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Name);
        }
    }
}
