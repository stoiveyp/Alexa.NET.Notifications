using System.Collections.Generic;
using Newtonsoft.Json;

namespace Alexa.NET.Notifications
{
    public class Icon
    {
        [JsonProperty("iconId")]
        public string Id { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("sources")]
        public Dictionary<string,IconSource> Sources { get; set; } = new Dictionary<string, IconSource>();
    }
}