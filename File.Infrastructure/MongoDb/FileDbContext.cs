﻿using InfinityNetServer.Services.File.Domain.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.File.Infrastructure.MongoDb
{
    public class FileDbContext
    {

        protected readonly IConfiguration _configuration;

        protected readonly IMongoClient _client;

        protected readonly IMongoDatabase _database;

        public IMongoCollection<BaseMetadata> BaseMetadata { get; }

        public IMongoCollection<PhotoMetadata> PhotoMetadata { get; }

        public IMongoCollection<VideoMetadata> VideoMetadata { get; }

        public IMongoCollection<FileMetadata> FileMetadata { get; }

        public IMongoCollection<AudioMetadata> AudioMetadata { get; }

        public FileDbContext(IConfiguration configuration)
        {
            _configuration = configuration;

            _client = new MongoClient(MongoClientSettings
                .FromUrl(new MongoUrl(configuration.GetSection("MongoDB.Connection").ToString())));

            _database = _client.GetDatabase(configuration.GetSection("MongoDB.DatabaseName").ToString());

            FileMetadata = GetCollection<BaseMetadata>();

        }

        public IMongoCollection<TEntity> GetCollection<TEntity>(string name = "")
        {
            if (string.IsNullOrEmpty(name))
                name = typeof(TEntity).Name + "s";

            return _database.GetCollection<TEntity>(name);
        }

        public void DropDatabase()
        {
            _client.DropDatabase(_configuration.GetSection("MongoDB.DatabaseName").ToString());
        }

    }
}