using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GardeningExpress.DespatchCloudClient.DTO.Request;
using Moq;
using Moq.Contrib.HttpClient;
using Moq.Protected;
using Newtonsoft.Json;
using NUnit.Framework;

namespace GardeningExpress.DespatchCloudClient.Tests.Unit.Orders
{
    [TestFixture]
    public class CreateOrderAsyncTests
    {

        private readonly DespatchCloudConfig _despatchCloudConfig = new DespatchCloudConfig
        {
            ApiBaseUrl = "https://fake.api",
            LoginPassword = "secret",
            LoginEmailAddress = "test@test.com"
        };

        private Mock<HttpMessageHandler> _handler;
        private DespatchCloudHttpClient _despatchCloudHttpClient;

        private OrderCreateRequest orderRequest = new OrderCreateRequest();

        [SetUp]
        public void Setup()
        {
            _handler = new Mock<HttpMessageHandler>();
            var httpClient = _handler.CreateClient();
            httpClient.BaseAddress = new Uri(_despatchCloudConfig.ApiBaseUrl);

            _despatchCloudHttpClient = new DespatchCloudHttpClient(httpClient);
        }

        [Test]
        public async Task CreateOrderAsync_ShouldCallDespatchCloudCreateOrderURI_WithOrderCreateRequest()
        {
            // ARRANGE
            var uriString = $"{_despatchCloudConfig.ApiBaseUrl}orders/create";
            var uri = new Uri(uriString);

            _handler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.Is<HttpRequestMessage>(r => r.RequestUri == uri && r.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(System.Net.HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(new ApiResponse()), Encoding.UTF8)
                })
                .Verifiable();

            // ACT
            var result = await _despatchCloudHttpClient.CreateOrderAsync(orderRequest);

            // ASSERT
            // Make sure the create order endpoint on despatch cloud hit
            _handler.Protected().Verify("SendAsync", Times.Once(),
                ItExpr.Is<HttpRequestMessage>(r => r.RequestUri == new Uri(uriString) && r.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>());
        }
    }
}
