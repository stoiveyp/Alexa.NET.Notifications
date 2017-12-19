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

        [JsonProperty("expiryTime", ItemConverterType = typeof(IsoDateTimeConverter))]
        public DateTime ExpiryTime { get; set; }

        [JsonProperty("referenceId")]
        public string ReferenceId { get; set; }
    }
}