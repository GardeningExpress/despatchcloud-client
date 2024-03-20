using System;
using System.Threading.Tasks;
using GardeningExpress.DespatchCloudClient.Tests.Integration.Utils;
using NUnit.Framework;
using Shouldly;

namespace GardeningExpress.DespatchCloudClient.Tests.Integration
{
    public class CancelOrderAsyncTests : BaseIntegrationTests
    {
        private const string ChannelOrderId = "601220151";
            private const string AlreadyDespatchedChannelOrderId = "gogroopie-test-638464556449172005";

        protected override Task MethodForAuthTest()
        {
            // this should return a 404, but not a 401, so test should pass
            return DespatchCloudHttpClient
                .CancelOrderAsync(1);
        }

        [Test]
        public async Task When_order_already_despatched_returns_error_message()
        {
            var orderDetails = await DespatchCloudHttpClient.GetOrderByChannelOrderIdAsync(AlreadyDespatchedChannelOrderId);

            if (!orderDetails.IsSuccess)
            {
                Assert.Fail("Could not get order - {orderDetails.Error}");
            }

            if (orderDetails.Data.StatusId != Constants.OrderStatus.Despatched)
            {
                Assert.Fail("Order was not already despatched");
            }

            var cancelOrderResponse = await DespatchCloudHttpClient
                .CancelOrderAsync(orderDetails.Data.Id);

            if (cancelOrderResponse.IsSuccess == true)
                Assert.Fail($"Order was cancelled when it should not have been");

            cancelOrderResponse.Error.ShouldBe("Order in Despatched status.");
        }

        [Test]
        public async Task Cancels_order()
        {
            var orderReq = TestUtils.GetCreateOrderRequest();
            orderReq.ChannelOrderId = $"TestSetId-{DateTime.Now.Ticks}";

            var orderDetails = await DespatchCloudHttpClient.CreateOrderAsync(orderReq);

            if (!orderDetails.IsSuccess)
            {
                Assert.Fail("Could not get order - {orderDetails.Error}");
            }

            if (orderDetails.Data.StatusId == Constants.OrderStatus.Cancelled)
            {
                Assert.Fail("Order was already cancelled");
            }

            var cancelOrderResponse = await DespatchCloudHttpClient
                .CancelOrderAsync(orderDetails.Data.Id);

            if (cancelOrderResponse.IsSuccess == false)
            {
                Assert.Fail($"Failed to cancel order - {cancelOrderResponse.Error}");
            }

            var orderAfterCancellationDetails = await DespatchCloudHttpClient.GetOrderByChannelOrderIdAsync(ChannelOrderId);

            if (!orderAfterCancellationDetails.IsSuccess)
            {
                Assert.Fail("Could not get order after cancellation - {orderDetails.Error}");
            }

            orderAfterCancellationDetails.Data.StatusId.ShouldBe(Constants.OrderStatus.Cancelled);
        }
    }
}