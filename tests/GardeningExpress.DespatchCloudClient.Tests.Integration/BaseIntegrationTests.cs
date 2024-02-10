using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using GardeningExpress.DespatchCloudClient.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace GardeningExpress.DespatchCloudClient.Tests.Integration
{
    public abstract class BaseIntegrationTests
    {
        private const string ApiBaseUrl = "https://gardeningexpresssb.despatchcloud.co.uk/public-api";
        
        protected string LoginPassword = "";
        protected string LoginEmailAddress = "";

        protected IDespatchCloudHttpClient DespatchCloudHttpClient;

        [SetUp]
        public void SetUp()
        {
            LoginPassword = "";
            LoginEmailAddress = "";

            CreateHttpClient();
        }

        [Test]
        public async Task Throws_ApiAuthenticationException_When_Auth_Fails()
        {
            // Arrange
            LoginPassword = "fake";
            LoginEmailAddress = "fake";

            CreateHttpClient();

            // Act / Assert
            try
            {
                await MethodForAuthTest();
            }
            catch (NotImplementedException)
            {
                // ignore
            }
            catch (ApiAuthenticationException)
            {
                // this is what we want
                Assert.Pass();
            }
            catch (Exception ex)
            {
                Assert.Fail();
            }
        }

        protected abstract Task MethodForAuthTest();

        private void CreateHttpClient()
        {
            var userSecretsConfig = new ConfigurationBuilder()
                .AddUserSecrets<SearchInventoryAsyncTests>()
                .Build();

            if (string.IsNullOrEmpty(LoginEmailAddress))
                LoginEmailAddress = userSecretsConfig["LoginEmailAddress"];
            if (string.IsNullOrEmpty(LoginPassword))
                LoginPassword = userSecretsConfig["LoginPassword"];

            if (string.IsNullOrEmpty(LoginEmailAddress))
                throw new Exception("LoginEmailAddress is null");
            if (string.IsNullOrEmpty(LoginPassword))
                throw new Exception("LoginPassword is null");

            var appSettings = JsonConvert.SerializeObject(new
            {
                DespatchCloud = new
                {
                    Environment = "sandbox",
                    ApiBaseUrl = ApiBaseUrl,
                    LoginEmailAddress = LoginEmailAddress,
                    LoginPassword = LoginPassword
                }
            });

            var builder = new ConfigurationBuilder()
                .AddJsonStream(new MemoryStream(Encoding.UTF8.GetBytes(appSettings)));

            IServiceCollection services = new ServiceCollection();
            services.AddScoped<IConfiguration>(_ => builder.Build());
            services.AddDespatchCloudClient();

            var mockLogger = new Mock<ILogger<GetDespatchCloudAuthenticationTokenByLoggingIn>>();
            services.AddSingleton(mockLogger.Object);

            DespatchCloudHttpClient = services
                .BuildServiceProvider()
                .GetRequiredService<IDespatchCloudHttpClient>();
        }
    }
}