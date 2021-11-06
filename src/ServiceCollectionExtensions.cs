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

            services.AddTransient<AddAuthTokenHandler>();

            services.AddHttpClient<IGetDespatchCloudAuthenticationToken, GetDespatchCloudAuthenticationTokenByLoggingIn>((serviceProvider, client) =>
            {
                var options = serviceProvider.GetRequiredService<IOptionsMonitor<DespatchCloudConfig>>();
                client.BaseAddress = new Uri(options.CurrentValue.ApiBaseUrl);
            });

            services.AddHttpClient<IDespatchCloudHttpClient, DespatchCloudHttpClient>((serviceProvider, client) =>
                {
                    var options = serviceProvider.GetRequiredService<IOptionsMonitor<DespatchCloudConfig>>();
                    client.BaseAddress = new Uri(options.CurrentValue.ApiBaseUrl);
                })
                .AddHttpMessageHandler<AddAuthTokenHandler>();
        }
    }
}