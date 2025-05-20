using System.Text.Json;
using System.Text.Json.Serialization;

namespace HelperContainer.JsonConvert
{
    public class JsonConverterForDecimal:JsonConverter<Decimal>
    {
        public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.GetDecimal(); // or parse string if needed
        }

        public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value); // or format if needed
        }
    }
}
