namespace InfinityNetServer.BuildingBlocks.Application.DTOs.Others
{
    public sealed record ProfileIdWithMutualCount
    {
        public string ProfileId { get; set; }

        public int Count { get; set; }
    }
}
