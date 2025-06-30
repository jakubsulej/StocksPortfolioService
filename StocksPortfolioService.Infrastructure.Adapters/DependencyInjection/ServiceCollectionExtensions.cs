using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using Refit;
using StocksPortfolioService.Infrastructure.Adapters.Abstractions.CurrencyLayer;
using StocksPortfolioService.Infrastructure.Adapters.Configuration;
using StocksPortfolioService.Infrastructure.Adapters.CurrencyLayer;

namespace StocksPortfolioService.Infrastructure.Adapters.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddCurrencyLayerClient(configuration)
            .AddConfiguration(configuration);
    }

    private static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .Configure<CurrencyLayerConfiguration>(configuration.GetSection(nameof(CurrencyLayerConfiguration)));
        return services;
    }

    private static IServiceCollection AddCurrencyLayerClient(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddRefitClient<ICurrencyLayerApi>()
            .ConfigureHttpClient((sp, client) =>
            {
                var config = sp.GetRequiredService<IOptions<CurrencyLayerConfiguration>>().Value;
                client.BaseAddress = new Uri(config.BaseAddressUrl);
            })
            .AddPolicyHandler((sp, _) =>
            {
                var config = sp.GetRequiredService<IOptions<CurrencyLayerConfiguration>>().Value;
                return HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .WaitAndRetryAsync(
                        retryCount: config.RequestRetryCount,
                        sleepDurationProvider: attempt =>
                            TimeSpan.FromSeconds(Math.Pow(config.SleepBetweenRetriesInMs, attempt))
                    );
            });

        services
            .AddScoped<ICurrencyLayerClient, CurrencyLayerClient>();

        return services;
    }
}
