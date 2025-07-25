﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using StocksPortfolioService.Infrastructure.Models;

namespace StocksPortfolioService.Infrastructure.Database.Models;

public class PortfolioData : IEntity
{
    [BsonElement("id")]
    public ObjectId Id { get; set; }

    [BsonElement("currentTotalValue")]
    public float CurrentTotalValue { get; set; }

    [BsonElement("isDeleted")]
    public bool IsDeleted { get; set; } = false;

    [BsonElement("deletedAt")]
    public DateTime? DeletedAt { get; set; }

    [BsonElement("stocks")]
    public ICollection<StockData> Stocks { get; set; } = [];
}

public class StockData
{
    [BsonElement("ticker")]
    public string Ticker { get; set; }

    [BsonElement("baseCurrency")]
    public string BaseCurrency { get; set; }

    [BsonElement("numberOfShares")]
    public int NumberOfShares { get; set; }
}