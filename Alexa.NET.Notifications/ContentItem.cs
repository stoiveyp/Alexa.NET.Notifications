using Newtonsoft.Json;

namespace Alexa.NET.Notifications
{
    public class ContentItem
    {
        [JsonProperty("primaryText", NullValueHandling = NullValueHandling.Ignore)]
        public string PrimaryText { get; set; }

        [JsonProperty("secondaryText", NullValueHandling = NullValueHandling.Ignore)]
        public string SecondaryText { get; set; }

        public ContentItem(string primaryText)
        {
            PrimaryText = primaryText;
        }
    }
}