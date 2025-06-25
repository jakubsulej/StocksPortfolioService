using MediatR;

namespace StocksPortfolioService.Application.Queries.GetPortfolio;

public record GetPortfolioRequest(string Id) : IRequest<GetPortfolioResponse>;
