using InfinityNetServer.BuildingBlocks.Domain.Entities;
using InfinityNetServer.BuildingBlocks.Domain.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace InfinityNetServer.Services.Notification.Domain.Entities
{
    public class Notification : MongoEntity<Guid>
    {
        public Notification() => Id = Guid.NewGuid();
        
        [BsonElement("account_id")]
        [BsonRepresentation(BsonType.String)]
        public Guid AccountId { get; set; }

        [BsonElement("entity_id")]
        public string? EntityId { get; set; }

        [BsonElement("type")]
        public NotificationType Type { get; set; }

        [BsonElement("thumbnail_id")]
        [BsonRepresentation(BsonType.String)]
        public Guid? ThumbnailId { get; set; }

        [BsonElement("title_params")]
        public string[] TitleParams { get; set; }

        [BsonElement("content_params")]
        public string[] ContentParams { get; set; }

        [BsonElement("is_read")]
        public bool IsRead { get; set; } = false;

    }
}
