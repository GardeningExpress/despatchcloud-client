using System.Linq;
using System.Threading.Tasks;
using GardeningExpress.DespatchCloudClient.DTO;
using GardeningExpress.DespatchCloudClient.DTO.Filter;
using NUnit.Framework;
using Shouldly;

namespace GardeningExpress.DespatchCloudClient.Tests.Integration
{
    public class SearchInventoryAsyncTests : BaseIntegrationTests
    {
        protected override Task MethodForAuthTest()
        {
            return DespatchCloudHttpClient
                .SearchInventoryAsync(new InventorySearchFilters());
        }

        [Test]
        public async Task Returns_Inventory_For_SKU()
        {
            var inventorySearchFilters = new InventorySearchFilters
            {
                SKU = "DEAL16071"
            };

            var result = await DespatchCloudHttpClient
                .SearchInventoryAsync(inventorySearchFilters);

            result.PagedData.Data.Count.ShouldBe(1);
            result.PagedData.Data.First().Name.ShouldStartWith("Prunus triloba");
        }
    }
}