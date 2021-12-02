using System;
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

            return response.IsSuccessStatusCode
                ? await CreateSuccessResponse<ListResponse<OrderData>>(response)
                : await CreateErrorResponse<ListResponse<OrderData>>(response);
        }

        public async Task<ListResponse<Inventory>> SearchInventoryAsync(InventorySearchFilters inventorySearchFilters, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient
                .GetAsync($"inventory?{inventorySearchFilters.GetQueryString()}", cancellationToken);

            return response.IsSuccessStatusCode
                ? await CreateSuccessResponse<ListResponse<Inventory>>(response)
                : await CreateErrorResponse<ListResponse<Inventory>>(response);
        }

        private async Task<T> CreateSuccessResponse<T>(HttpResponseMessage response)
            where T : ApiResponse
        {
            var apiResponse = await SerializeResponse<T>(response);
            return (T)Activator.CreateInstance(typeof(T), apiResponse);
        }


        private async Task<T> CreateErrorResponse<T>(HttpResponseMessage response)
            where T : ApiResponse
        {
            var despatchCloudErrorResponse = await SerializeResponse<DespatchCloudErrorResponse>(response);
            return (T)Activator.CreateInstance(typeof(T), despatchCloudErrorResponse.Error);
        }

        private async Task<T> SerializeResponse<T>(HttpResponseMessage response)
        {
            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync(), _jsonSerializerSettings);
        }
    }
}