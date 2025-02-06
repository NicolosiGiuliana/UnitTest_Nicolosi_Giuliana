using System.Globalization;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace FieldMicroservice.Binders
{
    public class TimeSpanConverter : JsonConverter<TimeSpan>
    {
        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();

            // Manejar el caso en el que el valor es nulo o está vacío
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new JsonException("Time value is required and cannot be null or empty.");
            }

            try
            {
                return TimeSpan.ParseExact(value, @"hh\:mm", CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                throw new JsonException($"Invalid time format: {value}. Please use the format 'hh:mm'.");
            }
            catch (OverflowException)
            {
                throw new JsonException($"Invalid time value: {value}. The hour value must be between 0 and 23.");
            }
        }

        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(@"hh\:mm"));
        }
    }
}
