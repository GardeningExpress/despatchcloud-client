using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GardeningExpress.DespatchCloudClient.DTO;
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

        public async Task<ListResponse<OrderData>> SearchOrdersAsync(OrderSearchFilters orderSearchFilters, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(orderSearchFilters.Search))
            {
                throw new Exception("Search filter is required.");
            }

            var response = await _httpClient
                .GetAsync($"orders?{orderSearchFilters.GetQueryString()}", cancellationToken);

            if (!response.IsSuccessStatusCode)
                return await CreateErrorResponse<ListResponse<OrderData>>(response);

            var pagedResult = await DeserializeResponse<PagedResult<OrderData>>(response);
            return new ListResponse<OrderData>(pagedResult);
        }

        public async Task<ListResponse<Inventory>> SearchInventoryAsync(InventorySearchFilters inventorySearchFilters, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient
                .GetAsync($"inventory?{inventorySearchFilters.GetQueryString()}", cancellationToken);

            if (!response.IsSuccessStatusCode)
                return await CreateErrorResponse<ListResponse<Inventory>>(response);

            var pagedResult = await DeserializeResponse<PagedResult<Inventory>>(response);
            return new ListResponse<Inventory>(pagedResult);
        }

        public async Task<Inventory> GetInventoryBySKUAsync(string sku, CancellationToken cancellationToken = default)
        {
            var searchFilters = new InventorySearchFilters
            {
                SKU = sku
            };

            var response = await SearchInventoryAsync(searchFilters, cancellationToken);

            return response.IsSuccess
                ? response.PagedResult.Data?.FirstOrDefault()
                : CreateErrorResponse<Inventory>(response.Error);
        }


        private async Task<T> CreateErrorResponse<T>(HttpResponseMessage response)
            where T : ApiResponse
        {
            var despatchCloudErrorResponse = await DeserializeResponse<DespatchCloudErrorResponse>(response);
            return CreateErrorResponse<T>(despatchCloudErrorResponse.Error);
        }

        private static T CreateErrorResponse<T>(string errorMessage) => (T)Activator.CreateInstance(typeof(T), errorMessage);

        private async Task<T> DeserializeResponse<T>(HttpResponseMessage response)
        {
            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync(), _jsonSerializerSettings);
        }
    }
}