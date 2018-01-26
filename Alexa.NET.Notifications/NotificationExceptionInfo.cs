using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Alexa.NET
{
    public class NotificationExceptionInfo
    {
        [JsonProperty("message")]
        public string Message
        {
            get; set;
        }

        [JsonProperty("details")]
        public JObject Details { get; set; }
    }
}