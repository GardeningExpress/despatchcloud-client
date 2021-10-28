using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace GardeningExpress.DespatchCloudClient
{
    public class AddAuthTokenHandler : DelegatingHandler
    {
        private readonly IGetDespatchCloudAuthenticationToken _getDespatchCloudAuthenticationToken;

        public AddAuthTokenHandler(IGetDespatchCloudAuthenticationToken getDespatchCloudAuthenticationToken)
        {
            _getDespatchCloudAuthenticationToken = getDespatchCloudAuthenticationToken;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken
        )
        {
            if (request.RequestUri.AbsolutePath.EndsWith("auth/login"))
            {
                return await base.SendAsync(request, cancellationToken);
            }

            if (request.Headers.Authorization == null)
            {
                var token = await _getDespatchCloudAuthenticationToken.GetTokenAsync();
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
