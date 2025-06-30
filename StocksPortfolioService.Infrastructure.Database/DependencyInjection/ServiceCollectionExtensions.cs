using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using StocksPortfolioService.Domain.Repositories;
using StocksPortfolioService.Infrastructure.Database.Configuration;
using StocksPortfolioService.Infrastructure.Database.Repositories;

namespace StocksPortfolioService.Infrastructure.Database.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddMongoDb(configuration)
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
            .Configure<MongoDbConfiguration>(configuration.GetSection(nameof(MongoDbConfiguration)));
        return services;
    }
}
