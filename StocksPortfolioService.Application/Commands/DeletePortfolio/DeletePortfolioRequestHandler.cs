using MediatR;
using StocksPortfolioService.Domain.Repositories;

namespace StocksPortfolioService.Application.Commands.DeletePortfolio;

internal class DeletePortfolioRequestHandler : IRequestHandler<DeletePortfolioRequest, DeletePortfolioResponse>
{
    private readonly IPortfolioRepository _portfolioRepository;

    public DeletePortfolioRequestHandler(IPortfolioRepository portfolioRepository)
    {
        _portfolioRepository = portfolioRepository;
    }

    public async Task<DeletePortfolioResponse> Handle(DeletePortfolioRequest request, CancellationToken cancellationToken)
    {
        await _portfolioRepository.DeleteByIdAsync(request.Id, cancellationToken);
        return new DeletePortfolioResponse();
    }
}
