using MediatR;

namespace StocksPortfolioService.Application.Commands.DeletePortfolio;

public record DeletePortfolioRequest(string Id) : IRequest<DeletePortfolioResponse>;
