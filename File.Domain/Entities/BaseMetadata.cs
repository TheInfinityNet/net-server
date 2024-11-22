using System;
using InfinityNetServer.BuildingBlocks.Domain.Entities;
using InfinityNetServer.BuildingBlocks.Domain.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace InfinityNetServer.Services.File.Domain.Entities
{
    public class BaseMetadata : MongoEntity<Guid>
    {

        public BaseMetadata() => Id = Guid.NewGuid();

        [BsonElement("owner_id")]
        [BsonRepresentation(BsonType.String)]
        public Guid? OwnerId { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("owner_type")]
        public FileOwnerType? OwnerType { get; set; }

        [BsonElement("type")]
        public FileMetadataType Type { get; set; }

        [BsonElement("size")]
        public long Size { get; set; }

        [BsonElement("created_by")]
        [BsonRepresentation(BsonType.String)]
        public Guid? CreatedBy { get; set; }

        [BsonElement("updated_by")]
        [BsonRepresentation(BsonType.String)]
        public Guid? UpdatedBy { get; set; }

        [BsonElement("deleted_at")]
        public DateTime? DeletedAt { get; set; }

        [BsonElement("deleted_by")]
        [BsonRepresentation(BsonType.String)]
        public Guid? DeletedBy { get; set; }

        [BsonElement("is_deleted")]
        public bool IsDeleted { get; set; } = false;

    }

}
