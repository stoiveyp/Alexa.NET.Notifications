using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Alexa.NET.Notifications
{
    public class NotificationListResponse
    {
        [JsonProperty("totalCount")]
        public int TotalCount { get; set; }

        [JsonProperty("notifications")]
        public Notification[] Notifications { get; set; }

        [JsonProperty("links"), JsonConverter(typeof(SinglePropertyConverter<NotificationLink, Uri>))]
        public Dictionary<string, Uri> Links { get; set; }
    }
}
