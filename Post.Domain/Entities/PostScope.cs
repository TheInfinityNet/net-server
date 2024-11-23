using InfinityNetServer.BuildingBlocks.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace InfinityNetServer.Services.Post.Domain.Entities
{
    public class PostScope : MongoEntity<Guid>
    {

        [BsonElement("post_id")]
        [BsonRepresentation(BsonType.String)]
        public Guid PostId { get; set; }

        [BsonElement("who_can_see")]
        public IList<PostScope> WhoCanSee { get; set; } = [];

    }
}
