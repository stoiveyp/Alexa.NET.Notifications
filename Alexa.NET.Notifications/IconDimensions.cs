using Newtonsoft.Json;

namespace Alexa.NET.Notifications
{
    public class IconDimensions
    {
        [JsonProperty("widthPixels")]
        public int Width { get; set; }

        [JsonProperty("heightPixels")]
        public int Height { get; set; }
    }
}