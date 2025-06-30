using Microsoft.Extensions.Caching.Memory;
using StocksPortfolioService.Infrastructure.Adapters.Abstractions.CurrencyLayer;

namespace StocksPortfolioService.Application.Services;

public interface ICurrencyLayerService
{
    Task<IDictionary<string, decimal>?> GetCachedCurrencies(CancellationToken cancellationToken);
}

internal class CurrencyLayerService : ICurrencyLayerService
{
    private readonly TimeSpan _cacheExpirationTime;
    private readonly IMemoryCache _memoryCache;
    private readonly ICurrencyLayerClient _currencyLayerClient;

    public CurrencyLayerService(
        IMemoryCache memoryCache,
        ICurrencyLayerClient currencyLayerClient)
    {
        _cacheExpirationTime = TimeSpan.FromHours(24);
        _memoryCache = memoryCache;
        _currencyLayerClient = currencyLayerClient;
    }

    public async Task<IDictionary<string, decimal>?> GetCachedCurrencies(CancellationToken cancellationToken) =>
        await _memoryCache.GetOrCreateAsync(nameof(GetCachedCurrencies), async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = _cacheExpirationTime;
            return (await _currencyLayerClient.GetLiveAsync(cancellationToken))?.Quotes;
        });
}
