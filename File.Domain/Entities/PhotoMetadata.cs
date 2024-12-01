using MongoDB.Bson.Serialization.Attributes;

namespace InfinityNetServer.Services.File.Domain.Entities
{
    public class PhotoMetadata : BaseMetadata
    {

        public PhotoMetadata() => Type = BuildingBlocks.Domain.Enums.FileMetadataType.Photo;

        [BsonElement("width")]
        public int Width { get; set; }

        [BsonElement("height")]
        public int Height { get; set; }

    }
}
