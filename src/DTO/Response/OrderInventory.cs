using Newtonsoft.Json;

namespace GardeningExpress.DespatchCloudClient.DTO.Response
{
    public class OrderInventory
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("inventory_id")]
        public int InventoryId { get; set; }

        [JsonProperty("order_summary_id")]
        public int OrderSummaryId { get; set; }

        [JsonProperty("merge_order_summary_id")]
        public string MergeOrderSummaryId { get; set; }

        [JsonProperty("sales_channel_item_id")]
        public string SalesChannelItemId { get; set; }

        [JsonProperty("sku")]
        public string SKU { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("unit_price")]
        public string UnitPrice { get; set; }

        [JsonProperty("unit_tax")]
        public string UnitTax { get; set; }

        [JsonProperty("line_total_discount")]
        public string LineTotalDiscount { get; set; }

        [JsonProperty("price")]
        public string Price { get; set; }

        [JsonProperty("options")]
        public string Options { get; set; }

        [JsonProperty("notes")]
        public string Notes { get; set; }

        [JsonProperty("hs_code")]
        public string HsCode { get; set; }

        [JsonProperty("country_of_origin")]
        public string CountryOfOrigin { get; set; }

        [JsonProperty("customs_description")]
        public string CustomsDescription { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }
    }
}
