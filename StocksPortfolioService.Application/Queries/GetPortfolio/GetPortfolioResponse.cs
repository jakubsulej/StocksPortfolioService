namespace StocksPortfolioService.Application.Queries.GetPortfolio;

public record GetPortfolioResponse
{
    public required string Id { get; init; }
    public required float CurrentTotalValue { get; init; }
    public required IEnumerable<StockData> Stocks { get; init; }

    public record StockData
    {
        public required string Ticker { get; init; }
        public required string BaseCurrency { get; init; }
        public required int NumberOfShares { get; init; }
    }
}
