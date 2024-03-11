using GardeningExpress.DespatchCloudClient.DTO.Request;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GardeningExpress.DespatchCloudClient.Tests.Integration
{
    public class CreateOrderAsyncTests : BaseIntegrationTests
    {
        protected override Task MethodForAuthTest() =>
            DespatchCloudHttpClient
                .CreateOrderAsync(new OrderCreateRequest());

        private readonly string thirdPartyOrderJson = "{ \"platform\": \"GoGroopie.com\", \"deal_id\": \"11111\", \"product\": \"Test deal 4 Colours\", \"voucher_code\": \"1112223334\", \"redeem_date\": \"19-10-2019\", \"order_id\": \"1111111111\", \"price_options\": \"Coffee\", \"price\": \"10.99\", \"currency\": \"GBP\", \"full_name\": \"John Doe\", \"email\": \"john.doe@domain.com\", \"phone\": \"+333331111111\", \"house\": \"11\", \"street\": \"test street\", \"city\": \"London\", \"postcode\": \"SE15LB\", \"country_code\": \"GB\", \"sku\": \"01-0111\", \"pipe_deal_id\": \"11055\", \"postage_price\": \"2.99\", \"net_merchant_return\": \"5.99\"  }";

        [Test]
        public async void CreateOrderAsync()
        {
            // ARRANGE
            var newOrder = new OrderCreateRequest()
            {
                StatusId = 1,
                ShippingMethodRequested = "Test",
                PaymentMethod = "CC",
                PaymentRef = "CC-123",
                PaymentCurrency = "GBP",
                TotalPaid = "100",
                TotalDiscount = "0",
                TotalTax = "0",
                TotalWeight = "100",
                Email = "demo@despatchcloud.com",
                PhoneOne = "+905559998877",
                VatNumber = "V1",
                EoriNumber = "E1",
                TaxId = "T1",
                ShippingName = "Test Customer 2",
                ShippingAddressLineOne = "SAL 1",
                ShippingAddressLineTwo = "SAL 2",
                ShippingAddressCity = "Izmir",
                ShippingAddressCounty = "Ege",
                ShippingAddressCountry = "Turkey",
                ShippingAddressPostcode = "35090",
                ShippingAddressIso = "TR",
                InvoiceName = "Invoice Name 1",
                InvoiceAddressLineOne = "IAL 1",
                InvoiceAddressLineTwo = "IAL 2",
                InvoiceAddressCity = "Bursa",
                InvoiceAddressCounty = "Marmara",
                InvoiceAddressCountry = "Turkey",
                InvoiceAddressPostcode = "16300",
                InvoiceAddressIso = "TR",
                CustomerComments = "Test 1 Comment",
                Notes = new List<string> {
                    "Test 1 Note"
                },
                CustomFields = new Dictionary<string, string>() {
                    { "test1-1","Test Data" }
                },
                FulfilmentClientId = null,
                ShippingPaid = "2.00",
                ManualChannelId = 1

            };

            // ACT
            var result = await DespatchCloudHttpClient.CreateOrderAsync(newOrder);

            // ASSERT
            Assert.IsTrue(result.IsSuccess);
        }

        [Test]
        public async void CreateThirdPartyOrderAsync_ShouldReturnANewOrderResponse()
        {
            // ARRANGE
            var order = JsonConvert.DeserializeObject<ThirdPartyOrderCreateRequest>(thirdPartyOrderJson);

            // ACT
            var result = await DespatchCloudHttpClient.CreateThirdPartyOrderAsync(order);

            // ASSERT
            Assert.IsTrue(result.IsSuccess);
        }
    }
}
