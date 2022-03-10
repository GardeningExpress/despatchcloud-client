using System.Threading.Tasks;
using NUnit.Framework;
using Shouldly;

namespace GardeningExpress.DespatchCloudClient.Tests.Integration
{
    public class GetInventoryBySKUAsyncTests : BaseIntegrationTests
    {
        protected override Task MethodForAuthTest() =>
            DespatchCloudHttpClient
                .GetInventoryBySKUAsync("anything");

        [Test]
        public async Task Returns_Inventory_For_SKU()
        {
            var result = await DespatchCloudHttpClient
                .GetInventoryBySKUAsync("DEAL16071");

            result.ShouldNotBeNull();
            result.Data.ShouldNotBeNull();
            result.Data.Name.ShouldNotBeNullOrEmpty();
            result.Data.Name.ShouldStartWith("Prunus triloba");
        }

        [Test]
        public async Task Returns_Inventory_For_SKU_With_Malformed_CustomFields()
        {
            var result = await DespatchCloudHttpClient
                .GetInventoryBySKUAsync("PP23235");

            result.ShouldNotBeNull();
            result.Data.ShouldNotBeNull();
            result.Data.Name.ShouldNotBeNullOrEmpty();
            result.Data.Name.ShouldStartWith("Bellis Hot Pink");
            
            result.Data.CustomFields.ShouldBeEmpty();
        }

        [Test]
        public async Task Returns_Null_For_SKU_That_Does_Not_Exist()
        {
            var result = await DespatchCloudHttpClient
                .GetInventoryBySKUAsync("not_exist");

            result.Data.ShouldBeNull();
        }
    }
}