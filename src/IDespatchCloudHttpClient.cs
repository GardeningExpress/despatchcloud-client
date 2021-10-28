using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GardeningExpress.DespatchCloudClient
{
    public interface IDespatchCloudHttpClient
    {
        Task<HttpResponseMessage> PostAsJsonAsync<T>(string requestUri, T value, CancellationToken cancellationToken);
        Task<HttpResponseMessage> GetAsync(string requestUri, CancellationToken cancellationToken);
    }
}