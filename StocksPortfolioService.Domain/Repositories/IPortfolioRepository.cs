using StocksPortfolioService.Domain.Models;

namespace StocksPortfolioService.Domain.Repositories;

public interface IPortfolioRepository
{
    Task DeleteByIdAsync(string id, CancellationToken cancellationToken);
    Task<Portfolio> GetByIdAsync(string id, CancellationToken cancellationToken);
}
