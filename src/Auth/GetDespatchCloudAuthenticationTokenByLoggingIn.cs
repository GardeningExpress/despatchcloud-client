using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace GardeningExpress.DespatchCloudClient.Auth
{
    public class GetDespatchCloudAuthenticationTokenByLoggingIn : IGetDespatchCloudAuthenticationToken
    {
        private readonly HttpClient _httpClient;
        private readonly IOptionsMonitor<DespatchCloudConfig> _despatchCloudConfig;
        private readonly ILogger<GetDespatchCloudAuthenticationTokenByLoggingIn> _logger;
        private readonly IMemoryCache _memoryCache;

        private static SemaphoreSlim semaphore = new SemaphoreSlim(1,1);

        MemoryCacheEntryOptions cacheEntryOptions;

        private readonly string keyForCacheEntry = "token";

        public GetDespatchCloudAuthenticationTokenByLoggingIn(
            HttpClient httpClient,
            IOptionsMonitor<DespatchCloudConfig> despatchCloudConfig,
            ILogger<GetDespatchCloudAuthenticationTokenByLoggingIn> logger,
            IMemoryCache memoryCache
        )
        {
            _httpClient = httpClient;
            _despatchCloudConfig = despatchCloudConfig;
            _logger = logger;
            _memoryCache = memoryCache;
            _httpClient.BaseAddress = new Uri(_despatchCloudConfig.CurrentValue.ApiBaseUrl);
            cacheEntryOptions = new()
            {
                // Defaults to 10 minute expiry if no config setting
                AbsoluteExpirationRelativeToNow =
                TimeSpan.FromMilliseconds(60 * 1000 * _despatchCloudConfig.CurrentValue.TokenCacheExpiryMinutes) 
            };
        }

        public async Task<string> GetTokenAsync()
        {
            HttpResponseMessage responseMessage;

            // protect race conditions on memorycache if multiple threads triggered simulataneously
            semaphore.Wait();
            try
            {
                var item = _memoryCache.Get(keyForCacheEntry);
                if (item != null) {
                    _logger.LogDebug("GetTokenAsync() returning cached token");
                    return item.ToString();
                }

                var loginRequest = new
                {
                    email = _despatchCloudConfig.CurrentValue.LoginEmailAddress,
                    password = _despatchCloudConfig.CurrentValue.LoginPassword,
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
                    // cache it
                    _memoryCache.Set(keyForCacheEntry, loginResponse.Token, cacheEntryOptions);
                    return loginResponse.Token;
                }
            }
            catch (Exception ex)
            {
                throw new ApiAuthenticationException(ex);
            }
            finally
            {
                semaphore.Release();
            }

            if (responseMessage.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new ApiAuthenticationException("Could not get DespatchCloud Token - Invalid Credentials");
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