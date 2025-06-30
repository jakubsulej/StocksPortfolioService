using Microsoft.Extensions.Options;
using MongoDB.Driver;
using StocksPortfolioService.Domain.Models;
using StocksPortfolioService.Domain.Repositories._Shared;
using StocksPortfolioService.Infrastructure.Database.Configuration;

namespace StocksPortfolioService.Infrastructure.Database.Repositories;

internal class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, IEntity
{
    private readonly IMongoCollection<TEntity> _collection;

    public GenericRepository(IMongoClient mongoClient, IOptions<MongoDbConfiguration> config)
    {
        var database = mongoClient.GetDatabase(config.Value.DatabaseName);
        _collection = database.GetCollection<TEntity>(config.Value.PortfolioCollectionName);
    }

    public async Task<TEntity?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        var filter = Builders<TEntity>.Filter.And(
            Builders<TEntity>.Filter.Eq(p => p.Id, id),
            Builders<TEntity>.Filter.Ne(p => p.IsDeleted, true)
        );

        return await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task DeleteByIdAsync(string id, CancellationToken cancellationToken)
    {
        var filter = Builders<TEntity>.Filter.Eq(p => p.Id, id);
        var update = Builders<TEntity>.Update
            .Set(p => p.IsDeleted, true)
            .Set(p => p.DeletedAt, DateTime.UtcNow);

        await _collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
    }
}
