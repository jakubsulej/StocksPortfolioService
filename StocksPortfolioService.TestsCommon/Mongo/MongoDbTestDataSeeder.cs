using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace StocksPortfolioService.TestsCommon.Mongo;

public class MongoTestDataSeeder<T>
{
    private readonly IMongoCollection<T> _collection;

    public MongoTestDataSeeder(IMongoClient client, string databaseName, string collectionName)
    {
        var database = client.GetDatabase(databaseName);
        _collection = database.GetCollection<T>(collectionName);
    }

    public async Task SeedAsync(IEnumerable<T> items)
    {
        await _collection.DeleteManyAsync(FilterDefinition<T>.Empty); // clean slate
        await _collection.InsertManyAsync(items);
    }

    public async Task ClearAsync()
    {
        await _collection.DeleteManyAsync(FilterDefinition<T>.Empty);
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _collection.Find(FilterDefinition<T>.Empty).ToListAsync();
    }
}
