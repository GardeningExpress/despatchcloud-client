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

        public async Task<PagedResult<OrderData>> SearchOrdersAsync(OrderSearchFilters orderSearchFilters)
        {
            if (string.IsNullOrWhiteSpace(orderSearchFilters.Search))
            {
                throw new Exception("Search filter is required.");
            }

            var response = await _httpClient.GetAsync($"orders?{orderSearchFilters.GetQueryString()}");

            if (response.IsSuccessStatusCode)
            {
                return await SerializeResponse<PagedResult<OrderData>>(response);
            }
            else
            {
                // todo: let's work out an error handling strategy later
                throw new Exception("Failed to get data");
            }
        }

        public async Task<PagedResult<Inventory>> SearchInventoryAsync(InventorySearchFilters inventorySearchFilters)
        {
            var response = await _httpClient.GetAsync($"inventory?{inventorySearchFilters.GetQueryString()}");

            if (response.IsSuccessStatusCode)
            {
                return await SerializeResponse<PagedResult<Inventory>>(response);
            }
            else
            {
                // todo: let's work out an error handling strategy later
                throw new Exception("Failed to get data");
            }
        }

        private async Task<T> SerializeResponse<T>(HttpResponseMessage response)
        {
            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync(), _jsonSerializerSettings);
        }
    }
}