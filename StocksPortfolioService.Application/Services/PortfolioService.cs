using MongoDB.Bson;
using StocksPortfolioService.Infrastructure.Models;
using StocksPortfolioService.Infrastructure.Repositories;
using System.Threading.Tasks;

namespace StocksPortfolioService.Application.Services;

public interface IPortfolioService
{
    Task DeletePortfolio(string portfolioId);
    Task<PortfolioData> GetPortfolioById(string portfolioId);
}

internal class PortfolioService : IPortfolioService
{
    private readonly IPortfolioRepository _portfolioRepository;

    public PortfolioService(IPortfolioRepository portfolioRepository)
    {
        _portfolioRepository = portfolioRepository;
    }

    public Task<PortfolioData> GetPortfolioById(string portfolioId)
    {
        var id = ObjectId.Parse(portfolioId);
        return _portfolioRepository.GetPortfolio(id);
    }

    public Task DeletePortfolio(string portfolioId)
    {
        var id = ObjectId.Parse(portfolioId);
        return _portfolioRepository.DeletePortfolio(id);
    }
}
