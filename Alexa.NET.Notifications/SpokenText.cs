using Alexa.NET.Response.Ssml;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Alexa.NET.Notifications
{
    public class SpokenText
    {
        public SpokenText() { }

        public SpokenText(string locale, string text)
        {
            Locale = locale;
            Text = text;
        }

        public SpokenText(string locale, Speech speech)
        {
            Locale = locale;
            Ssml = speech.ToXml();
        }

        [JsonProperty("locale")]
        public string Locale { get; set; }

        [JsonProperty("ssml",NullValueHandling = NullValueHandling.Ignore)]
        public string Ssml { get; set; }

        [JsonProperty("text",NullValueHandling = NullValueHandling.Ignore)]
        public string Text { get; set; }
    }
}