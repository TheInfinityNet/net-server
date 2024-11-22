namespace InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.File
{
    public sealed record AudioMetadataResponse : BaseMetadataResponse
    {

        public int Duration { get; set; }

    }
}
