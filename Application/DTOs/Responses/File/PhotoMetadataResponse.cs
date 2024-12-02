namespace InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.File
{
    public record PhotoMetadataResponse : BaseMetadataResponse
    {

        public int Width { get; set; } = 500;

        public int Height { get; set; } = 500;

    }
}
