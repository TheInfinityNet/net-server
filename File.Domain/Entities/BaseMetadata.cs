using System;
using InfinityNetServer.Services.File.Domain.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace InfinityNetServer.Services.File.Domain.Entities
{
    public class BaseMetadata
    {

        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("owner_id")]
        public Guid OwnerId { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("owner_type")]
        public FileOwnerType OwnerType { get; set; }

        [BsonElement("type")]
        public FileType Type { get; set; }

        [BsonElement("size")]
        public long Size { get; set; }

        [BsonElement("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("created_by")]
        public Guid CreatedBy { get; set; }

        [BsonElement("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [BsonElement("updated_by")]
        public Guid? UpdatedBy { get; set; }

        [BsonElement("deleted_at")]
        public DateTime? DeletedAt { get; set; }

        [BsonElement("deleted_by")]
        public Guid? DeletedBy { get; set; }

        [BsonElement("is_deleted")]
        public bool IsDeleted { get; set; } = false;

        [BsonElement("url")]
        public string Url { get; set; }

    }

}
