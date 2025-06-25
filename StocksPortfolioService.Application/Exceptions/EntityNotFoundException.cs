using System;

namespace StocksPortfolioService.Application.Exceptions;

public class EntityNotFoundException : ArgumentNullException
{
    public EntityNotFoundException(string message)
    {
        Message = message;
    }

    public override string Message { get; }
}
