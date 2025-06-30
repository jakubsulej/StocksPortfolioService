using MongoDB.Bson;

namespace StocksPortfolioService.Infrastructure.Models;

public interface IEntity
{
    public ObjectId Id { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
}
