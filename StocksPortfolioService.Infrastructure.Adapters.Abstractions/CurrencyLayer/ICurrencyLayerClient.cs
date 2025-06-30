using StocksPortfolioService.Infrastructure.Adapters.CurrencyLayer.Models;

namespace StocksPortfolioService.Infrastructure.Adapters.Abstractions.CurrencyLayer;

public interface ICurrencyLayerClient
{
    Task<GetLiveResponse> GetLiveAsync(CancellationToken cancellationToken);
}
