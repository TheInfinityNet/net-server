using MongoDB.Bson.Serialization.Attributes;

namespace InfinityNetServer.Services.File.Domain.Entities
{
    public class FileMetadata : BaseMetadata
    {

        [BsonElement("mimeType")]
        public string MimeType { get; set; }

    }
}
