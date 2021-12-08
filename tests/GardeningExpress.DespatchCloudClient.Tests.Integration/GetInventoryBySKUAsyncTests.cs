using System.Threading.Tasks;
using GardeningExpress.DespatchCloudClient.Auth;
using GardeningExpress.DespatchCloudClient.DTO;
using NUnit.Framework;
using Shouldly;

namespace GardeningExpress.DespatchCloudClient.Tests.Integration
{
    public class GetInventoryBySKUAsyncTests : BaseIntegrationTests
    {
        [Test]
        public void Throws_ApiAuthenticationException_When_Auth_Fails()
        {
            // Arrange
            LoginPassword = "secret";
            LoginEmailAddress = "test@test.com";

            CreateHttpClient();

            // Act / Assert

            Assert.ThrowsAsync<ApiAuthenticationException>(() => DespatchCloudHttpClient
                .GetInventoryBySKUAsync("anything"));
        }

        [Test]
        public async Task Returns_Inventory_For_SKU()
        {
            var result = await DespatchCloudHttpClient
                .GetInventoryBySKUAsync("DEAL16071");

            result.ShouldNotBeNull();
            result.Name.ShouldStartWith("Prunus triloba");
        }

        [Test]
        public async Task Returns_Null_For_SKU_That_Does_Not_Exist()
        {
            var result = await DespatchCloudHttpClient
                .GetInventoryBySKUAsync("not_exist");

            result.ShouldBeNull();
        }
    }
}