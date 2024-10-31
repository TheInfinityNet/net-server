namespace InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.File
{
    public sealed record FileMetadataResponse : BaseFileMetadataResponse
    {

        public string MimeType { get; set; }

    }
}
