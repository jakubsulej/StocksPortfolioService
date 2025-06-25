namespace StocksPortfolioService.Infrastructure.Configuration;

public class MongoDbConfiguration
{
    public string ConnectionString { get; init; }
    public string DatabaseName { get; init; }
    public string PortfolioCollectionName { get; init; }
}
