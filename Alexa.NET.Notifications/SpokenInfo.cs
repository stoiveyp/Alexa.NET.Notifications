using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Alexa.NET.Notifications
{
    public class SpokenInfo
    {
        [JsonProperty("content")]
        public List<SpokenText> Content { get; set; } = new List<SpokenText>();
    }
}
