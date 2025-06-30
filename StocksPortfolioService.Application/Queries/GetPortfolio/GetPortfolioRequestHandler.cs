using MediatR;
using StocksPortfolioService.Application.Services;
using StocksPortfolioService.Domain.Exceptions;
using StocksPortfolioService.Domain.Repositories;

namespace StocksPortfolioService.Application.Queries.GetPortfolio;

internal class GetPortfolioRequestHandler : IRequestHandler<GetPortfolioRequest, GetPortfolioResponse>
{
    private readonly IPortfolioRepository _portfolioRepostory;

    public GetPortfolioRequestHandler(IPortfolioRepository portfolioRepository)
    {
        _portfolioRepostory = portfolioRepository;
    }

    public async Task<GetPortfolioResponse> Handle(GetPortfolioRequest request, CancellationToken cancellationToken)
    {
        var portfolio = await _portfolioRepostory.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException($"Portfolio with id {request.Id} was not found");

        return new GetPortfolioResponse
        {
            Id = portfolio.Id,
            CurrentTotalValue = portfolio.CurrentTotalValue,
            Stocks = portfolio.Stocks.Select(s => new GetPortfolioResponse.StockData
            {
                BaseCurrency = s.BaseCurrency,
                NumberOfShares = s.NumberOfShares,
                Ticker = s.Ticker,
            })
        };
    }
}
