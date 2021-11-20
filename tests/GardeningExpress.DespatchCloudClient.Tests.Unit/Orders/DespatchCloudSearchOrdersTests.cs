﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GardeningExpress.DespatchCloudClient.Auth;
using GardeningExpress.DespatchCloudClient.DTO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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

        private GetDespatchCloudAuthenticationTokenByLoggingIn _getDespatchCloudAuthenticationTokenByLoggingIn;
        private Mock<HttpMessageHandler> _handler;
        private DespatchCloudSearchOrders _DespatchCloudSearchOrders;

        [SetUp]
        public void SetUp()
        {
            // All requests made with HttpClient go through its handler's SendAsync() which we mock
            _handler = new Mock<HttpMessageHandler>();
            var httpClient = _handler.CreateClient();

            var mockOptions = new Mock<IOptionsMonitor<DespatchCloudConfig>>();
            mockOptions.SetupGet(x => x.CurrentValue)
                .Returns(_despatchCloudConfig);

            var mockLogger = new Mock<ILogger<GetDespatchCloudAuthenticationTokenByLoggingIn>>();

            _getDespatchCloudAuthenticationTokenByLoggingIn = new GetDespatchCloudAuthenticationTokenByLoggingIn(
                httpClient,
                mockOptions.Object, mockLogger.Object
            );

            var mockSearchLogger = new Mock<ILogger<DespatchCloudSearchOrders>>();

            _DespatchCloudSearchOrders = new DespatchCloudSearchOrders(httpClient,
                mockOptions.Object, mockSearchLogger.Object, _getDespatchCloudAuthenticationTokenByLoggingIn);
        }

        [Test]
        public async Task Return_data_if_success()
        {
            var values = new
            {
                token = "token",
                email = "demo@mail.com"
            };

            var expectedData = new DespatchCloudOrderDto { 
                current_page = 1,
                data = new List<DespatchCloudOrderData> { 
                    new DespatchCloudOrderData { email = values.email }
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


            var result = await _DespatchCloudSearchOrders.SearchOrdersAsync(
                new DespatchCloudOrderSearchFilters
                {
                    Search = values.email
                });

            result.data.ShouldNotBeEmpty();
            result.current_page.ShouldBe<int>(1);
            result.data[0].email.ShouldBe(values.email);
        }

        [Test]
        public async Task Filters_applied()
        {
            var values = new
            {
                token = "token",
                email = "demo@mail.com"
            };

            var expectedData = new DespatchCloudOrderDto
            {
                current_page = 1,
                data = new List<DespatchCloudOrderData> {
                    new DespatchCloudOrderData { email = values.email }
                }
            };

            var filters = new DespatchCloudOrderSearchFilters
            {
                Search = values.email,
                DateRange = new DateRangeFilter
                {
                    StartDate = new DateTime(2021, 11, 08),
                    EndDate = new DateTime(2021, 11, 09)
                },
                Sort = DespatchCloudOrderSearchSort.name_za,
                FieldFilters = DespatchCloudOrderSearchFieldFilters.search_name
            };

            _handler.SetupRequest(HttpMethod.Post, "https://fake.api/auth/login")
                .ReturnsResponse(JsonConvert.SerializeObject(values), "application/json");


            _handler.SetupRequest(HttpMethod.Get, $"https://fake.api/orders?{filters.GetQueryString()}")
                .ReturnsResponse(JsonConvert.SerializeObject(expectedData), "application/json");

            var result = await _DespatchCloudSearchOrders.SearchOrdersAsync(filters);

            result.data.ShouldNotBeEmpty();
            result.current_page.ShouldBe<int>(1);
            result.data[0].email.ShouldBe(values.email);
        }
    }
}