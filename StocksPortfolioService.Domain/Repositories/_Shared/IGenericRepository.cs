using StocksPortfolioService.Domain.Models;

namespace StocksPortfolioService.Domain.Repositories._Shared;

public interface IGenericRepository<TEntity> where TEntity : class, IEntity
{
    Task DeleteByIdAsync(string id, CancellationToken cancellationToken);
    Task<TEntity?> GetByIdAsync(string id, CancellationToken cancellationToken);
}
