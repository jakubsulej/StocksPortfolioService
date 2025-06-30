using FakeItEasy;
using MongoDB.Bson;
using StocksPortfolioService.Application.Queries.GetTotalPortfolioValue;
using StocksPortfolioService.Application.Services;
using StocksPortfolioService.Domain.Consts;
using StocksPortfolioService.Domain.Exceptions;
using StocksPortfolioService.Domain.Models;
using StocksPortfolioService.Domain.Repositories;

namespace StocksPortfolioService.Application.Tests.Queries.GetTotalPortfolioValue;

public class GetTotalPortfolioRequestHandlerTests
{
    private readonly IPortfolioRepository _portfolioRepository = A.Fake<IPortfolioRepository>();
    private readonly IStockService _stockService = A.Fake<IStockService>();
    private readonly ICurrencyLayerService _currencyService = A.Fake<ICurrencyLayerService>();
    private readonly GetTotalPortfolioValueRequestHandler _sut;

    public GetTotalPortfolioRequestHandlerTests()
    {
        _sut = new GetTotalPortfolioValueRequestHandler(_portfolioRepository, _stockService, _currencyService);
    }

    [Fact]
    public async Task Returns_Correct_Total_When_All_Currencies_Are_USD()
    {
        // Arrange
        var portfolioId = ObjectId.GenerateNewId().ToString();
        var currency = CurrencyConsts.USD;
        var ticker = "TSLA";
        var request = new GetTotalPortfolioValueRequest(portfolioId.ToString(), currency); ;

        var portfolio = new Portfolio
        {
            Id = portfolioId,
            Stocks =
            [
                new Stock { Ticker = ticker, BaseCurrency = currency, NumberOfShares = 2 }
            ]
        };

        A.CallTo(() => _portfolioRepository.GetByIdAsync(portfolioId.ToString(), A<CancellationToken>._)).Returns(portfolio);
        A.CallTo(() => _stockService.GetCurrentStockPrice(ticker)).Returns(Task.FromResult((500m, CurrencyConsts.USD)));
        A.CallTo(() => _currencyService.GetCachedCurrencies(A<CancellationToken>._))
            .Returns(new Dictionary<string, decimal>
            {
                { "USDUSD", 1m }
            });

        // Act
        var result = await _sut.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(1000m, result.TotalAmount); // 2 * $500
    }

    [Fact]
    public async Task Converts_To_USD_When_BaseCurrency_Is_Not_USD()
    {
        // Arrange
        var portfolioId = ObjectId.GenerateNewId().ToString();
        var request = new GetTotalPortfolioValueRequest(portfolioId.ToString(), CurrencyConsts.USD);

        var portfolio = new Portfolio
        {
            Id = portfolioId,
            Stocks =
            [
                new Stock { Ticker = "TSLA", BaseCurrency = "EUR", NumberOfShares = 2 }
            ]
        };

        A.CallTo(() => _portfolioRepository.GetByIdAsync(portfolioId.ToString(), A<CancellationToken>._)).Returns(portfolio);
        A.CallTo(() => _stockService.GetCurrentStockPrice("TSLA")).Returns(Task.FromResult((500m, "EUR")));
        A.CallTo(() => _currencyService.GetCachedCurrencies(A<CancellationToken>._))
            .Returns(new Dictionary<string, decimal>
            {
            { "USDEUR", 0.5m }, // 1 USD = 0.5 EUR => 1 EUR = 2 USD
            { "USDUSD", 1m }
            });

        // Act
        var result = await _sut.Handle(request, CancellationToken.None);

        // Assert
        // EUR price = 500; USD equivalent = 500 / 0.5 = 1000; total = 2 * 1000 = 2000
        Assert.Equal(2000m, result.TotalAmount);
    }

    [Fact]
    public async Task Converts_From_NonUsd_To_TargetCurrency()
    {
        // Arrange
        var portfolioId = ObjectId.GenerateNewId().ToString();
        var request = new GetTotalPortfolioValueRequest(portfolioId.ToString(), "SEK");

        var portfolio = new Portfolio
        {
            Id = portfolioId,
            Stocks =
            [
                new Stock { Ticker = "GME", BaseCurrency = "EUR", NumberOfShares = 1 }
            ]
        };

        A.CallTo(() => _portfolioRepository.GetByIdAsync(portfolioId.ToString(), A<CancellationToken>._)).Returns(portfolio);
        A.CallTo(() => _stockService.GetCurrentStockPrice("GME")).Returns(Task.FromResult((100m, "EUR")));

        A.CallTo(() => _currencyService.GetCachedCurrencies(A<CancellationToken>._))
            .Returns(new Dictionary<string, decimal>
            {
            { "USDEUR", 0.5m }, // 1 EUR = 2 USD
            { "USDSEK", 10m }   // 1 USD = 10 SEK
            });

        // Act
        var result = await _sut.Handle(request, CancellationToken.None);

        // Assert
        // EUR → USD: 100 / 0.5 = 200 USD
        // USD → SEK: 200 * 10 = 2000 SEK
        Assert.Equal(2000m, result.TotalAmount);
    }

    [Fact]
    public async Task Throws_When_Portfolio_Not_Found()
    {
        // Arrange
        var portfolioId = ObjectId.GenerateNewId().ToString();
        var request = new GetTotalPortfolioValueRequest(portfolioId.ToString(), CurrencyConsts.USD);

        A.CallTo(() => _portfolioRepository.GetByIdAsync(portfolioId.ToString(), A<CancellationToken>._)).Returns<Portfolio?>(null);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            _sut.Handle(request, CancellationToken.None));
    }
}
