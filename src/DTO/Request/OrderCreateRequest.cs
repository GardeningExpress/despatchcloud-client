using System.Collections.Generic;
using Newtonsoft.Json;

namespace GardeningExpress.DespatchCloudClient.DTO.Request
{
    public record OrderCreateRequest
    {
        [JsonProperty("status_id")]
        public int StatusId { get; set; }

        [JsonProperty("date_received")]
        public string DateReceived { get; set; }

        [JsonProperty("shipping_method_requested")]
        public string ShippingMethodRequested { get; set; }

        [JsonProperty("one_off_shipment")]
        public string OneOffShipment { get; set; }

        [JsonProperty("channel_alt_id")]
        public string ChannelAltId { get; set; }

        [JsonProperty("payment_method")]
        public string PaymentMethod { get; set; }

        [JsonProperty("payment_ref")]
        public string PaymentRef { get; set; }

        [JsonProperty("payment_currency")]
        public string PaymentCurrency { get; set; }

        [JsonProperty("total_paid")]
        public string TotalPaid { get; set; }

        [JsonProperty("total_discount")]
        public string TotalDiscount { get; set; }

        [JsonProperty("total_tax")]
        public string TotalTax { get; set; }

        [JsonProperty("total_weight")]
        public string TotalWeight { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phone_one")]
        public string PhoneOne { get; set; }

        [JsonProperty("contact_id")]
        public string ContactId { get; set; }

        [JsonProperty("vat_number")]
        public string VatNumber { get; set; }

        [JsonProperty("eori_number")]
        public string EoriNumber { get; set; }

        [JsonProperty("tax_id")]
        public string TaxId { get; set; }

        [JsonProperty("shipping_name")]
        public string ShippingName { get; set; }

        [JsonProperty("shipping_name_company")]
        public string ShippingNameCompany { get; set; }

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

        [JsonProperty("invoice_name")]
        public string InvoiceName { get; set; }

        [JsonProperty("invoice_name_company")]
        public string InvoiceNameCompany { get; set; }

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


        [JsonProperty("customer_comments")]
        public string CustomerComments { get; set; }

        [JsonProperty("notes")]
        public List<string> Notes { get; set; }

        [JsonProperty("custom_fields")]
        public Dictionary<string, string> CustomFields { get; set; }

        [JsonProperty("fulfilment_client_id")]
        public int? FulfilmentClientId { get; set; }

        [JsonProperty("shipping_paid")]
        public string ShippingPaid { get; set; }

        [JsonProperty("manual_channel_id")]
        public int ManualChannelId { get; set; }

        [JsonProperty("channel_order_id")]
        public string ChannelOrderId { get; set; }
    }
}