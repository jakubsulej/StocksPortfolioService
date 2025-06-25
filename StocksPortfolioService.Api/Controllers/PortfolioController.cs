using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StocksPortfolioService.Application.Commands.DeletePortfolio;
using StocksPortfolioService.Application.Exceptions;
using StocksPortfolioService.Application.Queries.GetPortfolio;
using StocksPortfolioService.Application.Queries.GetTotalPortfolioValue;
using System;
using System.Net;
using System.Threading.Tasks;

namespace StocksPortfolioService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PortfolioController : ControllerBase
{
    private readonly IMediator _mediator;

    public PortfolioController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetPortfolioResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Get(string id)
    {
        try
        {
            var response = await _mediator.Send(new GetPortfolioRequest(id));
            return Ok(response);
        }
        catch (EntityNotFoundException ex)
        {
            return Problem(title: ex.Message, detail: ex.InnerException?.Message, statusCode: (int)HttpStatusCode.NotFound);
        }
        catch (Exception ex)
        {
            return Problem(title: ex.Message, detail: ex.InnerException?.Message, statusCode: (int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet("/value")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetTotalPortfolioValueResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> GetTotalPortfolioValue(string portfolioId, string currency = "USD")
    {
        try
        {
            var response = await _mediator.Send(new GetTotalPortfolioValueRequest(portfolioId, currency));
            return Ok(response);
        }
        catch (EntityNotFoundException ex)
        {
            return Problem(title: ex.Message, detail: ex.InnerException?.Message, statusCode: (int)HttpStatusCode.NotFound);
        }
        catch (Exception ex)
        {
            return Problem(title: ex.Message, detail: ex.InnerException?.Message, statusCode: (int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet("/delete")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(void))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> DeletePortfolio(string portfolioId)
    {
        try
        {
            await _mediator.Send(new DeletePortfolioRequest(portfolioId));
            return Ok();
        }
        catch (Exception ex)
        {
            return Problem(title: ex.Message, detail: ex.InnerException?.Message, statusCode: (int)HttpStatusCode.InternalServerError);
        }
    }
}