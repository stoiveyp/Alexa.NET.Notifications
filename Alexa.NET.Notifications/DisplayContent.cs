﻿using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Alexa.NET.Notifications
{
    public class DisplayContent
    {
        [JsonProperty("locale")]
        public string Locale { get; set; }

        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        [JsonProperty("body",NullValueHandling = NullValueHandling.Ignore)]
        public string Body { get; set; }

        [JsonProperty("toast",NullValueHandling = NullValueHandling.Ignore)]
        public ContentItem Toast { get; set; }

        [JsonProperty("bodyItems")]
        public List<ContentItem> BodyItems { get; set; } = new List<ContentItem>();

        public bool ShouldSerializeBodyItems()
        {
            return BodyItems.Any();
        }
    }
}