using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Alexa.NET.Notifications.Tests
{
    public static class Utility
    {
        private const string ExamplesPath = "Examples";

        public static string GetExampleJson(string file)
        {
            return File.ReadAllText(Path.Combine(ExamplesPath, file));
        }

        public static T GetObjectFromJson<T>(string json)
        {
            using (var reader = new JsonTextReader(new StringReader(json)))
            {
                return JsonSerializer.Create().Deserialize<T>(reader);
            }
        }

        public static T GetObjectFromExample<T>(string file)
        {
            using (var reader = new JsonTextReader(new StringReader(GetExampleJson(file))))
            {
                return JsonSerializer.Create().Deserialize<T>(reader);
            }
        }

        public static bool CompareJson(object actual, string expectedFile)
        {
            var actualJObject = JObject.FromObject(actual);
            var expected = GetExampleJson(expectedFile);
            var expectedJObject = JObject.Parse(expected);
            Console.WriteLine(actualJObject);
            return JToken.DeepEquals(expectedJObject, actualJObject);
        }
    }
}
