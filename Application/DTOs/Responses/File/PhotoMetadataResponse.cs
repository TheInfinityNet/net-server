namespace InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.File
{
    public record PhotoMetadataResponse : BaseMetadataResponse
    {

        public int Width { get; set; }

        public int Height { get; set; }

    }
}
