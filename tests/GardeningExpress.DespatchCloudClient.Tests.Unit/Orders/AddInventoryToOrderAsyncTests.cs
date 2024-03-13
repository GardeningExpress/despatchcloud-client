using GardeningExpress.DespatchCloudClient.DTO.Request;
using Moq;
using Moq.Contrib.HttpClient;
using Moq.Protected;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GardeningExpress.DespatchCloudClient.Tests.Unit.Orders
{
    public class AddInventoryToOrderAsyncTests
    {
        private readonly string testJson = "{\"items\": [{\"inventory_id\": \"3333\", \"name\": \"MACBOOKAIR\",\"sku\": \"MACBOOKAIR\",\"quantity\": 1,\"unit_price\": 10, \"line_total_discount\": 2, \"options\": \"Colour: Pink\", \"notes\": \"TEst 1\", \"hs_code\": \"code33\", \"country_of_origin\": \"Japan\",  \"image_url\":\"https://media-exp1.licdn.com/dms/image/C4D0BAQFenp-2o4YZpA/company-logo_200_200/0/1594636780827?e=2147483647&v=beta&t=_UDcVrneIwMgxwvV3x8pEJifJSxkTW52B0bbln2Q86I\" }]}";

        private readonly DespatchCloudConfig _despatchCloudConfig = new()
        {
            ApiBaseUrl = "https://fake.api",
            LoginPassword = "secret",
            LoginEmailAddress = "test@test.com"
        };

        private Mock<HttpMessageHandler> _handler;
        private DespatchCloudHttpClient _despatchCloudHttpClient;

        [SetUp]
        public void SetUp()
        {
            // All requests made with HttpClient go through its handler's SendAsync() which we mock
            _handler = new Mock<HttpMessageHandler>();
            var httpClient = _handler.CreateClient();
            httpClient.BaseAddress = new Uri(_despatchCloudConfig.ApiBaseUrl);

            _despatchCloudHttpClient = new DespatchCloudHttpClient(httpClient);

        }

        [Test]
        public async Task AddInventoryToOrderAsync_ShouldDeseriliseData_ToStructureRequiedByDespatchCloud()
        {
            // ARRANGE

            // ACT
            var request = JsonConvert.DeserializeObject<OrderInventoryAddRequest>(testJson);

            // ASSERT
            Assert.AreEqual(1, request.Items.Count());
            var item = request.Items[0];
            Assert.AreEqual("3333", item.InventoryId);
            Assert.AreEqual("MACBOOKAIR", item.Name);
            Assert.AreEqual("MACBOOKAIR", item.SKU);
            Assert.AreEqual(1, item.Quantity);
            Assert.AreEqual(10, item.UnitPrice);
            Assert.AreEqual(2, item.LineTotalDiscount);
            Assert.AreEqual("Colour: Pink", item.Options);
            Assert.AreEqual("TEst 1", item.Notes);
            Assert.AreEqual("code33", item.HsCode);
            Assert.AreEqual("Japan", item.CountryOfOrigin);
            Assert.AreEqual("https://media-exp1.licdn.com/dms/image/C4D0BAQFenp-2o4YZpA/company-logo_200_200/0/1594636780827?e=2147483647&v=beta&t=_UDcVrneIwMgxwvV3x8pEJifJSxkTW52B0bbln2Q86I", item.ImageUrl);
        }


        [Test]
        public async Task AddInventoryToOrderAsync_ShouldCallCorrectDespatchCloudEndpoint_WithPostMethod()
        {
            // ARRANGE
            var uriString = $"{_despatchCloudConfig.ApiBaseUrl}order/88888888/add_inventory";
            var uri = new Uri(uriString);
            var addRequest = JsonConvert.DeserializeObject<OrderInventoryAddRequest>(testJson);

            _handler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.Is<HttpRequestMessage>(r => r.RequestUri == uri && r.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(System.Net.HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(new ApiResponse()), Encoding.UTF8)
                })
                .Verifiable();

            // ACT
            var result = await _despatchCloudHttpClient.AddInventoryToOrderAsync("88888888",addRequest);

            // ASSERT
            Assert.True(result.IsSuccess);
            _handler.Protected().Verify("SendAsync", Times.Once(),
                ItExpr.Is<HttpRequestMessage>(r => r.RequestUri == new Uri(uriString) && r.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>());
        }
    }
}
