using System;
using System.Net.Http;
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
                    var configurationSection = configuration
                        .GetSection("DespatchCloud");

                    var environmentValue = configurationSection
                        .GetValue<string>("Environment");

                    if (string.IsNullOrEmpty(environmentValue))
                        throw new Exception("DespatchCloud environment not set in config");

                    configurationSection.Bind(settings);
                });

            services.AddTransient<AddAuthTokenHandler>();

            services.AddHttpClient<IGetDespatchCloudAuthenticationToken, GetDespatchCloudAuthenticationTokenByLoggingIn>((serviceProvider, client) =>
            {
                var options = serviceProvider.GetRequiredService<IOptionsMonitor<DespatchCloudConfig>>();
                client.BaseAddress = new Uri(options.CurrentValue.ApiBaseUrl);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.Timeout = TimeSpan.FromSeconds(30);
            });

            services.AddHttpClient<IDespatchCloudHttpClient, DespatchCloudHttpClient>((serviceProvider, client) =>
            {
                var options = serviceProvider.GetRequiredService<IOptionsMonitor<DespatchCloudConfig>>();
                client.BaseAddress = new Uri(options.CurrentValue.ApiBaseUrl);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.Timeout = TimeSpan.FromSeconds(30);
            }).AddHttpMessageHandler<AddAuthTokenHandler>();
        }
    }
}