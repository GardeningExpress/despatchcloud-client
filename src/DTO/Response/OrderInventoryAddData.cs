using GardeningExpress.DespatchCloudClient.DTO.Request;
using Newtonsoft.Json;
using System.Collections.Generic;


namespace GardeningExpress.DespatchCloudClient.DTO.Response
{
    public class OrderInventoryAddData
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("validation_error")]
        public IList<string> ValidationError { get; set; }

        [JsonProperty("result")]
        public OrderInventoryAddDataResult Result { get; set; }
    }

    public class OrderInventoryAddDataResult
    {
        [JsonProperty("items")]
        public IList<OrderInventory> Items { get; set; } = new List<OrderInventory>();

        [JsonProperty("errors")]
        public IList<OrderInventoryAddError> Errors { get; set; }
    }

    public class OrderInventoryAddError
    {
        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("input")]
        public OrderInventoryItem Input { get; set; }
    }

    public class OrderInventoryAddDataResultItem
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("inventory_id")]
        public int InventoryId { get; set; }

        [JsonProperty("order_summary_id")]
        public int OrderSummaryId { get; set; }

        [JsonProperty("merge_order_summary_id")]
        public int? MergeOrderSummaryId { get; set; } // Nullable for potential null values

        [JsonProperty("sales_channel_item_id")]
        public int? SalesChannelItemId { get; set; } // Nullable for potential null values

        [JsonProperty("sku")]
        public string Sku { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("unit_price")]
        public decimal UnitPrice { get; set; }

        [JsonProperty("unit_tax")]
        public decimal UnitTax { get; set; }

        [JsonProperty("line_total_discount")]
        public decimal LineTotalDiscount { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

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
