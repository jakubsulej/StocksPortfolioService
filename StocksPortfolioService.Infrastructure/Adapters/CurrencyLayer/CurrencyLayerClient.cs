using Microsoft.Extensions.Options;
using StocksPortfolioService.Infrastructure.Adapters.CurrencyLayer.Models;
using StocksPortfolioService.Infrastructure.Configuration;
using System.Threading;
using System.Threading.Tasks;

namespace StocksPortfolioService.Infrastructure.Adapters.CurrencyLayer;

public interface ICurrencyLayerClient
{
    Task<GetLiveResponse> GetLiveAsync(CancellationToken cancellationToken);
}

internal class CurrencyLayerClient : ICurrencyLayerClient
{
    private readonly ICurrencyLayerApi _api;
    private readonly CurrencyLayerConfiguration _config;

    public CurrencyLayerClient(ICurrencyLayerApi api, IOptions<CurrencyLayerConfiguration> config)
    {
        _api = api;
        _config = config.Value;
    }

    public async Task<GetLiveResponse> GetLiveAsync(CancellationToken cancellationToken)
    {
        var response = await _api.GetLiveAsync(_config.ApiAccessKey, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            return new GetLiveResponse
            {
                Quotes = response.Content.Quotes
            };
        }

        return null;
    }
}
