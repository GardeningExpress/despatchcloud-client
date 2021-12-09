using System.Threading.Tasks;
using GardeningExpress.DespatchCloudClient.DTO;
using NUnit.Framework;
using Shouldly;

namespace GardeningExpress.DespatchCloudClient.Tests.Integration
{
    public class UpdateInventoryAsyncTests : BaseIntegrationTests
    {
        protected override Task MethodForAuthTest() =>
            DespatchCloudHttpClient
                .GetInventoryBySKUAsync("anything");

        
        [Test]
        public async Task Update_Name()
        {
            // this is a bit crap as we're getting the inventory item too
            var inventory = await DespatchCloudHttpClient
                .GetInventoryBySKUAsync("DEAL16071");

            var originalName = inventory.Data.Name;

            var request = new InventoryUpdateRequest
            {
                Name = "New name"
            };

            var updatedInventoryResult = await DespatchCloudHttpClient
                .UpdateInventoryAsync(inventory.Data.Id, request);

            updatedInventoryResult.IsSuccess.ShouldBeTrue();
            
            updatedInventoryResult.Data.Name.ShouldBe("New name");

            var inventoryAfterUpdate = await DespatchCloudHttpClient
                .GetInventoryBySKUAsync("DEAL16071");

            inventoryAfterUpdate.Data.Name.ShouldBe("New name");

            //reset
            var resetInventoryResult = await DespatchCloudHttpClient
                .UpdateInventoryAsync(inventory.Data.Id, new InventoryUpdateRequest { Name = originalName });
            
            var inventoryAfterReset = await DespatchCloudHttpClient
                .GetInventoryBySKUAsync("DEAL16071");

            inventoryAfterReset.Data.Name.ShouldBe(originalName);
        }
    }
}