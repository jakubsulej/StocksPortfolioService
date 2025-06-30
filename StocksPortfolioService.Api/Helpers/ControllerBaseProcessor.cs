using MediatR;
using Microsoft.AspNetCore.Mvc;
using StocksPortfolioService.Domain.Exceptions;
using System.Net;

namespace StocksPortfolioService.Api.Helpers;

public abstract class ControllerBaseProcessor(IMediator mediator) : ControllerBase
{
    public async Task<IActionResult> Process<T>(IRequest<T> request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await mediator.Send(request, cancellationToken);
            return response is null ? NoContent() : Ok(response);
        }
        catch (EntityNotFoundException ex)
        {
            return Problem(title: ex.Message, detail: ex.InnerException?.Message, statusCode: (int)HttpStatusCode.NotFound);
        }
        catch (ConflictException ex)
        {
            return Problem(title: ex.Message, detail: ex.InnerException?.Message, statusCode: (int)HttpStatusCode.Conflict);
        }
        catch (Exception ex)
        {
            return Problem(title: ex.Message, detail: ex.InnerException?.Message, statusCode: (int)HttpStatusCode.InternalServerError);
        }
    }
}
