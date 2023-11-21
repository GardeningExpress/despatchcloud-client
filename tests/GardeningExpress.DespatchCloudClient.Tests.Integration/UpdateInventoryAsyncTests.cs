using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GardeningExpress.DespatchCloudClient.DTO.Request;
using GardeningExpress.DespatchCloudClient.DTO.Response;
using NUnit.Framework;
using Shouldly;

namespace GardeningExpress.DespatchCloudClient.Tests.Integration
{
    public class UpdateInventoryAsyncTests : BaseIntegrationTests
    {
        protected override Task MethodForAuthTest()
        {
            throw new NotImplementedException();
        }

        private Inventory _originalInventory;

        [SetUp]
        public async Task SetUp()
        {
            var response = await DespatchCloudHttpClient
                .GetInventoryBySKUAsync("DEAL16071");

            if (!response.IsSuccess)
                throw new Exception("Could not get inventory - " + response.Error);
            if (response.Data == null)
                throw new Exception("Could not get inventory - Not found");

            _originalInventory = response.Data;
        }

        [TearDown]
        public async Task Reset()
        {
            // don't bother
            if (LoginPassword == "fake" || LoginEmailAddress == "fake")
                return;

            var resetRequest = new InventoryUpdateRequest
            {
                Name = _originalInventory.Name,
                CustomFields = _originalInventory.CustomFields
            };

            var resetInventoryResult = await DespatchCloudHttpClient
                .UpdateInventoryAsync(_originalInventory.Id, resetRequest);

            var inventoryAfterReset = await DespatchCloudHttpClient
                .GetInventoryBySKUAsync("DEAL16071");

            inventoryAfterReset.Data.Name.ShouldBe(_originalInventory.Name);

            foreach (var dataCustomField in inventoryAfterReset.Data.CustomFields)
                dataCustomField.Value.ShouldBe(_originalInventory.CustomFields[dataCustomField.Key]);
        }

        [Test]
        public async Task Update_Name()
        {
            var request = new InventoryUpdateRequest
            {
                Name = "New name"
            };

            var updatedInventoryResult = await DespatchCloudHttpClient
                .UpdateInventoryAsync(_originalInventory.Id, request);

            updatedInventoryResult.IsSuccess.ShouldBeTrue();
            updatedInventoryResult.Data.Name.ShouldBe("New name");

            var inventoryAfterUpdate = await DespatchCloudHttpClient
                .GetInventoryBySKUAsync("DEAL16071");

            inventoryAfterUpdate.Data.Name.ShouldBe("New name");
        }

        [Test]
        public async Task Update_PotSize_CustomField_Leaves_Other_CustomFields_Alone()
        {
            var request = new InventoryUpdateRequest
            {
                CustomFields = new Dictionary<string, object>
                {
                    { "pot-size-3", "100 Litre" }
                }
            };

            var updatedInventoryResult = await DespatchCloudHttpClient
                .UpdateInventoryAsync(_originalInventory.Id, request);

            updatedInventoryResult.IsSuccess.ShouldBeTrue();
            updatedInventoryResult.Data.CustomFields["pot-size-3"].ShouldBe("100 Litre");

            var inventoryAfterUpdate = await DespatchCloudHttpClient
                .GetInventoryBySKUAsync("DEAL16071");

            inventoryAfterUpdate.Data.CustomFields["pot-size-3"].ShouldBe("100 Litre");
        }
        
        [Test]
        public async Task Does_Not_Add_CustomField_If_Not_Found()
        {
            var request = new InventoryUpdateRequest
            {
                CustomFields = new Dictionary<string, object>
                {
                    { "new_field", "Whatever" }
                }
            };

            var updatedInventoryResult = await DespatchCloudHttpClient
                .UpdateInventoryAsync(_originalInventory.Id, request);

            updatedInventoryResult.IsSuccess.ShouldBeTrue();
            updatedInventoryResult.Data.CustomFields.Count.ShouldBe(_originalInventory.CustomFields.Count);
            updatedInventoryResult.Data.CustomFields.ShouldNotContainKey("new_field");
        }
    }
}