using Microsoft.AspNetCore.TestHost;
using MongoDB.Driver;
using Testcontainers.MongoDb;

namespace StocksPortfolioService.Api.IntegrationTests;

public class ApplicationTestFixture : IAsyncLifetime
{
    public HttpClient? HttpClient { get; private set; }
    public TestServer? TestServer { get; private set; }
    public MongoDbContainer MongoDbContainer { get; private set; }
    public IMongoClient MongoClient { get; private set; }
    public string MongoDbName = "portfolioServiceDb";

    public ApplicationTestFixture()
    {
        MongoDbContainer = new MongoDbBuilder()
            .WithPortBinding(27017)
            .Build();
    }

    public async Task InitializeAsync()
    {
        await MongoDbContainer.StartAsync();
        var mongoDbConnectionString = MongoDbContainer.GetConnectionString();

        var testApplication = new WebApiApplicationFactory(
            mongoDbConnectionString,
            MongoDbName);

        MongoClient = new MongoClient(mongoDbConnectionString);

        HttpClient = testApplication.CreateClient();
        TestServer = testApplication.Server;
    }

    public async Task DisposeAsync()
    {
        HttpClient?.Dispose();
        TestServer?.Dispose();
        await MongoDbContainer.DisposeAsync();
    }
}

[CollectionDefinition(Name)]
public class ApplicationTestCollection : ICollectionFixture<ApplicationTestFixture>
{
    public const string Name = "Application test collection";
}
