namespace StocksPortfolioService.Infrastructure.Database.Configuration;

public class MongoDbConfiguration
{
    public required string ConnectionString { get; init; }
    public required string DatabaseName { get; init; }
    public required string PortfolioCollectionName { get; init; }
}
