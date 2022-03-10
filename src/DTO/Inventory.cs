using System.Collections.Generic;
using GardeningExpress.DespatchCloudClient.JsonConverters;
using Newtonsoft.Json;

namespace GardeningExpress.DespatchCloudClient.DTO
{
    public class Inventory
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("sku")]
        public string SKU { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("custom_fields")]
        [JsonConverter(typeof(EmptyArrayOrDictionaryConverter))]

        public Dictionary<string, object> CustomFields { get; set; }
    }
}