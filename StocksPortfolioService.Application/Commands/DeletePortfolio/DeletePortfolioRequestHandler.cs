using MediatR;
using StocksPortfolioService.Application.Services;
using System.Threading;
using System.Threading.Tasks;

namespace StocksPortfolioService.Application.Commands.DeletePortfolio;

internal class DeletePortfolioRequestHandler : IRequestHandler<DeletePortfolioRequest, DeletePortfolioResponse>
{
    private readonly IPortfolioService _portfolioService;

    public DeletePortfolioRequestHandler(IPortfolioService portfolioService)
    {
        _portfolioService = portfolioService;
    }

    public async Task<DeletePortfolioResponse> Handle(DeletePortfolioRequest request, CancellationToken cancellationToken)
    {
        await _portfolioService.DeletePortfolio(request.Id);
        return new DeletePortfolioResponse();
    }
}
