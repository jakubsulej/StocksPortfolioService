using System.Collections.Generic;

namespace StocksPortfolioService.Infrastructure.Adapters.CurrencyLayer.Models;

public class Quote
{
    public bool Success { get; init; }
    public string Terms { get; init; }
    public string Privacy { get; init; }
    public int Timestamp { get; init; }
    public string Source { get; init; }
    public Dictionary<string, decimal> Quotes { get; init; }
}
