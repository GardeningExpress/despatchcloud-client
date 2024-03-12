using GardeningExpress.DespatchCloudClient.DTO.Request;
using GardeningExpress.DespatchCloudClient.Model.GoGroopie;
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

        private readonly string goGroopieProductJson = "{ \"platform\": \"GoGroopie.com\", \"deal_id\": \"11111\", \"product\": \"Test deal 4 Colours\", \"voucher_code\": \"1112223334\", \"redeem_date\": \"19-10-2019\", \"order_id\": \"1111111111\", \"price_options\": \"Coffee\", \"price\": \"10.99\", \"currency\": \"GBP\", \"full_name\": \"John Doe\", \"email\": \"john.doe@domain.com\", \"phone\": \"+333331111111\", \"house\": \"11\", \"street\": \"test street\", \"city\": \"London\", \"postcode\": \"SE15LB\", \"country_code\": \"GB\", \"sku\": \"01-0111\", \"pipe_deal_id\": \"11055\", \"postage_price\": \"2.99\", \"net_merchant_return\": \"5.99\"  }";

        private OrderCreateRequest newOrder;

        [SetUp]
        public void Setup()
        {
            newOrder = new OrderCreateRequest()
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
        }
                
        [Test]
        public async Task CreateOrderAsync_ShouldReturnIsSuccessAsTrueAndIdField_WhenSuccessful()
        {
            // ARRANGE

            // ACT
            var result = await DespatchCloudHttpClient.CreateOrderAsync(newOrder);

            // ASSERT
            Assert.IsTrue(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.NotNull(result.Data.Id); // Verify trackable id set
        }

        [Theory]
        [TestCase("PaymentMethod", null)]
        public async Task CreateOrderAsync_ShouldReturnIsSuccessAsFalse_WhenRequiredFieldMissing(string field, object setValue = null)
        {
            // ARRANGE
            TestUtils.SetPropertyValue(newOrder, field, setValue);

            // ACT
            var result = await DespatchCloudHttpClient.CreateOrderAsync(newOrder);

            // ASSERT
            Assert.IsFalse(result.IsSuccess);
        }

        [Test]
        public async Task CreateOrderAsync_WithGoGroopieProduct_ShouldReturnANewOrderResponse()
        {
            // ARRANGE
            var product = JsonConvert.DeserializeObject<Product>(goGroopieProductJson);
            var order = Helpers.ThirdPartyOrderHelper.ConvertGoGroopieProductToOrderRequest(product);

            // ACT
            var result = await DespatchCloudHttpClient.CreateOrderAsync(order);

            // ASSERT
            Assert.IsTrue(result.IsSuccess);
        }
    }
}
