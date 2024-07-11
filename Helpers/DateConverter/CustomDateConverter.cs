using Newtonsoft.Json;
using System.Globalization;
using System;
namespace ZahimarProject.Helpers.DateConverter
{
    public class CustomDateConverter : JsonConverter<DateTime>
    {
        private readonly string _dateFormat = "dddd MMM dd";

        public override void WriteJson(JsonWriter writer, DateTime value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString(_dateFormat, CultureInfo.InvariantCulture));
        }

        public override DateTime ReadJson(JsonReader reader, Type objectType, DateTime existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return DateTime.ParseExact((string)reader.Value, _dateFormat, CultureInfo.InvariantCulture);
        }
    }
}
