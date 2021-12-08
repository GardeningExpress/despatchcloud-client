using System;
using System.Net.Http;
using System.Threading.Tasks;
using GardeningExpress.DespatchCloudClient.Auth;
using GardeningExpress.DespatchCloudClient.DTO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace GardeningExpress.DespatchCloudClient.Tests.Integration
{
    public class SearchInventoryAsyncTests
    {
        private string ApiBaseUrl = "https://gardeningexpresssb.despatchcloud.co.uk/public-api";
        private string LoginPassword = "";
        private string LoginEmailAddress = "";

        private IConfiguration Configuration { get; set; }

        private DespatchCloudHttpClient _despatchCloudHttpClient;

        [SetUp]
        public void SetUp()
        {
            var builder = new ConfigurationBuilder()
                .AddUserSecrets<SearchInventoryAsyncTests>();

            Configuration = builder.Build();

            IServiceCollection services = new ServiceCollection();
            
            IConfiguration configuration = builder.Build();

            services.AddScoped<IConfiguration>(_ => configuration);

            
            services.AddDespatchCloudClient();

            
            
            if (string.IsNullOrEmpty(LoginEmailAddress))
                LoginEmailAddress = Configuration["LoginEmailAddress"];

            if (string.IsNullOrEmpty(LoginPassword))
                LoginPassword = Configuration["LoginPassword"];

            CreateHttpClient();
        }

        private void CreateHttpClient()
        {
            if (string.IsNullOrEmpty(LoginEmailAddress))
                throw new Exception("LoginEmailAddress is null");
            if (string.IsNullOrEmpty(LoginPassword))
                throw new Exception("LoginPassword is null");

            

            
            var mockOptions = new Mock<IOptionsMonitor<DespatchCloudConfig>>();
            mockOptions.SetupGet(x => x.CurrentValue)
                .Returns(new DespatchCloudConfig
                {
                    ApiBaseUrl = ApiBaseUrl,
                    LoginPassword = LoginPassword,
                    LoginEmailAddress = LoginEmailAddress
                });

            var mockLogger = new Mock<ILogger<GetDespatchCloudAuthenticationTokenByLoggingIn>>();

            _client  = HttpClientFactory.Create(new CKEnterprise.Common.CkApiMessageHandler(email, password))

                
            var authHttpClient = new HttpClient
            {
                BaseAddress = new Uri(mockOptions.Object.CurrentValue.ApiBaseUrl)
            };

            var getDespatchCloudAuthenticationToken = new GetDespatchCloudAuthenticationTokenByLoggingIn(
                authHttpClient,
                mockOptions.Object,
                mockLogger.Object
            );

            var addAuthTokenHandler = new AddAuthTokenHandler(getDespatchCloudAuthenticationToken);

            var httpClient = new HttpClient(addAuthTokenHandler)
            {
                BaseAddress = new Uri(mockOptions.Object.CurrentValue.ApiBaseUrl)
            };

            _despatchCloudHttpClient = new DespatchCloudHttpClient(httpClient);
        }

        [Test]
        public void Throws_ApiAuthenticationException_When_Auth_Fails()
        {
            // Arrange
            LoginPassword = "secret";
            LoginEmailAddress = "test@test.com";

            CreateHttpClient();

            // Act / Assert
            var inventorySearchFilters = new InventorySearchFilters();

            Assert.ThrowsAsync<ApiAuthenticationException>(() => _despatchCloudHttpClient
                .SearchInventoryAsync(inventorySearchFilters));
        }

        [Test]
        public async Task Returns_Inventory_For_Sku()
        {
            var inventorySearchFilters = new InventorySearchFilters();
            inventorySearchFilters.SKU = "DEAL16071";

            var result = await _despatchCloudHttpClient
                .SearchInventoryAsync(inventorySearchFilters);

            result.PagedResult.Data.Count.ShouldBe(1);
        }
    }
}