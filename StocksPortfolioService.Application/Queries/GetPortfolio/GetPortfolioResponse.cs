using MongoDB.Bson;
using System.Collections.Generic;

namespace StocksPortfolioService.Application.Queries.GetPortfolio;

public record GetPortfolioResponse
{
    public ObjectId Id { get; init; }
    public float CurrentTotalValue { get; init; }
    public IEnumerable<StockData> Stocks { get; init; }

    public class StockData
    {
        public string Ticker { get; init; }
        public string BaseCurrency { get; init; }
        public int NumberOfShares { get; init; }
    }
}
