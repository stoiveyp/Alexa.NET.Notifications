using System.Collections.Generic;
using System.Text;
using Alexa.NET.Response.Directive.Templates;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Alexa.NET.Notifications
{
    public class SkillInfo
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("content")]
        public List<SkillContent> Content { get; set; } = new List<SkillContent>();
    }

    public class SkillContent
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("locale")]
        public string Locale { get; set; }

        [JsonProperty("icon")]
        public Icon Icon { get; set; }
    }
}
