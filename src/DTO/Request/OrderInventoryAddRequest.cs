﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace GardeningExpress.DespatchCloudClient.DTO.Request
{
    public record OrderInventoryAddRequest
    {
        [JsonProperty("items")]
        public IList<OrderInventoryItem> Items { get; set; } = new List<OrderInventoryItem>();
    }

    public record OrderInventoryItem
    {
        [JsonProperty("inventory_id")]
        public string InventoryId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("sku")]
        public string SKU { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("unit_price")]
        public decimal UnitPrice { get; set; }

        [JsonProperty("line_total_discount")]
        public decimal LineTotalDiscount { get; set; }

        [JsonProperty("options")]
        public string Options { get; set; }

        [JsonProperty("notes")]
        public string Notes { get; set; }

        [JsonProperty("hs_code")]
        public string HsCode { get; set; }

        [JsonProperty("country_of_origin")]
        public string CountryOfOrigin { get; set; }

        [JsonProperty("image_url")]
        public string ImageUrl { get; set; }
    }

}
