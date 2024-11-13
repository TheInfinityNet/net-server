using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace InfinityNetServer.BuildingBlocks.Infrastructure.MongoDB
{
    public class MongoDbContext
    {

        protected readonly IConfiguration _configuration;

        protected readonly IMongoClient _client;

        protected readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            _configuration = configuration;

            _client = new MongoClient(MongoClientSettings
                .FromUrl(new MongoUrl(configuration["MongoDB:Connection"].ToString())));

            _database = _client.GetDatabase(configuration["MongoDB:DatabaseName"].ToString());

        }

        public IMongoCollection<TEntity> GetCollection<TEntity>()
            => _database.GetCollection<TEntity>(typeof(TEntity).Name + "s");


        public void DropDatabase()
            => _client.DropDatabase(_configuration.GetSection("MongoDB.DatabaseName").ToString());


    }
}
