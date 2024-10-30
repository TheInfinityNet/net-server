namespace InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.File
{
    public sealed record VideoMetadataResponse : PhotoMetadataResponse
    {

        public PhotoMetadataResponse Thumbnail { get; set; }

        public int Duration { get; set; }

    }
}
