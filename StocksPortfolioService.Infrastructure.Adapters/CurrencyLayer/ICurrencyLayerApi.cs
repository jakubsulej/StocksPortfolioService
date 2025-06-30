using Refit;
using StocksPortfolioService.Infrastructure.Adapters.CurrencyLayer.Models;

namespace StocksPortfolioService.Infrastructure.Adapters.CurrencyLayer;

public interface ICurrencyLayerApi
{
    [Get("/live")]
    Task<ApiResponse<Quote>> GetLiveAsync([AliasAs("access_key")] string accessKey, CancellationToken cancellationToken);
}
