using GardeningExpress.DespatchCloudClient.DTO.Request;
using GardeningExpress.DespatchCloudClient.Helpers;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardeningExpress.DespatchCloudClient.Tests.Unit.Services
{
    public class ThirdPartyOrderServiceTests
    {
        private readonly string thirdPartyOrderJson = "{ \"platform\": \"GoGroopie.com\", \"deal_id\": \"11111\", \"product\": \"Test deal 4 Colours\", \"voucher_code\": \"1112223334\", \"redeem_date\": \"19-10-2019\", \"order_id\": \"1111111111\", \"price_options\": \"Coffee\", \"price\": \"10.99\", \"currency\": \"GBP\", \"full_name\": \"John Doe\", \"email\": \"john.doe@domain.com\", \"phone\": \"+333331111111\", \"house\": \"11\", \"street\": \"test street\", \"city\": \"London\", \"postcode\": \"SE15LB\", \"country_code\": \"GB\", \"sku\": \"01-0111\", \"pipe_deal_id\": \"11055\", \"postage_price\": \"2.99\", \"net_merchant_return\": \"5.99\"  }";

        [Test]
        public void ConvertThirdPartyOrderToOrderRequest_ShouldConvertToOrderRequest() 
        {
            // ARRANGE
            var order = JsonConvert.DeserializeObject<ThirdPartyOrderCreateRequest>(thirdPartyOrderJson);

            // ACT
            var result = ThirdPartyOrderHelper.ConvertThirdPartyOrderToOrderRequest(order);

            // ASSERT
            // Defaults
            Assert.AreEqual(1, result.StatusId);

            // Mappings

            // Pricing and customer
            Assert.AreEqual(order.Currency, result.PaymentCurrency);
            Assert.AreEqual(order.Price, result.TotalPaid);
            Assert.AreEqual(order.RedeemDate, result.DateReceived);
            Assert.AreEqual(order.Email, result.Email);
            Assert.AreEqual(order.PostagePrice, result.ShippingPaid);
            
            // Address
            Assert.AreEqual(order.House, result.ShippingAddressLineOne);
            Assert.AreEqual(order.Street, result.ShippingAddressLineTwo);
            Assert.AreEqual(order.Postcode, result.ShippingAddressPostcode);
            Assert.AreEqual(order.City, result.ShippingAddressCity);
            Assert.AreEqual(order.CountryCode, result.ShippingAddressIso);
            Assert.AreEqual(order.House, result.InvoiceAddressLineOne);
            Assert.AreEqual(order.Street, result.InvoiceAddressLineTwo);
            Assert.AreEqual(order.Postcode, result.InvoiceAddressPostcode);
            Assert.AreEqual(order.City, result.InvoiceAddressCity);
            Assert.AreEqual(order.CountryCode, result.InvoiceAddressIso);

        }
    }
}
