using System.Threading.Tasks;
using NUnit.Framework;
using Shouldly;

namespace GardeningExpress.DespatchCloudClient.Tests.Integration;

public class SetOrderStatusAsyncTests : BaseIntegrationTests
{
    private const string ChannelOrderId = "601220150";

    protected override Task MethodForAuthTest()
    {
        // will return 404, but not a 401
        return DespatchCloudHttpClient
            .SetOrderStatusAsync(1, Constants.OrderStatus.OnHold);
    }

    [Test]
    public async Task Sets_order_status()
    {
        var orderDetails = await DespatchCloudHttpClient.GetOrderByChannelOrderIdAsync(ChannelOrderId);

        if (!orderDetails.IsSuccess)
        {
            Assert.Fail("Could not get order - {orderDetails.Error}");
        }

        var setOrderStatus = await DespatchCloudHttpClient
            .SetOrderStatusAsync(orderDetails.Data.Id, Constants.OrderStatus.OnHold);

        if (setOrderStatus.IsSuccess == false)
        {
            Assert.Fail($"Failed to set order status - {setOrderStatus.Error}");
        }

        var orderAfterSetStatus = await DespatchCloudHttpClient
            .GetOrderByChannelOrderIdAsync(ChannelOrderId);

        if (!orderAfterSetStatus.IsSuccess)
        {
            Assert.Fail("Could not get order after set status - {orderDetails.Error}");
        }

        orderAfterSetStatus.Data.StatusId.ShouldBe(Constants.OrderStatus.OnHold);

        // set status back to what it was
        var setStatusBack = await DespatchCloudHttpClient
            .SetOrderStatusAsync(orderDetails.Data.Id, orderDetails.Data.StatusId);

        setStatusBack.IsSuccess.ShouldBeTrue($"Could not set back to status - {setStatusBack.Error}");
    }
}