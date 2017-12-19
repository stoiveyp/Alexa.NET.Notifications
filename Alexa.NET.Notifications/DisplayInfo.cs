using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Alexa.NET.Notifications
{
    public class DisplayInfo
    {
        [JsonProperty("content")]
        public List<DisplayContent> Content { get; set; } = new List<DisplayContent>();
    }
}
