using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Polly;
using Polly.Extensions.Http;
using Refit;
using StocksPortfolioService.Infrastructure.Adapters.CurrencyLayer;
using StocksPortfolioService.Infrastructure.Configuration;
using StocksPortfolioService.Infrastructure.Repositories;
using System;

namespace StocksPortfolioService.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddMongoDb(configuration)
            .AddCurrencyLayerClient(configuration)
            .AddConfiguration(configuration);
    }

    private static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddSingleton<IMongoClient>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<MongoDbConfiguration>>().Value;
                return new MongoClient(settings.ConnectionString);
            })
            .AddRepositories();

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services
            .AddScoped<IPortfolioRepository, PortfolioRepository>();

        return services;
    }

    private static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .Configure<CurrencyLayerConfiguration>(configuration.GetSection(nameof(CurrencyLayerConfiguration)))
            .Configure<MongoDbConfiguration>(configuration.GetSection(nameof(MongoDbConfiguration)));
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
