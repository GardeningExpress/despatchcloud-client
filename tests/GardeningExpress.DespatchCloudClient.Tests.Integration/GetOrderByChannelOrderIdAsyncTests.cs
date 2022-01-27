using System.Threading.Tasks;
using NUnit.Framework;
using Shouldly;

namespace GardeningExpress.DespatchCloudClient.Tests.Integration
{
    public class GetOrderByChannelOrderIdAsyncTests : BaseIntegrationTests
    {
        private const string ChannelOrderId = "600951054";
        
        protected override Task MethodForAuthTest()
        {
            return DespatchCloudHttpClient
                .GetOrderByChannelOrderIdAsync(ChannelOrderId);
        }

        [Test]
        public async Task Returns_Null_When_Order_Not_Found()
        {
            var result = await DespatchCloudHttpClient
                .GetOrderByChannelOrderIdAsync("not_found");

            result.Error.ShouldBeNull();
            result.Data.ShouldBeNull();
        }
        
        [Test]
        public async Task Returns_Order()
        {
            var result = await DespatchCloudHttpClient
                .GetOrderByChannelOrderIdAsync(ChannelOrderId);

            result.Error.ShouldBeNull();
            result.Data.ChannelOrderId.ShouldBe(ChannelOrderId);
        }
    }
}