using System.Collections.Generic;
using Newtonsoft.Json;

namespace GardeningExpress.DespatchCloudClient.DTO.Request
{
    public class OrderCreateRequest
    {
        [JsonProperty("status_id")]
        public int StatusId { get; set; } = 0;

        [JsonProperty("date_received")]
        public string DateReceived { get; set; } = string.Empty;

        [JsonProperty("shipping_method_requested")]
        public string ShippingMethodRequested { get; set; } = string.Empty;

        [JsonProperty("one_off_shipment")]
        public string OneOffShipment { get; set; } = string.Empty;

        [JsonProperty("channel_alt_id")]
        public string ChannelAltId { get; set; } = string.Empty;

        [JsonProperty("payment_method")]
        public string PaymentMethod { get; set; } = string.Empty;

        [JsonProperty("payment_ref")]
        public string PaymentRef { get; set; } = string.Empty;

        [JsonProperty("payment_currency")]
        public string PaymentCurrency { get; set; } = string.Empty;

        [JsonProperty("total_paid")]
        public string TotalPaid { get; set; } = string.Empty;

        [JsonProperty("total_discount")]
        public string TotalDiscount { get; set; } = string.Empty;

        [JsonProperty("total_tax")]
        public string TotalTax { get; set; } = string.Empty;

        [JsonProperty("total_weight")]
        public string TotalWeight { get; set; } = string.Empty;

        [JsonProperty("email")]
        public string Email { get; set; } = string.Empty;

        [JsonProperty("phone_one")]
        public string PhoneOne { get; set; } = string.Empty;

        [JsonProperty("contact_id")]
        public string ContactId { get; set; } = string.Empty;

        [JsonProperty("vat_number")]
        public string VatNumber { get; set; } = string.Empty;

        [JsonProperty("eori_number")]
        public string EoriNumber { get; set; } = string.Empty;

        [JsonProperty("tax_id")]
        public string TaxId { get; set; } = string.Empty;

        [JsonProperty("shipping_name")]
        public string ShippingName { get; set; } = string.Empty;

        [JsonProperty("shipping_name_company")]
        public string ShippingNameCompany { get; set; } = string.Empty;

        [JsonProperty("shipping_address_line_one")]
        public string ShippingAddressLineOne { get; set; } = string.Empty;

        [JsonProperty("shipping_address_line_two")]
        public string ShippingAddressLineTwo { get; set; } = string.Empty;

        [JsonProperty("shipping_address_city")]
        public string ShippingAddressCity { get; set; } = string.Empty;

        [JsonProperty("shipping_address_county")]
        public string ShippingAddressCounty { get; set; } = string.Empty;

        [JsonProperty("shipping_address_country")]
        public string ShippingAddressCountry { get; set; } = string.Empty;

        [JsonProperty("shipping_address_postcode")]
        public string ShippingAddressPostcode { get; set; } = string.Empty;

        [JsonProperty("shipping_address_iso")]
        public string ShippingAddressIso { get; set; } = string.Empty;

        [JsonProperty("invoice_name")]
        public string InvoiceName { get; set; } = string.Empty;

        [JsonProperty("invoice_name_company")]
        public string InvoiceNameCompany { get; set; } = string.Empty;

        [JsonProperty("invoice_address_line_one")]
        public string InvoiceAddressLineOne { get; set; } = string.Empty;

        [JsonProperty("invoice_address_line_two")]
        public string InvoiceAddressLineTwo { get; set; } = string.Empty;

        [JsonProperty("invoice_address_city")]
        public string InvoiceAddressCity { get; set; } = string.Empty;

        [JsonProperty("invoice_address_county")]
        public string InvoiceAddressCounty { get; set; } = string.Empty;

        [JsonProperty("invoice_address_country")]
        public string InvoiceAddressCountry { get; set; } = string.Empty;

        [JsonProperty("invoice_address_postcode")]
        public string InvoiceAddressPostcode { get; set; } = string.Empty;

        [JsonProperty("invoice_address_iso")]
        public string InvoiceAddressIso { get; set; } = string.Empty;


        [JsonProperty("customer_comments")]
        public string CustomerComments { get; set; } = string.Empty;

        [JsonProperty("notes")]
        public List<string> Notes { get; set; }

        [JsonProperty("custom_fields")]
        public Dictionary<string, string> CustomFields { get; set; }

        [JsonProperty("fulfilment_client_id")]
        public int? FulfilmentClientId { get; set; }

        [JsonProperty("shipping_paid")]
        public string ShippingPaid { get; set; } = string.Empty;

        [JsonProperty("manual_channel_id")]
        public int ManualChannelId { get; set; }
    }
}