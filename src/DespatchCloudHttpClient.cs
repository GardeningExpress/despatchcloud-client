using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GardeningExpress.DespatchCloudClient
{
    public class DespatchCloudHttpClient : IDespatchCloudHttpClient
    {
        private readonly HttpClient _httpClient;

        public DespatchCloudHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> PostAsJsonAsync<T>(string requestUri, T value, CancellationToken cancellationToken)
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(value),
                Encoding.UTF8,
                "application/json"
            );

            return await _httpClient.PostAsync(requestUri, content, cancellationToken);
        }

        public async Task<HttpResponseMessage> GetAsync(string requestUri, CancellationToken cancellationToken)
            => await _httpClient.GetAsync(requestUri, cancellationToken);
    }
}