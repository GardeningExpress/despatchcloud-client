using System;
using GardeningExpress.DespatchCloudClient.DTO.Request;
using GardeningExpress.DespatchCloudClient.Tests.Integration.Utils;
using NUnit.Framework;
using System.Threading.Tasks;
using Shouldly;

namespace GardeningExpress.DespatchCloudClient.Tests.Integration
{
    public class CreateOrderAsyncTests : BaseIntegrationTests
    {
        protected override Task MethodForAuthTest() =>
            DespatchCloudHttpClient
                .CreateOrderAsync(new OrderCreateRequest());

        private OrderCreateRequest _newOrder;

        [SetUp]
        public void Setup()
        {
            _newOrder = TestUtils.GetCreateOrderRequest();
            _newOrder.InvoiceName = "Integration Tests: CreateOrderAsync()";
        }

        [Test]
        public async Task CreateOrderAsync_ShouldReturnIsSuccessAsTrueAndIdField_WhenSuccessful()
        {
            // ARRANGE

            // ACT
            var result = await DespatchCloudHttpClient.CreateOrderAsync(_newOrder);

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
            TestUtils.SetPropertyValue(_newOrder, field, setValue);

            // ACT
            var result = await DespatchCloudHttpClient.CreateOrderAsync(_newOrder);

            // ASSERT
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(result.Error, $"Error executing statement: Column '{jsonField}' cannot be null");
        }

        [Test]
        public async Task CreateOrderAsync_CanSetChannelId()
        {
            // ARRANGE
            _newOrder.ChannelOrderId = $"TestSetId-{DateTime.Now.Ticks}";

            // ACT
            var result = await DespatchCloudHttpClient.CreateOrderAsync(_newOrder);

            // ASSERT
            Assert.IsTrue(result.IsSuccess);

            Assert.AreEqual(_newOrder.ChannelOrderId, result.Data.ChannelOrderId);
        }

        [Test]
        public async Task CreateOrderAsync_DuplicateOrder()
        {
            // ACT
            var channelOrderId = $"Test-{DateTime.Now.Ticks}";

            var order = _newOrder with { ChannelOrderId = channelOrderId };

            var result1 = await DespatchCloudHttpClient.CreateOrderAsync(order);
            var result2 = await DespatchCloudHttpClient.CreateOrderAsync(order);

            // ASSERT
            Assert.IsTrue(result1.IsSuccess);
            Assert.IsFalse(result2.IsSuccess);

            result2.Error.ShouldBe($"The Channel Order ID ({channelOrderId}) is already in use.");
        }

        
        [Test]
        public async Task CreateOrderAsync_WithInvalidManualChannelId_ShouldReturnAHttp422ErrorMessage()
        {
            // 422 response occurs when manualchannelid is wrong
            // ARRANGE
            var channelOrderId = $"Test-{DateTime.Now.Ticks}";
            var order = _newOrder with { ChannelOrderId = channelOrderId };
            order.ManualChannelId = 1234;

            // ACT
            var result1 = await DespatchCloudHttpClient.CreateOrderAsync(order);

            // ASSERT
            Assert.IsFalse(result1.IsSuccess);
            Assert.IsNotNull(result1.Error);
            Assert.AreEqual("Response from DespatchCloud: 422 - Unprocessable Entity", result1.Error);
        }
    }
}