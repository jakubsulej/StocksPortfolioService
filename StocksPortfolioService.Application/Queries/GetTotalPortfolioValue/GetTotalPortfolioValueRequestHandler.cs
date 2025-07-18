﻿using MediatR;
using StocksPortfolioService.Application.Services;
using StocksPortfolioService.Domain.Consts;
using StocksPortfolioService.Domain.Exceptions;
using StocksPortfolioService.Domain.Repositories;

namespace StocksPortfolioService.Application.Queries.GetTotalPortfolioValue;

public class GetTotalPortfolioValueRequestHandler : IRequestHandler<GetTotalPortfolioValueRequest, GetTotalPortfolioValueResponse>
{
    private readonly IPortfolioRepository _portfolioRepository;
    private readonly IStockService _stockService;
    private readonly ICurrencyLayerService _currencyLayerService;

    public GetTotalPortfolioValueRequestHandler(
        IPortfolioRepository portfolioRepository,
        IStockService stockService, 
        ICurrencyLayerService currencyLayerService)
    {
        _portfolioRepository = portfolioRepository;
        _stockService = stockService;
        _currencyLayerService = currencyLayerService;
    }

    public async Task<GetTotalPortfolioValueResponse> Handle(GetTotalPortfolioValueRequest request, CancellationToken cancellationToken)
    {
        var targetCurrency = request.Currency;
        var portfolio = await _portfolioRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException($"Portfolio with id {request.Id} was not found");

        var currencies = await _currencyLayerService.GetCachedCurrencies(cancellationToken)
            ?? throw new EntityNotFoundException("Currencies were not found");

        decimal totalAmount = 0;

        foreach (var stock in portfolio.Stocks)
        {
            var stockPriceResponse = await _stockService.GetCurrentStockPrice(stock.Ticker);
            var price = stockPriceResponse.Price;
            var valueInBaseCurrency = price * stock.NumberOfShares;

            if (stock.BaseCurrency == targetCurrency)
            {
                totalAmount += valueInBaseCurrency;
            }
            else
            {
                var toUsdKey = $"USD{stock.BaseCurrency}";
                if (!currencies.TryGetValue(toUsdKey, out var baseToUsdRate) || baseToUsdRate == 0)
                    throw new EntityNotFoundException($"Missing or invalid conversion rate for {toUsdKey}");

                var valueInUsd = valueInBaseCurrency / baseToUsdRate;

                if (targetCurrency == CurrencyConsts.USD)
                {
                    totalAmount += valueInUsd;
                }
                else
                {
                    var fromUsdKey = $"USD{targetCurrency}";
                    if (!currencies.TryGetValue(fromUsdKey, out var usdToTargetRate) || usdToTargetRate == 0)
                        throw new EntityNotFoundException($"Missing or invalid conversion rate for {fromUsdKey}");

                    totalAmount += valueInUsd * usdToTargetRate;
                }
            }
        }

        return new GetTotalPortfolioValueResponse
        {
            TotalAmount = totalAmount
        };
    }
}
