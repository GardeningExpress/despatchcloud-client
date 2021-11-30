using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GardeningExpress.DespatchCloudClient.DTO;

namespace GardeningExpress.DespatchCloudClient
{
    public interface IDespatchCloudHttpClient
    {
        Task<PagedResult<OrderData>> SearchOrdersAsync(OrderSearchFilters orderSearchFilters);

        Task<PagedResult<Inventory>> SearchInventoryAsync(InventorySearchFilters inventorySearchFilters)

        [Obsolete]
        Task<HttpResponseMessage> PostAsJsonAsync<T>(string requestUri, T value, CancellationToken cancellationToken);

        [Obsolete]
        Task<HttpResponseMessage> GetAsync(string requestUri, CancellationToken cancellationToken);
    }
}