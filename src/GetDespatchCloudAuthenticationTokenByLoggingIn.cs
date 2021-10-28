using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace GardeningExpress.DespatchCloudClient
{
    public class GetDespatchCloudAuthenticationTokenByLoggingIn : IGetDespatchCloudAuthenticationToken
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<DespatchCloudConfig> _despatchCloudConfig;
        private readonly ILogger<GetDespatchCloudAuthenticationTokenByLoggingIn> _logger;

        public GetDespatchCloudAuthenticationTokenByLoggingIn(
            HttpClient httpClient,
            IOptions<DespatchCloudConfig> despatchCloudConfig,
            ILogger<GetDespatchCloudAuthenticationTokenByLoggingIn> logger
        )
        {
            _httpClient = httpClient;
            _despatchCloudConfig = despatchCloudConfig;
            _logger = logger;

            _httpClient.BaseAddress = new Uri(_despatchCloudConfig.Value.ApiBaseUrl);
        }

        public async Task<string> GetTokenAsync()
        {
            HttpResponseMessage responseMessage;

            try
            {
                var loginRequest = new
                {
                    email = _despatchCloudConfig.Value.LoginEmailAddress,
                    password = _despatchCloudConfig.Value.LoginPassword,
                };

                EnsureRequestValid(loginRequest);

                responseMessage = await _httpClient
                    .PostAsync("auth/login",
                        new StringContent(
                            JsonConvert.SerializeObject(loginRequest),
                            Encoding.UTF8,
                            "application/json"
                        )
                    );

                if (responseMessage.IsSuccessStatusCode)
                {
                    var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(await responseMessage.Content.ReadAsStringAsync());

                    return loginResponse.Token;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Could not get DespatchCloud Token", ex);
            }

            if (responseMessage.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new Exception("Could not get DespatchCloud Token - Invalid Credentials");
            }

            var content = await responseMessage.Content.ReadAsStringAsync();

            _logger.LogError("Error getting DespatchCloud Token. DespatchCloud Status: {Status} {Reason} {Content}",
                responseMessage.StatusCode,
                responseMessage.ReasonPhrase,
                content
            );

            throw new Exception("Could not get DespatchCloud Token");
        }

        private static void EnsureRequestValid(dynamic loginRequest)
        {
            if (string.IsNullOrEmpty(loginRequest.email))
                throw new Exception("DespatchCloud email address not set in config");
            if (string.IsNullOrEmpty(loginRequest.password))
                throw new Exception("DespatchCloud password not set in config");
        }

        private class LoginResponse
        {
            public string Token { get; set; }
        }
    }
}