using System;
using GardeningExpress.DespatchCloudClient.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace GardeningExpress.DespatchCloudClient
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDespatchCloudClient(this IServiceCollection services)
        {
            services.AddOptions<DespatchCloudConfig>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.GetSection("DespatchCloud")
                        .Bind(settings);
                });

            services.AddTransient<IGetDespatchCloudAuthenticationToken, GetDespatchCloudAuthenticationTokenByLoggingIn>();
            services.AddTransient<AddAuthTokenHandler>();

            services.AddHttpClient<IDespatchCloudHttpClient, DespatchCloudHttpClient>((serviceProvider, client) =>
                {
                    var options = serviceProvider.GetService<IOptions<DespatchCloudConfig>>();

                    if (options == null || string.IsNullOrEmpty(options.Value.ApiBaseUrl))
                    {
                        throw new Exception("DespatchCloud API Base URL not set");
                    }

                    client.BaseAddress = new Uri(options.Value.ApiBaseUrl);
                })
                .AddHttpMessageHandler<AddAuthTokenHandler>();
        }
    }
}