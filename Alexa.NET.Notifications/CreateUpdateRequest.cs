using System;
using Alexa.NET.Notifications;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Alexa.NET.Notifications
{
    public class CreateUpdateRequest
    {
        [JsonProperty("displayInfo")]
        public DisplayInfo DisplayInfo { get; set; }

        [JsonProperty("spokenInfo")]
        public SpokenInfo SpokenInfo { get; set; }

        [JsonProperty("expiryTime"),JsonConverter(typeof(CustomIsoConverter))]
        public DateTime ExpiryTime { get; set; }

        [JsonProperty("referenceId")]
        public string ReferenceId { get; set; }
    }

    public class CustomIsoConverter:JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return DateTime.Parse(existingValue.ToString());
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(((DateTime)value).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.sssZ"));
        }
    }
}