using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StocksPortfolioService.Application.DependencyInjection;

namespace StocksPortfolioService.Api.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWebApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddApplicationServices(configuration);
    }
}
