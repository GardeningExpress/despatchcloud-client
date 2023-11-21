using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GardeningExpress.DespatchCloudClient.DTO.Filter;
using GardeningExpress.DespatchCloudClient.DTO.Request;
using GardeningExpress.DespatchCloudClient.DTO.Response;

namespace GardeningExpress.DespatchCloudClient
{
    public interface IDespatchCloudHttpClient
    {
        Task<ApiResponse<OrderData>> CreateOrderAsync(OrderCreateRequest orderCreateRequest, CancellationToken cancellationToken = default);

        Task<ListResponse<OrderData>> SearchOrdersAsync(OrderSearchFilters orderSearchFilters, CancellationToken cancellationToken = default);

        Task<ApiResponse<OrderData>> GetOrderByChannelOrderIdAsync(string orderId, CancellationToken cancellationToken = default);
        
        Task<ListResponse<Inventory>> SearchInventoryAsync(InventorySearchFilters inventorySearchFilters, CancellationToken cancellationToken = default);

        Task<ApiResponse<Inventory>> GetInventoryBySKUAsync(string sku, CancellationToken cancellationToken = default);

        Task<ApiResponse<Inventory>> UpdateInventoryAsync(string inventoryId, InventoryUpdateRequest inventoryUpdateRequest, CancellationToken cancellationToken = default);

        [Obsolete]
        Task<HttpResponseMessage> PostAsJsonAsync<T>(string requestUri, T value, CancellationToken cancellationToken);

        [Obsolete]
        Task<HttpResponseMessage> GetAsync(string requestUri, CancellationToken cancellationToken);
    }
}