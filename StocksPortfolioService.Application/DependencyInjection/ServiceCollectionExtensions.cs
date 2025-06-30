using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StocksPortfolioService.Application.Services;
using StocksPortfolioService.Infrastructure.Adapters.DependencyInjection;
using StocksPortfolioService.Infrastructure.Database.DependencyInjection;

namespace StocksPortfolioService.Application.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddDatabaseServices(configuration)
            .AddCQRSServices()
            .AddMemoryCache()
            .AddTransientServices()
            .AddInfrastructureServices(configuration);
    }

    private static IServiceCollection AddCQRSServices(this IServiceCollection services)
    {
        services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

        return services;
    }

    private static IServiceCollection AddTransientServices(this IServiceCollection services)
    {
        services
            .AddTransient<IStockService, StockService>()
            .AddTransient<ICurrencyLayerService, CurrencyLayerService>();

        return services;
    }
}
