using InfinityNetServer.Services.File.Domain.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace InfinityNetServer.Services.File.Infrastructure.MongoDb
{
    public class FileDbContext
    {

        protected readonly IConfiguration _configuration;

        protected readonly IMongoClient _client;

        protected readonly IMongoDatabase _database;

        public IMongoCollection<PhotoMetadata> PhotoMetadata { get; }

        public IMongoCollection<VideoMetadata> VideoMetadata { get; }

        public IMongoCollection<FileMetadata> FileMetadata { get; }

        public IMongoCollection<AudioMetadata> AudioMetadata { get; }

        public FileDbContext(IConfiguration configuration)
        {
            _configuration = configuration;

            _client = new MongoClient(MongoClientSettings
                .FromUrl(new MongoUrl(configuration["MongoDB:Connection"].ToString())));

            _database = _client.GetDatabase(configuration["MongoDB:DatabaseName"].ToString());

            PhotoMetadata = GetCollection<PhotoMetadata>();

            VideoMetadata = GetCollection<VideoMetadata>();

            AudioMetadata = GetCollection<AudioMetadata>();

            FileMetadata = GetCollection<FileMetadata>();

        }

        public IMongoCollection<TEntity> GetCollection<TEntity>()
            => _database.GetCollection<TEntity>(typeof(TEntity).Name + "s");
        

        public void DropDatabase()
            => _client.DropDatabase(_configuration.GetSection("MongoDB.DatabaseName").ToString());
        

    }
}
