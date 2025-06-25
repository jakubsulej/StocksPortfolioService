using System.Collections.Generic;

namespace StocksPortfolioService.Infrastructure.Adapters.CurrencyLayer.Models;

public class GetLiveResponse
{
    public IDictionary<string, decimal> Quotes { get; init; }
}
