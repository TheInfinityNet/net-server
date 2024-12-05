namespace InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Profile
{
    public sealed record PageProfileResponse : BaseProfileResponse
    {

        public string Description { get; set; }

        public string GenerateName()
        {
            return Name;
        }
    }
}
