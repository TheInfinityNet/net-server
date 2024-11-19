using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;

namespace InfinityNetServer.Services.Post.Domain.Entities
{
    public class TimelinePost
    {

        [BsonElement("post_id")]
        [BsonRepresentation(BsonType.String)]
        public Guid PostId { get; set; }

        [BsonElement("created_at")]
        [BsonRepresentation(BsonType.String)]
        public DateTime CreatedAt { get; set; }

    }
}
