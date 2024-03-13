using GardeningExpress.DespatchCloudClient.DTO.Request;
using GardeningExpress.DespatchCloudClient.Tests.Integration.Utils;
using NUnit.Framework;
using System.Threading.Tasks;

namespace GardeningExpress.DespatchCloudClient.Tests.Integration
{
    public class CreateOrderAsyncTests : BaseIntegrationTests
    {
        protected override Task MethodForAuthTest() =>
            DespatchCloudHttpClient
                .CreateOrderAsync(new OrderCreateRequest());

        private OrderCreateRequest newOrder;

        [SetUp]
        public void Setup()
        {
            newOrder = TestUtils.GetCreateOrderRequest();
            newOrder.InvoiceName = "Integration Tests: CreateOrderAsync()";
        }
                
        [Test]
        public async Task CreateOrderAsync_ShouldReturnIsSuccessAsTrueAndIdField_WhenSuccessful()
        {
            // ARRANGE

            // ACT
            var result = await DespatchCloudHttpClient.CreateOrderAsync(newOrder);

            // ASSERT
            Assert.IsTrue(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.NotNull(result.Data.Id); // Verify trackable id set
        }

        [Theory]
        [TestCase("PaymentMethod", "payment_method", null)]
        public async Task CreateOrderAsync_ShouldReturnIsSuccessAsFalseAndAnErrorMessage_WhenRequiredFieldMissing(string field, string jsonField, object setValue = null)
        {
            // ARRANGE
            TestUtils.SetPropertyValue(newOrder, field, setValue);

            // ACT
            var result = await DespatchCloudHttpClient.CreateOrderAsync(newOrder);

            // ASSERT
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(result.Error, $"Error executing statement: Column '{jsonField}' cannot be null");
        }

    }
}
