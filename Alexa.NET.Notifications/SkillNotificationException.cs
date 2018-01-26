using System;
using Newtonsoft.Json.Linq;

namespace Alexa.NET
{
    public class SkillNotificationException : Exception
    {
        public JObject ErrorDetail { get; }

        public SkillNotificationException(NotificationExceptionInfo info) : base(info.Message)
        {
            ErrorDetail = info.Details;
        }
    }
}