namespace GardeningExpress.DespatchCloudClient.DTO
{
    public class Inventory
    {
        public int id { get; set; }
        public int inventory_id { get; set; }
        public int order_summary_id { get; set; }
        public string merge_order_summary_id { get; set; }
        public string sales_channel_item_id { get; set; }
        public string sku { get; set; }
        public int quantity { get; set; }
        public string name { get; set; }
        public string unit_price { get; set; }
        public string unit_tax { get; set; }
        public string line_total_discount { get; set; }
        public string price { get; set; }
        public string options { get; set; }
        public string notes { get; set; }
        public string hs_code { get; set; }
        public string country_of_origin { get; set; }
        public string customs_description { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
    }
}
