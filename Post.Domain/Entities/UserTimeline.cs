using InfinityNetServer.BuildingBlocks.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace InfinityNetServer.Services.Post.Domain.Entities
{
    public class UserTimeline : MongoEntity<Guid>
    {

        [BsonElement("profile_id")]
        [BsonRepresentation(BsonType.String)]
        public Guid ProfileId { get; set; }

        public IList<TimelinePost> Posts { get; set; } = [];

    }
}
