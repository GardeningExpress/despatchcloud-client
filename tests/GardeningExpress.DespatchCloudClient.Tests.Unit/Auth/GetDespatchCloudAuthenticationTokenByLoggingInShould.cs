using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GardeningExpress.DespatchCloudClient.Auth;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Contrib.HttpClient;
using Moq.Protected;
using Newtonsoft.Json;
using NUnit.Framework;
using Shouldly;

namespace GardeningExpress.DespatchCloudClient.Tests.Unit.Auth
{
    [TestFixture]
    public class GetDespatchCloudAuthenticationTokenByLoggingInShould
    {
        private DespatchCloudConfig _despatchCloudConfig;

        private GetDespatchCloudAuthenticationTokenByLoggingIn _getDespatchCloudAuthenticationTokenByLoggingIn;
        private Mock<HttpMessageHandler> _handler;
        private Mock<IMemoryCache> _mockMemoryCache;

        [SetUp]
        public void SetUp()
        {
            // All requests made with HttpClient go through its handler's SendAsync() which we mock
            _handler = new Mock<HttpMessageHandler>();
            var httpClient = _handler.CreateClient();
            httpClient.BaseAddress = new Uri("https://test.go");

            _despatchCloudConfig = new DespatchCloudConfig
            {
                ApiBaseUrl = "https://despatch.cloud/api",
                LoginPassword = "password",
                LoginEmailAddress = "email@domain.com"
            };

            var mockOptions = new Mock<IOptionsMonitor<DespatchCloudConfig>>();
            mockOptions.SetupGet(x => x.CurrentValue)
                .Returns(_despatchCloudConfig);

            var mockLogger = new Mock<ILogger<GetDespatchCloudAuthenticationTokenByLoggingIn>>();
            
            _mockMemoryCache = new Mock<IMemoryCache>();
            _getDespatchCloudAuthenticationTokenByLoggingIn = new GetDespatchCloudAuthenticationTokenByLoggingIn(
                httpClient, mockOptions.Object, mockLogger.Object, _mockMemoryCache.Object
            );

        }

        [TestCase("email@domain.com", "")]
        [TestCase("email@domain.com", null)]
        public void Throw_ApiAuthenticationException_If_Email_Or_Password_Is_Null_Or_Empty(string loginEmailAddress, string loginPassword)
        {
            var tokenResponse = new
            {
                token = "this.a.token"
            };

            _handler.SetupAnyRequest()
                .ReturnsResponse(JsonConvert.SerializeObject(tokenResponse), "application/json");
            
            _despatchCloudConfig.LoginPassword = loginEmailAddress;
            _despatchCloudConfig.LoginPassword = loginPassword;

            // Act
            var exception = Assert
                .ThrowsAsync<ApiAuthenticationException>(() => _getDespatchCloudAuthenticationTokenByLoggingIn.GetTokenAsync());

            exception.Message.ShouldBe("Error authenticating with DespatchCloud");
            exception.InnerException.Message.ShouldBe("DespatchCloud password not set in config");
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

        [Test]
        public async Task GetTokenAsync_ShouldNotCallLoginEndpointIfCachedTokenExissts()
        {
            var cachedToken = "cached.token";
            var tokenResponse = new
            {
                token = "this.a.token"
            };
            var uriString = $"{_despatchCloudConfig.ApiBaseUrl}login";
            var uri = new Uri(uriString);
            var expectedValue = cachedToken as Object;
            _mockMemoryCache.Setup(r => r.TryGetValue(It.IsAny<Object>(), out expectedValue)).Returns(true);

            _handler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.Is<HttpRequestMessage>(r => r.RequestUri == uri && r.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(tokenResponse), Encoding.UTF8)
                })
                .Verifiable();

            // ACT
            var token = await _getDespatchCloudAuthenticationTokenByLoggingIn.GetTokenAsync();

            // ASSERT
            _handler.Protected().Verify("SendAsync", Times.Never(),
               ItExpr.Is<HttpRequestMessage>(r => r.RequestUri == new Uri(uriString) && r.Method == HttpMethod.Post),
               ItExpr.IsAny<CancellationToken>());
            token.ShouldBe(cachedToken);
        }
    }
}