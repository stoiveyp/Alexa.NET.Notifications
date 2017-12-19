using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Alexa.NET.Notifications
{
    internal class SinglePropertyConverter<TObject,TPropertyType>:JsonConverter where TObject:ISingleProperty<TPropertyType>,new()
    {
        private Type RequiredType { get; }

        public SinglePropertyConverter()
        {
            RequiredType = typeof(Dictionary<string, TPropertyType>);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer,((Dictionary<string, TPropertyType>)value).ToDictionary(kvp => kvp.Key,kvp => new TObject{SpecifiedProperty = kvp.Value}));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return new Dictionary<string, TPropertyType>();
            }
            var objects = serializer.Deserialize<Dictionary<string, TObject>>(reader);
            return objects.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.SpecifiedProperty);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == RequiredType;
        }
    }
}
