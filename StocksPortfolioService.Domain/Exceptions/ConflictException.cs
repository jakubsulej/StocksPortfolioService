namespace StocksPortfolioService.Domain.Exceptions;

public class ConflictException : Exception
{
    public ConflictException(string message)
    {
        Message = message;
    }

    public override string Message { get; }
}
