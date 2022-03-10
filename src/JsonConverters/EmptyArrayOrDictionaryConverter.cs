using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GardeningExpress.DespatchCloudClient.JsonConverters
{
    internal class EmptyArrayOrDictionaryConverter : JsonConverter
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
                JTokenType.Array when !token.HasValues => Activator.CreateInstance(objectType),
                _ => throw new JsonSerializationException("Object or empty array expected")
            };
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}