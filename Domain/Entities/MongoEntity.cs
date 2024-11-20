using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace InfinityNetServer.BuildingBlocks.Domain.Entities
{
    public abstract class MongoEntity<TId>
    {

        [BsonId]
        [BsonElement("id")]
        [BsonRepresentation(BsonType.String)]
        public TId Id { get; set; }

        [BsonElement("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [BsonElement("updated_at")]
        public DateTime? UpdatedAt { get; set; }

    }
}
