using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using GardeningExpress.DespatchCloudClient.Auth;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Contrib.HttpClient;
using Newtonsoft.Json;
using NUnit.Framework;
using Shouldly;

namespace GardeningExpress.DespatchCloudClient.Tests.Unit.Auth
{
    [TestFixture]
    public class GetDespatchCloudAuthenticationTokenByLoggingInShould
    {
        private readonly DespatchCloudConfig _despatchCloudConfig = new DespatchCloudConfig
        {
            ApiBaseUrl = "https://despatch.cloud/api",
            LoginPassword = "password",
            LoginEmailAddress = "email@domain.com"
        };

        private GetDespatchCloudAuthenticationTokenByLoggingIn _getDespatchCloudAuthenticationTokenByLoggingIn;
        private Mock<HttpMessageHandler> _handler;

        [SetUp]
        public void SetUp()
        {
            // All requests made with HttpClient go through its handler's SendAsync() which we mock
            _handler = new Mock<HttpMessageHandler>();
            var httpClient = _handler.CreateClient();
            httpClient.BaseAddress = new Uri("https://test.go");

            var mockOptions = new Mock<IOptionsMonitor<DespatchCloudConfig>>();
            mockOptions.SetupGet(x => x.CurrentValue)
                .Returns(_despatchCloudConfig);

            var mockLogger = new Mock<ILogger<GetDespatchCloudAuthenticationTokenByLoggingIn>>();

            _getDespatchCloudAuthenticationTokenByLoggingIn = new GetDespatchCloudAuthenticationTokenByLoggingIn(
                httpClient,
                mockOptions.Object, mockLogger.Object
            );
        }

        [Test]
        public void Throw_ApiAuthenticationException_if_DespatchCloud_returns_401()
        {
            // Arrange
            _handler.SetupAnyRequest()
                .ReturnsResponse(HttpStatusCode.Unauthorized);

            // Act
            var exception = Assert
                .ThrowsAsync<ApiAuthenticationException>(() => _getDespatchCloudAuthenticationTokenByLoggingIn.GetTokenAsync());

            exception.Message.ShouldBe("Could not get DespatchCloud Token - Invalid Credentials");
        }

        [Test]
        public async Task Return_token_if_success()
        {
            var tokenResponse = new
            {
                token = "this.a.token"
            };

            _handler.SetupAnyRequest()
                .ReturnsResponse(JsonConvert.SerializeObject(tokenResponse), "application/json");

            // Act
            var token = await _getDespatchCloudAuthenticationTokenByLoggingIn.GetTokenAsync();

            token.ShouldBe("this.a.token");
        }
    }
}