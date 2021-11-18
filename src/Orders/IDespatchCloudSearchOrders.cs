using GardeningExpress.DespatchCloudClient.Orders.DTO;
using System.Threading.Tasks;

namespace GardeningExpress.DespatchCloudClient.Orders
{
    public interface IDespatchCloudSearchOrders
    {
        Task<DespatchCloudOrderDto> SearchOrdersAsync(DespatchCloudOrderSearchFilters orderSearchFilters);
    }
}