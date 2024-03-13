using GardeningExpress.DespatchCloudClient.DTO.Request;
using GardeningExpress.DespatchCloudClient.DTO.Response;
using GardeningExpress.DespatchCloudClient.Tests.Integration.Utils;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Linq;
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
            newOrder.InvoiceName = "Integration Tests: AddInventoryToOrderAsync()";
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
            Assert.AreEqual(1, inventoryResponse.Data.Result.Items.Count);
            var responseInventoryData = inventoryResponse.Data.Result.Items[0];
            Assert.NotNull(responseInventoryData.InventoryId); // the id is created if not exists
            // Verify the fields related to the order as some are predefined by the inventory object
            Assert.AreEqual(newOrderInventory.Items[0].Name, responseInventoryData.Name);
            Assert.AreEqual(newOrderInventory.Items[0].Quantity, responseInventoryData.Quantity);
            Assert.AreEqual(newOrderInventory.Items[0].SKU, responseInventoryData.SKU);
            Assert.AreEqual(newOrderInventory.Items[0].Options, responseInventoryData.Options);
            Assert.AreEqual(newOrderInventory.Items[0].Notes, responseInventoryData.Notes);
 
        }

        [Test]
        public async Task AddInventoryToOrderAsync_ShouldReturnIsSuccessAsFalse_AndValidationError_OnEmptySetEntry()
        {
            // ARRANGE
            var newOrderInventory = new OrderInventoryAddRequest();

            // ACT
            var inventoryResponse = await DespatchCloudHttpClient.AddInventoryToOrderAsync(newOrderData.Id.ToString(), newOrderInventory);

            // ASSERT
            Assert.IsFalse(inventoryResponse.IsSuccess);
            Assert.AreEqual("Item's field is not valid", inventoryResponse.Error);
            Assert.Null(inventoryResponse.Data);
        }


        [Test]
        public async Task AddInventoryToOrderAsync_ShouldReturnIsSuccessAsTrueStatusAsPartialSuccess_AndItemWithErrorMessage_WhenInventoryItemMissingRequiredFIeld()
        {
            // ARRANGE
            var newOrderInventory = TestUtils.GetOrderInventoryAddRequest();
            newOrderInventory.Items[0].SKU = null;

            // ACT
            var inventoryResponse = await DespatchCloudHttpClient.AddInventoryToOrderAsync(newOrderData.Id.ToString(), newOrderInventory);

            // ASSERT
            Assert.IsTrue(inventoryResponse.IsSuccess);
            Assert.AreEqual("partial_success", inventoryResponse.Data.Status);
            Assert.AreEqual(1, inventoryResponse.Data.Result.Errors.Count());
            Assert.AreEqual("sku field is required.", inventoryResponse.Data.Result.Errors[0].Error);
            Assert.AreEqual(JsonConvert.SerializeObject(newOrderInventory.Items[0]), JsonConvert.SerializeObject(inventoryResponse.Data.Result.Errors[0].Input));
        }

        [Test]
        public async Task AddInventoryToOrderAsync_ShouldReturnIsSuccessAsFalseAndOrderNotFoundMessage_WhenOrderNumberInvalid()
        {
            // ARRANGE
            var newOrderInventory = TestUtils.GetOrderInventoryAddRequest();

            // ACT
            var inventoryResponse = await DespatchCloudHttpClient.AddInventoryToOrderAsync("-222", newOrderInventory);

            // ASSERT
            Assert.IsFalse(inventoryResponse.IsSuccess);
            Assert.AreEqual("Order Not Found", inventoryResponse.Error);
        }
    }
}
