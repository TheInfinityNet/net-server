using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Profile;
using InfinityNetServer.BuildingBlocks.Domain.Enums;

namespace InfinityNetServer.BuildingBlocks.Application.DTOs.Others
{
    public sealed record TagFacet : BaseFacet
    {

        public TagFacet() => Type = FacetType.Tag.ToString();

        public PreviewProfileResponse Profile { get; set; }

    }
}
