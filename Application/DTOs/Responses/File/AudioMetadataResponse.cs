namespace InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.File
{
    public sealed record AudioMetadataResponse : BaseFileMetadataResponse
    {

        public int Duration { get; set; }

    }
}
