using MediatR;
using Microsoft.AspNetCore.Mvc;
using StocksPortfolioService.Api.Helpers;
using StocksPortfolioService.Application.Commands.DeletePortfolio;
using StocksPortfolioService.Application.Queries.GetPortfolio;
using StocksPortfolioService.Application.Queries.GetTotalPortfolioValue;

namespace StocksPortfolioService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/[controller]")]
public class PortfolioController(IMediator mediator) : ControllerBaseProcessor(mediator)
{
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetPortfolioResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public Task<IActionResult> Get(string id, CancellationToken cancellationToken)
        => Process(new GetPortfolioRequest(id),  cancellationToken);

    [HttpGet("{portfolioId}/{currency}/total-value")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetTotalPortfolioValueResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public Task<IActionResult> GetTotalPortfolioValue(string portfolioId, string currency, CancellationToken cancellationToken)
        => Process(new GetTotalPortfolioValueRequest(portfolioId, currency), cancellationToken);

    [HttpDelete("{portfolioId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public Task<IActionResult> DeletePortfolio(string portfolioId, CancellationToken cancellationToken)
        => Process(new DeletePortfolioRequest(portfolioId), cancellationToken);
}