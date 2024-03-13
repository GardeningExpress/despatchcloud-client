using GardeningExpress.DespatchCloudClient.DTO.Request;
using GardeningExpress.DespatchCloudClient.DTO.Response;
using GardeningExpress.DespatchCloudClient.Tests.Integration.Utils;
using NUnit.Framework;
using System.Threading.Tasks;

namespace GardeningExpress.DespatchCloudClient.Tests.Integration
{
    public class AddInventoryToOrderAsyncTests : BaseIntegrationTests
    {
        protected override Task MethodForAuthTest() =>
         DespatchCloudHttpClient
             .CreateOrderAsync(new OrderCreateRequest());


        private readonly string testAddInventoryRequestJson = "{\"items\": [{\"inventory_id\": \"3333\", \"name\": \"MACBOOKAIR\",\"sku\": \"MACBOOKAIR\",\"quantity\": 1,\"unit_price\": 10, \"line_total_discount\": 2, \"options\": \"Colour: Pink\", \"notes\": \"TEst 1\", \"hs_code\": \"code33\", \"country_of_origin\": \"Japan\",  \"image_url\":\"https://media-exp1.licdn.com/dms/image/C4D0BAQFenp-2o4YZpA/company-logo_200_200/0/1594636780827?e=2147483647&v=beta&t=_UDcVrneIwMgxwvV3x8pEJifJSxkTW52B0bbln2Q86I\" }]}";

        private OrderData newOrderData;

        [SetUp]
        public async Task OneTimeSetup()
        {
            var newOrder = TestUtils.GetCreateOrderRequest();
            var newOrderResult = await DespatchCloudHttpClient.CreateOrderAsync(newOrder);
            newOrderData = newOrderResult.Data;
        }

        [Test]
        public async Task AddInventoryToOrderAsync_ShouldReturnIsSuccessAsTrue_AndStatusAsSuccess_OnValidEntry()
        {
            // ARRANGE
           
            var newOrderInventory = TestUtils.GetOrderInventoryAddRequest();

            // ACT
            var inventoryResponse = await DespatchCloudHttpClient.AddInventoryToOrderAsync(newOrderData.Id.ToString(), newOrderInventory);

            // ASSERT
            Assert.IsTrue(inventoryResponse.IsSuccess);
            Assert.AreEqual("success", inventoryResponse.Data.Status);
        }
    }
}
