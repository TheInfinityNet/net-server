using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace InfinityNetServer.Services.File.Domain.Entities
{
    public class VideoMetadata : BaseMetadata
    {

        public VideoMetadata() => Type = BuildingBlocks.Domain.Enums.FileMetadataType.Video;

        [BsonElement("thumbnail")]
        public PhotoMetadata Thumbnail { get; set; }

        [BsonElement("duration")]
        public double Duration { get; set; }

        [BsonElement("width")]
        public int Width { get; set; }

        [BsonElement("height")]
        public int Height { get; set; }

    }
}
