namespace StocksPortfolioService.Infrastructure.Configuration;

public class CurrencyLayerConfiguration
{
    public string ApiAccessKey { get; init; }
    public string BaseAddressUrl { get; init; }
    public int RequestRetryCount { get; init; } = 2;
    public int SleepBetweenRetriesInMs { get; init; } = 1000;
}
