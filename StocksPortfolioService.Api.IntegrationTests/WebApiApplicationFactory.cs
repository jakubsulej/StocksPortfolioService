using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace StocksPortfolioService.Api.IntegrationTests;

internal class WebApiApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _mongoDbConnectionString;
    private readonly string _mongoDbName;

    public WebApiApplicationFactory(
        string mongoDbConnectionString,
        string mongoDbName)
    {
        _mongoDbConnectionString = mongoDbConnectionString;
        _mongoDbName = mongoDbName;
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var initialData = new Dictionary<string, string>
        {
            ["DOTNET_ENVIRONMENT"] = "Development",
            ["CurrencyLayerConfiguration:ApiAccessKey"] = "edcbcd5977de259ca7fb25077ca8a0f6",
            ["CurrencyLayerConfiguration:BaseAddressUrl"] = "http://api.currencylayer.com/",
            ["CurrencyLayerConfiguration:RequestRetryCount"] = "2",
            ["CurrencyLayerConfiguration:SleepBetweenRetriesInMs"] = "1000",      
            ["MongoDbConfiguration:ConnectionString"] = _mongoDbConnectionString,
            ["MongoDbConfiguration:DatabaseName"] = _mongoDbName,
            ["MongoDbConfiguration:PortfolioCollectionName"] = "Portfolios"
        };

        builder
            .ConfigureAppConfiguration((_, configBuilder) =>
            {
                configBuilder.AddInMemoryCollection(initialData!);
            });

        return base.CreateHost(builder);
    }
}
