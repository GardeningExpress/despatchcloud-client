using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GardeningExpress.DespatchCloudClient.DTO;

namespace GardeningExpress.DespatchCloudClient
{
    public interface IDespatchCloudHttpClient
    {
        Task<ListResponse<OrderData>> SearchOrdersAsync(OrderSearchFilters orderSearchFilters, CancellationToken cancellationToken = default);

        Task<ListResponse<Inventory>> SearchInventoryAsync(InventorySearchFilters inventorySearchFilters, CancellationToken cancellationToken = default);

        Task<Inventory> GetInventoryBySKUAsync(string sku, CancellationToken cancellationToken = default);
        
        [Obsolete]
        Task<HttpResponseMessage> PostAsJsonAsync<T>(string requestUri, T value, CancellationToken cancellationToken);

        [Obsolete]
        Task<HttpResponseMessage> GetAsync(string requestUri, CancellationToken cancellationToken);
    }
}