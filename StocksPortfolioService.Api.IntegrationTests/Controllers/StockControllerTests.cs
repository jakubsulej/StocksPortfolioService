using MongoDB.Bson;
using MongoDB.Driver;
using StocksPortfolioService.Application.Queries.GetPortfolio;
using StocksPortfolioService.Application.Queries.GetTotalPortfolioValue;
using StocksPortfolioService.Domain.Models;
using StocksPortfolioService.TestsCommon.Helpers;
using StocksPortfolioService.TestsCommon.Http;
using StocksPortfolioService.TestsCommon.Mongo;
using System.Net;
using System.Text.Json;

namespace StocksPortfolioService.Api.IntegrationTests.Controllers;

[Collection(ApplicationTestCollection.Name)]
public class StockControllerTests
{
    private readonly HttpClient _httpClient;
    private readonly IMongoClient _mongoClient;
    private readonly string _mongoDbName;
    private readonly CancellationToken _cancellationToken;

    public StockControllerTests(ApplicationTestFixture applicationTestFixture)
    {
        _httpClient = applicationTestFixture.HttpClient ?? throw new ArgumentNullException($"{nameof(applicationTestFixture.HttpClient)} was null");
        _mongoClient = applicationTestFixture.MongoClient ?? throw new ArgumentNullException($"{nameof(applicationTestFixture.MongoClient)} was null");
        _mongoDbName = applicationTestFixture.MongoDbName ?? throw new ArgumentNullException($"{nameof(applicationTestFixture.MongoDbName)} was null");
        _cancellationToken = new CancellationTokenSource(TimeSpan.FromMinutes(5)).Token;
    }

    [Fact]
    public async Task GetPortfolio_ShouldReturnCorrectPortfolio()
    {
        //Arrange
        var id = new ObjectId("61377659d24fd78398a5a54a").ToString();
        var testPortfolios = new List<Portfolio>
        {
            new() 
            {
                Id = id,
                CurrentTotalValue = 0,
                IsDeleted = false,
                DeletedAt = null,
                Stocks =
                [
                    new Stock { Ticker = "TSLA", BaseCurrency = "USD", NumberOfShares = 20 }
                ]
            }
        };

        var seeder = new MongoTestDataSeeder<Portfolio>(_mongoClient, _mongoDbName, "Portfolios");
        await seeder.SeedAsync(testPortfolios);

        var request = HttpRequestMessageBuilder.BuildGet($"api/portfolio/{id}");

        //Act
        var response = await _httpClient.SendAsync(request, _cancellationToken);

        //Assert
        await response.AssertStatusCode(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync(_cancellationToken);
        var responseAsModel = JsonSerializer.Deserialize<GetPortfolioResponse>(content, JsonTestSerializerOptionsHelper.GetSerializerOptions());
        Assert.NotNull(responseAsModel);
    }

    [Fact]
    public async Task GetTotalPortfolioValue_ShouldReturnTotalPortfolioValue_GivenMultiplePortfolioRecords()
    {
        //Arrange
        var id = new ObjectId("61377659d24fd78398a5a54b").ToString();
        var currency = "USD";
        var testPortfolios = new List<Portfolio>
        {
            new()
            {
                Id = id,
                CurrentTotalValue = 0,
                IsDeleted = false,
                DeletedAt = null,
                Stocks =
                [
                    new Stock { Ticker = "TSLA", BaseCurrency = currency, NumberOfShares = 20 },
                    new Stock { Ticker = "GME", BaseCurrency = currency, NumberOfShares = 5 },
                    new Stock { Ticker = "NAS", BaseCurrency = currency, NumberOfShares = 7 },
                ]
            }
        };

        var seeder = new MongoTestDataSeeder<Portfolio>(_mongoClient, _mongoDbName, "Portfolios");
        await seeder.SeedAsync(testPortfolios);

        var request = HttpRequestMessageBuilder.BuildGet($"api/portfolio/{id}/{currency}/total-value");

        //Act
        var response = await _httpClient.SendAsync(request, _cancellationToken);

        //Assert
        await response.AssertStatusCode(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync(_cancellationToken);
        var responseAsModel = JsonSerializer.Deserialize<GetTotalPortfolioValueResponse>(content, JsonTestSerializerOptionsHelper.GetSerializerOptions());
        Assert.NotNull(responseAsModel);
    }

    [Fact]
    public async Task DeletePortfolio_ShouldSoftDeleteRecord_GivenNonDeletedData()
    {
        //Arrange
        var id = new ObjectId("61377659d24fd78398a5a54c").ToString();
        var testPortfolios = new List<Portfolio>
        {
            new()
            {
                Id = id,
                CurrentTotalValue = 0,
                IsDeleted = false,
                DeletedAt = null,
                Stocks =
                [
                    new Stock { Ticker = "TSLA", BaseCurrency = "USD", NumberOfShares = 20 }
                ]
            }
        };

        var seeder = new MongoTestDataSeeder<Portfolio>(_mongoClient, _mongoDbName, "Portfolios");
        await seeder.SeedAsync(testPortfolios);

        var request = HttpRequestMessageBuilder.BuildDelete($"api/portfolio/{id}");

        //Act
        var response = await _httpClient.SendAsync(request, _cancellationToken);

        //Assert
        await response.AssertStatusCode(HttpStatusCode.OK);
        var updatedValue = (await seeder.GetAllAsync()).FirstOrDefault(x => x.Id == id);
        Assert.NotNull(updatedValue);
        Assert.True(updatedValue.IsDeleted);
    }
}
