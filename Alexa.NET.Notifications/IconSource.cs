using System;
using Newtonsoft.Json;

namespace Alexa.NET.Notifications
{
    public class IconSource
    {
        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("dimensions")]
        public IconDimensions Dimensions { get; set; }
    }
}