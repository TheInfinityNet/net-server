namespace InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.File
{
    public sealed record FileMetadataResponse : BaseMetadataResponse
    {

        public string MimeType { get; set; }

    }
}
