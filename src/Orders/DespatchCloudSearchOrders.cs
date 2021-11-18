using GardeningExpress.DespatchCloudClient.Auth;
using GardeningExpress.DespatchCloudClient.Orders.DTO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace GardeningExpress.DespatchCloudClient.Orders
{
    public class DespatchCloudSearchOrders : IDespatchCloudSearchOrders
    {
        private HttpClient _httpClient;
        private IOptionsMonitor<DespatchCloudConfig> _despatchCloudConfig;
        private ILogger<DespatchCloudSearchOrders> _logger;
        private readonly IGetDespatchCloudAuthenticationToken _getDespatchCloudAuthenticationToken;

        public DespatchCloudSearchOrders(HttpClient httpClient, IOptionsMonitor<DespatchCloudConfig> despatchCloudConfig, ILogger<DespatchCloudSearchOrders> logger, IGetDespatchCloudAuthenticationToken getDespatchCloudAuthenticationToken)
        {
            _httpClient = httpClient;
            _despatchCloudConfig = despatchCloudConfig;
            _logger = logger;
            _getDespatchCloudAuthenticationToken = getDespatchCloudAuthenticationToken;
            _httpClient.BaseAddress = new Uri(_despatchCloudConfig.CurrentValue.ApiBaseUrl);
        }

        public async Task<DespatchCloudOrderDto> SearchOrdersAsync(DespatchCloudOrderSearchFilters orderSearchFilters)
        {
            if (string.IsNullOrWhiteSpace(orderSearchFilters.Search))
            {
                throw new Exception("Search filter is required.");
            }

            // get token
            var token = await _getDespatchCloudAuthenticationToken.GetTokenAsync();

            //get data
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"orders?{orderSearchFilters.GetQueryString()}");

            if (response.IsSuccessStatusCode)
            {
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                return JsonConvert.DeserializeObject<DespatchCloudOrderDto>(await response.Content.ReadAsStringAsync(), settings);
            }
            else
            {
                throw new Exception("Failed to get data");
            }
        }
    }
}
