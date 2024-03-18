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
        public async Task Returns_Inventory_For_SKU_With_CustomFields()
        {
            var result = await DespatchCloudHttpClient
                .GetInventoryBySKUAsync("DEAL16071");

            result.ShouldNotBeNull();
            result.Data.ShouldNotBeNull();
            result.Data.Name.ShouldNotBeNullOrEmpty();
            result.Data.Name.ShouldStartWith("Prunus triloba");
            result.Data.CustomFields.ShouldContainKeyAndValue("pot-size-1", "2-3 Litre");
        }
        
        [Test]
        public async Task Returns_Inventory_For_SKU_With_Empty_CustomFields()
        {
            var result = await DespatchCloudHttpClient
                .GetInventoryBySKUAsync("P800927");

            result.ShouldNotBeNull();
            result.Data.ShouldNotBeNull();
            result.Data.Name.ShouldNotBeNullOrEmpty();
            result.Data.Name.ShouldStartWith("Sempervivum Houseleeks");
            result.Data.CustomFields.ShouldBeEmpty();
        }
            
        [Test]
        //throwing exception 25th march 2022
        public async Task Returns_Inventory_For_SKU_S21041()
        {
            var result = await DespatchCloudHttpClient
                .GetInventoryBySKUAsync("S21041");

            result.ShouldNotBeNull();
            result.Data.ShouldNotBeNull();
            result.Data.Name.ShouldNotBeNullOrEmpty();
            result.Data.Name.ShouldStartWith("Syringa microphylla");
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