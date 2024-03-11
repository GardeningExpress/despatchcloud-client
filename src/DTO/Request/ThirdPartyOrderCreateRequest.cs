using Newtonsoft.Json;

namespace GardeningExpress.DespatchCloudClient.DTO.Request
{
    public class ThirdPartyOrderCreateRequest
    {
        [JsonProperty("platform")]
        public string Platform { get; set; }

        [JsonProperty("deal_id")]
        public string DealId { get; set; }

        [JsonProperty("product")]
        public string Product { get; set; }

        [JsonProperty("voucher_code")]
        public string VoucherCode { get; set; }

        [JsonProperty("redeem_date")]
        public string RedeemDate { get; set; }

        [JsonProperty("order_id")]
        public string OrderId { get; set; }

        [JsonProperty("price_options")]
        public string PriceOptions { get; set; }

        [JsonProperty("price")]
        public string Price { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("full_name")]
        public string FullName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("house")]
        public string House { get; set; }

        [JsonProperty("street")]
        public string Street { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("postcode")]
        public string Postcode { get; set; }

        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        [JsonProperty("sku")]
        public string SKU { get; set; }

        [JsonProperty("pipe_deal_id")]
        public string PipeDealId { get; set; }

        [JsonProperty("postage_price")]
        public string PostagePrice { get; set; }

        [JsonProperty("net_merchant_return")]
        public string NetMerchantReturn { get; set; }
    }
}
