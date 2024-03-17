using System.Collections.Generic;
using Newtonsoft.Json;

namespace GardeningExpress.DespatchCloudClient.DTO.Request
{
    public record InventoryUpdateRequest
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("sku")]
        public string SKU { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("custom_fields")]
        public Dictionary<string, object> CustomFields { get; set; }
    }
}