using Microsoft.AspNetCore.TestHost;
using MongoDB.Bson;
using MongoDB.Driver;
using StocksPortfolioService.Application.Queries.GetPortfolio;
using StocksPortfolioService.Application.Queries.GetTotalPortfolioValue;
using StocksPortfolioService.Infrastructure.Models;
using StocksPortfolioService.TestsCommon.Http;
using StocksPortfolioService.TestsCommon.Mongo;
using System.Net;
using System.Text.Json;

namespace StocksPortfolioService.Api.IntegrationTests.Controllers;

[Collection(ApplicationTestCollection.Name)]
public class StockControllerTests
{
    private readonly TestServer _testServer;
    private readonly HttpClient _httpClient;
    private readonly IMongoClient _mongoClient;
    private readonly string _mongoDbName;
    private readonly CancellationToken _cancellationToken;

    public StockControllerTests(ApplicationTestFixture applicationTestFixture)
    {
        _testServer = applicationTestFixture.TestServer ?? throw new ArgumentNullException($"{nameof(applicationTestFixture.TestServer)} was null");
        _httpClient = applicationTestFixture.HttpClient ?? throw new ArgumentNullException($"{nameof(applicationTestFixture.HttpClient)} was null");
        _mongoClient = applicationTestFixture.MongoClient ?? throw new ArgumentNullException($"{nameof(applicationTestFixture.MongoClient)} was null");
        _mongoDbName = applicationTestFixture.MongoDbName ?? throw new ArgumentNullException($"{nameof(applicationTestFixture.MongoDbName)} was null");
        _cancellationToken = new CancellationTokenSource(TimeSpan.FromMinutes(5)).Token;
    }

    [Fact]
    public async Task GetPortfolio_ShouldReturnCorrectPortfolio()
    {
        //Arrange
        var id = new ObjectId("61377659d24fd78398a5a54a");
        var testPortfolios = new List<PortfolioData>
        {
            new() 
            {
                Id = id,
                CurrentTotalValue = 0,
                IsDeleted = false,
                DeletedAt = null,
                Stocks =
                [
                    new StockData { Ticker = "TSLA", BaseCurrency = "USD", NumberOfShares = 20 }
                ]
            }
        };

        var seeder = new MongoTestDataSeeder<PortfolioData>(_mongoClient, _mongoDbName, "Portfolios");
        await seeder.SeedAsync(testPortfolios);

        var request = HttpRequestMessageBuilder.BuildGet($"api/portfolio/{id}");

        //Act
        var response = await _httpClient.SendAsync(request, _cancellationToken);

        //Assert
        await response.AssertStatusCode(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync(_cancellationToken);
        var responseAsModel = JsonSerializer.Deserialize<GetPortfolioResponse>(content);
        Assert.NotNull(responseAsModel);
    }

    [Fact]
    public async Task GetTotalPortfolioValue_ShouldReturnTotalPortfolioValue_GivenMultiplePortfolioRecords()
    {
        //Arrange
        var id = new ObjectId("61377659d24fd78398a5a54b");
        var testPortfolios = new List<PortfolioData>
        {
            new()
            {
                Id = id,
                CurrentTotalValue = 0,
                IsDeleted = false,
                DeletedAt = null,
                Stocks =
                [
                    new StockData { Ticker = "TSLA", BaseCurrency = "USD", NumberOfShares = 20 },
                    new StockData { Ticker = "GME", BaseCurrency = "USD", NumberOfShares = 5 },
                    new StockData { Ticker = "NAS", BaseCurrency = "NOK", NumberOfShares = 7 },
                ]
            }
        };

        var seeder = new MongoTestDataSeeder<PortfolioData>(_mongoClient, _mongoDbName, "Portfolios");
        await seeder.SeedAsync(testPortfolios);

        var request = HttpRequestMessageBuilder.BuildGet($"/value?portfolioId={id}");

        //Act
        var response = await _httpClient.SendAsync(request, _cancellationToken);

        //Assert
        await response.AssertStatusCode(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync(_cancellationToken);
        var responseAsModel = JsonSerializer.Deserialize<GetTotalPortfolioValueResponse>(content);
        Assert.NotNull(responseAsModel);
    }

    [Fact]
    public async Task DeletePortfolio_ShouldSoftDeleteRecord_GivenNonDeletedData()
    {
        //Arrange
        var id = new ObjectId("61377659d24fd78398a5a54c");
        var testPortfolios = new List<PortfolioData>
        {
            new()
            {
                Id = id,
                CurrentTotalValue = 0,
                IsDeleted = false,
                DeletedAt = null,
                Stocks =
                [
                    new StockData { Ticker = "TSLA", BaseCurrency = "USD", NumberOfShares = 20 }
                ]
            }
        };

        var seeder = new MongoTestDataSeeder<PortfolioData>(_mongoClient, _mongoDbName, "Portfolios");
        await seeder.SeedAsync(testPortfolios);

        var request = HttpRequestMessageBuilder.BuildGet($"/delete?portfolioId={id}");

        //Act
        var response = await _httpClient.SendAsync(request, _cancellationToken);

        //Assert
        await response.AssertStatusCode(HttpStatusCode.OK);
        var updatedValue = (await seeder.GetAllAsync()).FirstOrDefault(x => x.Id == id);
        Assert.NotNull(updatedValue);
        Assert.True(updatedValue.IsDeleted);
    }
}
