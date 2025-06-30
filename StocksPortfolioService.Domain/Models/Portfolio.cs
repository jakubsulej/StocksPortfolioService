namespace StocksPortfolioService.Domain.Models;

public class Portfolio : IEntity
{
    public required string Id { get; set; }

    public float CurrentTotalValue { get; set; }

    public bool IsDeleted { get; set; } = false;

    public DateTime? DeletedAt { get; set; }

    public ICollection<Stock> Stocks { get; set; } = [];
}

public class Stock
{
    public required string Ticker { get; set; }

    public required string BaseCurrency { get; set; }

    public int NumberOfShares { get; set; }
}