using System;
using System.Collections.Generic;
using Alexa.NET.Notifications;
using Newtonsoft.Json;

namespace Alexa.NET
{
    public class NotificationInfo
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("_links"), JsonConverter(typeof(SinglePropertyConverter<NotificationLink, Uri>))]
        public Dictionary<string, Uri> Links { get; set; }
    }
}