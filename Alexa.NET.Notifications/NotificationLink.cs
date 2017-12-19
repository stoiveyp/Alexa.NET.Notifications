using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Alexa.NET.Notifications
{
    internal class NotificationLink:ISingleProperty<Uri>
    {
        [JsonProperty("href")]
        public Uri SpecifiedProperty { get; set; }
    }
}
