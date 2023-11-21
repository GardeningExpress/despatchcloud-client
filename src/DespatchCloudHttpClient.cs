using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GardeningExpress.DespatchCloudClient.DTO.Filter;
using GardeningExpress.DespatchCloudClient.DTO.Request;
using GardeningExpress.DespatchCloudClient.DTO.Response;
using Newtonsoft.Json;

namespace GardeningExpress.DespatchCloudClient
{
    public class DespatchCloudHttpClient : IDespatchCloudHttpClient
    {
        private readonly HttpClient _httpClient;

        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public DespatchCloudHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;

            _jsonSerializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
        }

        [Obsolete]
        public async Task<HttpResponseMessage> PostAsJsonAsync<T>(string requestUri, T value, CancellationToken cancellationToken)
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(value),
                Encoding.UTF8,
                "application/json"
            );

            return await _httpClient.PostAsync(requestUri, content, cancellationToken);
        }

        [Obsolete]
        public async Task<HttpResponseMessage> GetAsync(string requestUri, CancellationToken cancellationToken)
            => await _httpClient.GetAsync(requestUri, cancellationToken);

        public async Task<ApiResponse<OrderData>> CreateOrderAsync(OrderCreateRequest orderCreateRequest, CancellationToken cancellationToken = default)
        {
            var httpContent = SerializeObjectToHttpContent(orderCreateRequest);

            var response = await _httpClient
                .PostAsync($"orders/create", httpContent, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var deserializeResponse = await DeserializeResponse<OrderData>(response);
                return new ApiResponse<OrderData>(deserializeResponse);
            }

            return await CreateErrorApiResponse<OrderData>(response);
        }

        public async Task<ListResponse<OrderData>> SearchOrdersAsync(OrderSearchFilters orderSearchFilters, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(orderSearchFilters.Search))
            {
                throw new Exception("Search filter is required.");
            }

            var response = await _httpClient
                .GetAsync($"orders?{orderSearchFilters.GetQueryString()}", cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return await CreateErrorListResponse<OrderData>(response);
            }

            var pagedResult = await DeserializeResponse<PagedResult<OrderData>>(response);
            return new ListResponse<OrderData>(pagedResult);
        }

        public async Task<ApiResponse<OrderData>> GetOrderByChannelOrderIdAsync(string orderId, CancellationToken cancellationToken = default)
        {
            var searchFilter = new OrderSearchFilters
            {
                Search = orderId
            };

            var response = await SearchOrdersAsync(searchFilter, cancellationToken);

            if (!response.IsSuccess)
                return CreateErrorApiResponse<OrderData>(response.Error);

            if (response.PagedData.Total > 1)
            {
                // todo: throw error?
            }

            var orderData = response.PagedData.Data.FirstOrDefault();

            return new ApiResponse<OrderData>(orderData);
        }

        public async Task<ListResponse<Inventory>> SearchInventoryAsync(InventorySearchFilters inventorySearchFilters, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient
                .GetAsync($"inventory?{inventorySearchFilters.GetQueryString()}", cancellationToken);

            if (!response.IsSuccessStatusCode)
                return await CreateErrorListResponse<Inventory>(response);

            var pagedResult = await DeserializeResponse<PagedResult<Inventory>>(response);
            return new ListResponse<Inventory>(pagedResult);
        }

        public async Task<ApiResponse<Inventory>> GetInventoryBySKUAsync(string sku, CancellationToken cancellationToken = default)
        {
            var searchFilters = new InventorySearchFilters
            {
                SKU = sku
            };

            var response = await SearchInventoryAsync(searchFilters, cancellationToken);

            return response.IsSuccess
                ? new ApiResponse<Inventory>(response.PagedData.Data?.FirstOrDefault())
                : CreateErrorApiResponse<Inventory>(response.Error);
        }

        public async Task<ApiResponse<Inventory>> UpdateInventoryAsync(string inventoryId, InventoryUpdateRequest inventoryUpdateRequest, CancellationToken cancellationToken = default)
        {
            var httpContent = SerializeObjectToHttpContent(inventoryUpdateRequest);

            var response = await _httpClient
                .PostAsync($"inventory/{inventoryId}/update", httpContent, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var deserializeResponse = await DeserializeResponse<Inventory>(response);
                return new ApiResponse<Inventory>(deserializeResponse);
            }

            return await CreateErrorApiResponse<Inventory>(response);
        }

        private async Task<ListResponse<T>> CreateErrorListResponse<T>(HttpResponseMessage response)
        {
            var errorResponse = await DeserializeResponse<DespatchCloudErrorResponse>(response);

            var errorMessage = errorResponse != null 
                ? errorResponse.Error 
                : $"Response from DespatchCloud: {response.StatusCode}";
            
            return CreateErrorListResponse<T>(errorMessage);
        }

        private async Task<ApiResponse<T>> CreateErrorApiResponse<T>(HttpResponseMessage response)
        {
            var errorResponse = await DeserializeResponse<DespatchCloudErrorResponse>(response);
            return CreateErrorApiResponse<T>(errorResponse.Error);
        }

        private static ApiResponse<T> CreateErrorApiResponse<T>(string errorMessage)
            => new ApiResponse<T>(errorMessage);

        private static ListResponse<T> CreateErrorListResponse<T>(string errorMessage)
            => new ListResponse<T>(errorMessage);

        private async Task<T> DeserializeResponse<T>(HttpResponseMessage response)
            => JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync(), _jsonSerializerSettings);

        private HttpContent SerializeObjectToHttpContent(object obj)
        {
            var json = JsonConvert.SerializeObject(obj, _jsonSerializerSettings);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}