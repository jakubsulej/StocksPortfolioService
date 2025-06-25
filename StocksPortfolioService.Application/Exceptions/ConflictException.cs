using System;

namespace StocksPortfolioService.Application.Exceptions;

public class ConflictException : Exception
{
    public ConflictException(string message)
    {
        Message = message;
    }

    public override string Message { get; }
}
