using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GardeningExpress.DespatchCloudClient.JsonConverters
{
    public class EmptyArrayOrDictionaryConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.IsAssignableFrom(typeof(Dictionary<string, object>));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var token = JToken.Load(reader);

            return token.Type switch
            {
                JTokenType.Object => token.ToObject(objectType, serializer),
                JTokenType.Array when !token.HasValues => new Dictionary<string, object>(),
                JTokenType.Array when token.HasValues => SerializeArrayToDictionary(token),
                JTokenType.Null => new Dictionary<string, object>(),
                _ => throw new JsonSerializationException("Object or empty array expected")
            };
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
        
        private Dictionary<string, object> SerializeArrayToDictionary(JToken token)
        {
            var result = new Dictionary<string, object>();

            if (token is JArray jArray)
                foreach (var (key, value) in jArray.Children<JToken>()
                    .SelectMany(j => j.ToObject<Dictionary<string, object>>()))
                {
                    result.Add(key, value);
                }

            return result;
        }
    }
}