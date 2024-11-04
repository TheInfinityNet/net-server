using MongoDB.Bson.Serialization.Attributes;

namespace InfinityNetServer.Services.File.Domain.Entities
{
    public class AudioMetadata : BaseMetadata
    {

        [BsonElement("duration")]
        public double Duration { get; set; }

    }
}
