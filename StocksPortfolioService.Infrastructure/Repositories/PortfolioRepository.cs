using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using StocksPortfolioService.Infrastructure.Configuration;
using StocksPortfolioService.Infrastructure.Models;
using System;
using System.Threading.Tasks;

namespace StocksPortfolioService.Infrastructure.Repositories;

public interface IPortfolioRepository
{
    Task<PortfolioData> GetPortfolio(ObjectId id);
    Task DeletePortfolio(ObjectId id);
}

internal class PortfolioRepository : IPortfolioRepository
{
    private readonly IMongoCollection<PortfolioData> _collection;

    public PortfolioRepository(IMongoClient mongoClient, IOptions<MongoDbConfiguration> config)
    {
        var database = mongoClient.GetDatabase(config.Value.DatabaseName);
        _collection = database.GetCollection<PortfolioData>(config.Value.PortfolioCollectionName);
    }

    public async Task<PortfolioData> GetPortfolio(ObjectId id)
    {
        var filter = Builders<PortfolioData>.Filter.And(
            Builders<PortfolioData>.Filter.Eq(p => p.Id, id),
            Builders<PortfolioData>.Filter.Ne(p => p.IsDeleted, true)
        );

        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task DeletePortfolio(ObjectId id)
    {
        var filter = Builders<PortfolioData>.Filter.Eq(p => p.Id, id);
        var update = Builders<PortfolioData>.Update
            .Set(p => p.IsDeleted, true)
            .Set(p => p.DeletedAt, DateTime.UtcNow);

        await _collection.UpdateOneAsync(filter, update);
    }
}
