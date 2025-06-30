namespace StocksPortfolioService.Domain.Models;

public interface IEntity
{
    public string Id { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
}
