using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace InfinityNetServer.Services.Post.Domain.Entities
{
    public class TimelinePost
    {

        [BsonElement("id")]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }

        [BsonElement("parent_id")]
        [BsonRepresentation(BsonType.String)]
        public Guid? ParentId { get; set; }

        [BsonElement("created_at")]
        [BsonRepresentation(BsonType.String)]
        public DateTime CreatedAt { get; set; }

    }
}
