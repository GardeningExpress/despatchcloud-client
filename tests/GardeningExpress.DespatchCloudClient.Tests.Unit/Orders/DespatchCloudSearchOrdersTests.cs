using System;
using System.Collections.Generic;
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

namespace GardeningExpress.DespatchCloudClient.Tests.Unit.Orders
{
    [TestFixture]
    public class DespatchCloudSearchOrdersTests
    {
        private readonly DespatchCloudConfig _despatchCloudConfig = new DespatchCloudConfig
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
        public async Task Return_data_if_success()
        {
            var values = new
            {
                token = "token",
                email = "demo@mail.com"
            };

            var expectedData = new PagedResult<OrderData>()
            {
                CurrentPage = 1,
                Data = new List<OrderData> {
                    new OrderData { Email = values.email }
                }
            };

            _handler.SetupRequest(HttpMethod.Post, "https://fake.api/auth/login")
                .ReturnsResponse(JsonConvert.SerializeObject(values), "application/json");

            _handler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.Is<HttpRequestMessage>(r => r.Method == HttpMethod.Get && r.RequestUri.ToString().StartsWith("https://fake.api/orders")),
                ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    Content = new StringContent(JsonConvert.SerializeObject(expectedData))
                });


            var apiResponse = await _despatchCloudHttpClient.SearchOrdersAsync(
                new OrderSearchFilters
                {
                    Search = values.email
                });

            apiResponse.PagedResult.Data.ShouldNotBeEmpty();
            apiResponse.PagedResult.CurrentPage.ShouldBe<int>(1);
            apiResponse.PagedResult.Data[0].Email.ShouldBe(values.email);
        }

        [Test]
        public async Task Filters_applied()
        {
            var values = new
            {
                token = "token",
                email = "demo@mail.com"
            };

            var expectedData = new PagedResult<OrderData>
            {
                CurrentPage = 1,
                Data = new List<OrderData> {
                    new OrderData { Email = values.email, ChannelId = 3 }
                }
            };

            var filters = new OrderSearchFilters
            {
                Search = values.email,
                DateRange = new DateRangeFilter
                {
                    StartDate = new DateTime(2021, 11, 08),
                    EndDate = new DateTime(2021, 11, 09)
                },
                Sort = OrderSearchSort.name_za,
                FieldFilters = OrderSearchFieldFilters.search_name,
                Channel = "3"
            };

            _handler.SetupRequest(HttpMethod.Post, "https://fake.api/auth/login")
                .ReturnsResponse(JsonConvert.SerializeObject(values), "application/json");

            _handler.SetupRequest(HttpMethod.Get, $"https://fake.api/orders?{filters.GetQueryString()}")
                .ReturnsResponse(JsonConvert.SerializeObject(expectedData), "application/json");

            var apiResponse = await _despatchCloudHttpClient.SearchOrdersAsync(filters);

            filters.GetQueryString().ShouldBe("filters[search]=demo%40mail.com&filters[search_field]=search_name&filters[date_range]=1636329600%2c1636416000&filters[sales_channel]=3&sort=name_za&page=1");
            apiResponse.PagedResult.Data.ShouldNotBeEmpty();
            apiResponse.PagedResult.CurrentPage.ShouldBe<int>(1);
            apiResponse.PagedResult.Data[0].Email.ShouldBe(values.email);
            apiResponse.PagedResult.Data[0].ChannelId.ShouldBe(3);
        }
    }
}
