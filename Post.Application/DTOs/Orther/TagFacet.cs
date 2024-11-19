using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Profile;

namespace InfinityNetServer.Services.Post.Application.DTOs.Orther
{
    public sealed record TagFacet : BaseFacet
    {

        public PreviewProfileResponse Profile { get; set; }

    }
}
