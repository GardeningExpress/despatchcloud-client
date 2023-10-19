using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GardeningExpress.DespatchCloudClient.DTO;
using Moq;
using Moq.Contrib.HttpClient;
using Moq.Protected;
using Newtonsoft.Json;
using NUnit.Framework;
using Shouldly;

namespace GardeningExpress.DespatchCloudClient.Tests.Unit.Inventory
{
    [TestFixture]
    public class DespatchCloudSearchInventoryTests
    {
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
        public async Task Handles_520_response()
        {
            var values = new
            {
                token = "token",
                email = "demo@mail.com"
            };

            _handler.SetupRequest(HttpMethod.Post, "https://fake.api/auth/login")
                .ReturnsResponse(JsonConvert.SerializeObject(values), "application/json");

            _handler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.Is<HttpRequestMessage>(r =>
                        r.Method == HttpMethod.Get && r.RequestUri.ToString().StartsWith("https://fake.api/inventory")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsResponse((HttpStatusCode)520);

            var apiResponse = await _despatchCloudHttpClient.SearchInventoryAsync(
                new InventorySearchFilters
                {
                    SKU = "test"
                });

            apiResponse.Error.ShouldBe("Response from DespatchCloud: 520");
        }
    }
}