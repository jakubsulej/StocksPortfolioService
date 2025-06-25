namespace StocksPortfolioService.Application.Queries.GetTotalPortfolioValue;

public record GetTotalPortfolioValueResponse
{
    public decimal TotalAmount { get; init; }
}
