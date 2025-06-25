using MediatR;
using StocksPortfolioService.Application.Exceptions;
using StocksPortfolioService.Application.Services;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StocksPortfolioService.Application.Queries.GetPortfolio;

internal class GetPortfolioRequestHandler : IRequestHandler<GetPortfolioRequest, GetPortfolioResponse>
{
    private readonly IPortfolioService _portfolioService;

    public GetPortfolioRequestHandler(IPortfolioService portfolioService)
    {
        _portfolioService = portfolioService;
    }

    public async Task<GetPortfolioResponse> Handle(GetPortfolioRequest request, CancellationToken cancellationToken)
    {
        var portfolio = await _portfolioService.GetPortfolioById(request.Id)
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
