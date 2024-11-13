namespace InfinityNetServer.BuildingBlocks.Application.DTOs.Requests
{
    public sealed record ConfirmSaveFileRequest
    {

        public string OwnerId { get; set; }

        public string FileMetadataId { get; set; }

    }
}
