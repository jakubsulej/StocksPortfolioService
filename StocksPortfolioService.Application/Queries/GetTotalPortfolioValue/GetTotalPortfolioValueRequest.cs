using MediatR;

namespace StocksPortfolioService.Application.Queries.GetTotalPortfolioValue;

public record GetTotalPortfolioValueRequest(string Id, string Currency) : IRequest<GetTotalPortfolioValueResponse>;
