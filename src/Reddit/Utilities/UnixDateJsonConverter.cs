using System;
using Newtonsoft.Json;

namespace Reddit.Utilities
{
    public class UnixDateJsonConverter : JsonConverter
    {
        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var dateTime = (DateTime) value;
            var stamp = dateTime.Subtract(Epoch);
            var result = stamp.TotalMilliseconds;
            writer.WriteValue(result);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var stamp = (double) reader.Value;
            var result = Epoch.AddSeconds(stamp);
            return result;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime);
        }
    }
}