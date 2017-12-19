using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Alexa.NET.Notifications
{
    public class Notification
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("displayInfo")]
        public DisplayInfo DisplayInfo { get; set; }

        [JsonProperty("recipientName")]
        public string RecipientName { get; set; }

        [JsonProperty("eventTime", ItemConverterType = typeof(IsoDateTimeConverter))]
        public DateTime EventTime { get; set; }

        [JsonProperty("expiryTime",ItemConverterType = typeof(IsoDateTimeConverter))]
        public DateTime ExpiryTime { get; set; }

        [JsonProperty("referenceId")]
        public string ReferenceId { get; set; }

        [JsonProperty("skillInfo")]
        public SkillInfo SkillInfo { get; set; }

        [JsonProperty("spokenInfo")]
        public SpokenInfo SpokenInfo { get; set; }

        [JsonProperty("_links"),JsonConverter(typeof(SinglePropertyConverter<NotificationLink,Uri>))]
        public Dictionary<string,Uri> Links { get; set; }
    }
}
