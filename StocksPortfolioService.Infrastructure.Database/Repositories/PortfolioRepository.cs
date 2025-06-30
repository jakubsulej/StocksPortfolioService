using Microsoft.Extensions.Options;
using MongoDB.Driver;
using StocksPortfolioService.Domain.Models;
using StocksPortfolioService.Domain.Repositories;
using StocksPortfolioService.Infrastructure.Database.Configuration;

namespace StocksPortfolioService.Infrastructure.Database.Repositories;

internal class PortfolioRepository(IMongoClient mongoClient, IOptions<MongoDbConfiguration> config) 
    : GenericRepository<Portfolio>(mongoClient, config), IPortfolioRepository;
