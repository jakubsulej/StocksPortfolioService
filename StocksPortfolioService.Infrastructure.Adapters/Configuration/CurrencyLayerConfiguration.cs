namespace StocksPortfolioService.Infrastructure.Adapters.Configuration;

public class CurrencyLayerConfiguration
{
    public required string ApiAccessKey { get; init; }
    public required string BaseAddressUrl { get; init; }
    public required int RequestRetryCount { get; init; } = 2;
    public required int SleepBetweenRetriesInMs { get; init; } = 1000;
}
