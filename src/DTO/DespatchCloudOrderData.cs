using System.Collections.Generic;

namespace GardeningExpress.DespatchCloudClient.DTO
{
    public class DespatchCloudOrderData
    {
        public int id { get; set; }
        public int status_id { get; set; }
        public int allocation_status_id { get; set; }
        public string channel_order_id { get; set; }
        public string chapi_id { get; set; }
        public int channel_id { get; set; }
        public string channel_username { get; set; }
        public string channel_alt_id { get; set; }
        public int is_merge { get; set; }
        public string merge_order_summary_id { get; set; }
        public int is_split { get; set; }
        public string split_order_summary_id { get; set; }
        public int is_sorted { get; set; }
        public int contact_id { get; set; }
        public string shipping_name_company { get; set; }
        public string shipping_name { get; set; }
        public string shipping_address_line_one { get; set; }
        public string shipping_address_line_two { get; set; }
        public string shipping_address_city { get; set; }
        public string shipping_address_county { get; set; }
        public string shipping_address_country { get; set; }
        public string shipping_address_postcode { get; set; }
        public string shipping_address_iso { get; set; }
        public string invoice_name_company { get; set; }
        public string invoice_name { get; set; }
        public string invoice_address_line_one { get; set; }
        public string invoice_address_line_two { get; set; }
        public string invoice_address_city { get; set; }
        public string invoice_address_county { get; set; }
        public string invoice_address_country { get; set; }
        public string invoice_address_postcode { get; set; }
        public string invoice_address_iso { get; set; }
        public string email { get; set; }
        public string phone_one { get; set; }
        public string phone_two { get; set; }
        public string total_paid { get; set; }
        public string total_discount { get; set; }
        public string total_tax { get; set; }
        public string shipping_paid { get; set; }
        public string shipping_method_requested { get; set; }
        public string shipping_tracking_code { get; set; }
        public string payment_method { get; set; }
        public string payment_ref { get; set; }
        public string payment_currency { get; set; }
        public string sales_channel { get; set; }
        public string date_received { get; set; }
        public string date_dispatched { get; set; }
        public string customer_comments { get; set; }
        public string gift_note { get; set; }
        public int deleted { get; set; }
        public int channel_updated { get; set; }
        public int channel_updated_time { get; set; }
        public string total_weight { get; set; }
        public int one_off_shipment { get; set; }
        public string shipment_id { get; set; }
        public int fulfilment_client_id { get; set; }
        public int sync_informed { get; set; }
        public int total_inventory_count { get; set; }
        public int total_inventory_quantity { get; set; }
        public int stock_levels_adjusted { get; set; }
        public string ioss_number { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public List<Inventory> inventory { get; set; }
        // public string[] shipment { get; set; }
    }
}