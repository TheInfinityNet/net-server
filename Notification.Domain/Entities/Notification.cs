using InfinityNetServer.BuildingBlocks.Domain.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace InfinityNetServer.Services.Notification.Domain.Entities
{
    public class Notification
    {

        [BsonId]
        [BsonElement("id")]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [BsonElement("account_id")]
        [BsonRepresentation(BsonType.String)]
        public Guid AccountId { get; set; }

        [BsonElement("type")]
        public NotificationType Type { get; set; }

        [BsonElement("thumbnail_id")]
        public Guid? ThumbnailId { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("content")]
        public string Content { get; set; }

        [BsonElement("is_read")]
        public bool IsRead { get; set; } = false;

        [BsonElement("permalink")]
        public string Permalink { get; set; }

        [BsonElement("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [BsonElement("updated_at")]
        public DateTime? UpdatedAt { get; set; }

    }
}
