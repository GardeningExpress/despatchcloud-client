using System.Collections.Generic;
using Newtonsoft.Json;

namespace GardeningExpress.DespatchCloudClient.DTO
{
    public class OrderData
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("status_id")]
        public int StatusId { get; set; }

        [JsonProperty("allocation_status_id")]
        public int AllocationStatusId { get; set; }

        [JsonProperty("channel_order_id")]
        public string ChannelOrderId { get; set; }

        [JsonProperty("chapi_id")]
        public string ChapiId { get; set; }

        [JsonProperty("channel_id")]
        public int ChannelId { get; set; }

        [JsonProperty("channel_username")]
        public string ChannelUsername { get; set; }

        [JsonProperty("channel_alt_id")]
        public string ChannelAltId { get; set; }

        [JsonProperty("is_merge")]
        public bool IsMerge { get; set; }

        [JsonProperty("merge_order_summary_id")]
        public string MergeOrderSummaryId { get; set; }

        [JsonProperty("is_split")]
        public bool IsSplit { get; set; }

        [JsonProperty("split_order_summary_id")]
        public string SplitOrderSummaryId { get; set; }

        [JsonProperty("is_sorted")]
        public bool IsSorted { get; set; }

        [JsonProperty("contact_id")]
        public int ContactId { get; set; }

        [JsonProperty("shipping_name_company")]
        public string ShippingNameCompany { get; set; }

        [JsonProperty("shipping_name")]
        public string ShippingName { get; set; }

        [JsonProperty("shipping_address_line_one")]
        public string ShippingAddressLineOne { get; set; }

        [JsonProperty("shipping_address_line_two")]
        public string ShippingAddressLineTwo { get; set; }

        [JsonProperty("shipping_address_city")]
        public string ShippingAddressCity { get; set; }

        [JsonProperty("shipping_address_county")]
        public string ShippingAddressCounty { get; set; }

        [JsonProperty("shipping_address_country")]
        public string ShippingAddressCountry { get; set; }

        [JsonProperty("shipping_address_postcode")]
        public string ShippingAddressPostcode { get; set; }

        [JsonProperty("shipping_address_iso")]
        public string ShippingAddressIso { get; set; }

        [JsonProperty("invoice_name_company")]
        public string InvoiceNameCompany { get; set; }

        [JsonProperty("invoice_name")]
        public string InvoiceName { get; set; }

        [JsonProperty("invoice_address_line_one")]
        public string InvoiceAddressLineOne { get; set; }

        [JsonProperty("invoice_address_line_two")]
        public string InvoiceAddressLineTwo { get; set; }

        [JsonProperty("invoice_address_city")]
        public string InvoiceAddressCity { get; set; }

        [JsonProperty("invoice_address_county")]
        public string InvoiceAddressCounty { get; set; }

        [JsonProperty("invoice_address_country")]
        public string InvoiceAddressCountry { get; set; }

        [JsonProperty("invoice_address_postcode")]
        public string InvoiceAddressPostcode { get; set; }

        [JsonProperty("invoice_address_iso")]
        public string InvoiceAddressIso { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phone_one")]
        public string PhoneOne { get; set; }

        [JsonProperty("phone_two")]
        public string PhoneTwo { get; set; }

        [JsonProperty("total_paid")]
        public string TotalPaid { get; set; }

        [JsonProperty("total_discount")]
        public string TotalDiscount { get; set; }

        [JsonProperty("total_tax")]
        public string TotalTax { get; set; }

        [JsonProperty("shipping_paid")]
        public string ShippingPaid { get; set; }

        [JsonProperty("shipping_method_requested")]
        public string ShippingMethodRequested { get; set; }

        [JsonProperty("shipping_tracking_code")]
        public string ShippingTrackingCode { get; set; }

        [JsonProperty("payment_method")]
        public string PaymentMethod { get; set; }

        [JsonProperty("payment_ref")]
        public string PaymentRef { get; set; }

        [JsonProperty("payment_currency")]
        public string PaymentCurrency { get; set; }

        [JsonProperty("sales_channel")]
        public string SalesChannel { get; set; }

        [JsonProperty("date_received")]
        public string DateReceived { get; set; }

        [JsonProperty("date_dispatched")]
        public string DateDispatched { get; set; }

        [JsonProperty("customer_comments")]
        public string CustomerComments { get; set; }

        [JsonProperty("gift_note")]
        public string GiftNote { get; set; }

        [JsonProperty("deleted")]
        public bool Deleted { get; set; }

        [JsonProperty("channel_updated")]
        public int ChannelUpdated { get; set; }

        [JsonProperty("channel_updated_time")]
        public int ChannelUpdatedTime { get; set; }

        [JsonProperty("total_weight")]
        public string TotalWeight { get; set; }

        [JsonProperty("one_off_shipment")]
        public int OneOffShipment { get; set; }

        [JsonProperty("shipment_id")]
        public string ShipmentId { get; set; }

        [JsonProperty("fulfilment_client_id")]
        public int FulfilmentClientId { get; set; }

        [JsonProperty("sync_informed")]
        public int SyncInformed { get; set; }

        [JsonProperty("total_inventory_count")]
        public int TotalInventoryCount { get; set; }

        [JsonProperty("total_inventory_quantity")]
        public int TotalInventoryQuantity { get; set; }

        [JsonProperty("stock_levels_adjusted")]
        public int StockLevelsAdjusted { get; set; }

        [JsonProperty("ioss_number")]
        public string IossNumber { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }

        [JsonProperty("inventory")]
        public List<Inventory> Inventory { get; set; }
        
        // public string[] shipment { get; set; }
    }
}