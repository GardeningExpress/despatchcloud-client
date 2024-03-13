using GardeningExpress.DespatchCloudClient.DTO.Request;
using GardeningExpress.DespatchCloudClient.Model.GoGroopie;
using GardeningExpress.DespatchCloudClient.Tests.Integration.Utils;
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
            newOrder = TestUtils.GetCreateOrderRequest();
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
        [TestCase("PaymentMethod", "payment_method", null)]
        public async Task CreateOrderAsync_ShouldReturnIsSuccessAsFalseAndAnErrorMessage_WhenRequiredFieldMissing(string field, string jsonField, object setValue = null)
        {
            // ARRANGE
            TestUtils.SetPropertyValue(newOrder, field, setValue);

            // ACT
            var result = await DespatchCloudHttpClient.CreateOrderAsync(newOrder);

            // ASSERT
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(result.Error, $"Error executing statement: Column '{jsonField}' cannot be null");
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
            Assert.NotNull(result.Data);
            Assert.NotNull(result.Data.Id); // Verify trackable id set
        }
    }
}
