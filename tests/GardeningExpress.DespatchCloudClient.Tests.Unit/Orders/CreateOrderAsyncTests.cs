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
    public class CreateOrderAsyncTests
    {
        private readonly string thirdPartyOrderJson = "{ \"platform\": \"GoGroopie.com\", \"deal_id\": \"11111\", \"product\": \"Test deal 4 Colours\", \"voucher_code\": \"1112223334\", \"redeem_date\": \"19-10-2019\", \"order_id\": \"1111111111\", \"price_options\": \"Coffee\", \"price\": \"10.99\", \"currency\": \"GBP\", \"full_name\": \"John Doe\", \"email\": \"john.doe@domain.com\", \"phone\": \"+333331111111\", \"house\": \"11\", \"street\": \"test street\", \"city\": \"London\", \"postcode\": \"SE15LB\", \"country_code\": \"GB\", \"sku\": \"01-0111\", \"pipe_deal_id\": \"11055\", \"postage_price\": \"2.99\", \"net_merchant_return\": \"5.99\"  }";

        private readonly DespatchCloudConfig _despatchCloudConfig = new DespatchCloudConfig
        {
            ApiBaseUrl = "https://fake.api",
            LoginPassword = "secret",
            LoginEmailAddress = "test@test.com"
        };

        private Mock<HttpMessageHandler> _handler;
        private DespatchCloudHttpClient _despatchCloudHttpClient;

        [SetUp]
        public void Setup()
        {
            _handler = new Mock<HttpMessageHandler>();
            var httpClient = _handler.CreateClient();
            httpClient.BaseAddress = new Uri(_despatchCloudConfig.ApiBaseUrl);

            _despatchCloudHttpClient = new DespatchCloudHttpClient(httpClient);
        }

        [Test]
        public async Task CreateThirdPartyOrderAsync_ShouldCallDespatchCloudCreateOrder_WithConvertedData()
        {
            // ARRANGE
            var uriString = $"{_despatchCloudConfig.ApiBaseUrl}orders/create";
            var uri = new Uri(uriString);
            var order = JsonConvert.DeserializeObject<ThirdPartyOrderCreateRequest>(thirdPartyOrderJson);

            _handler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.Is<HttpRequestMessage>(r => r.RequestUri == uri && r.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(System.Net.HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(new ApiResponse()), Encoding.UTF8)
                })
                .Verifiable();

            // ACT
            var result = await _despatchCloudHttpClient.CreateThirdPartyOrderAsync(order);

            // ASSERT
            // Make sure the create order endpoint on despatch cloud hit
            _handler.Protected().Verify("SendAsync", Times.Once(),
                ItExpr.Is<HttpRequestMessage>(r => r.RequestUri == new Uri(uriString) && r.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>());
        }


    }
}
