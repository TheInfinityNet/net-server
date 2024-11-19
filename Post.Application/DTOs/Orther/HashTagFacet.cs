using InfinityNetServer.BuildingBlocks.Application.DTOs.Others;
using InfinityNetServer.BuildingBlocks.Domain.Enums;

namespace InfinityNetServer.Services.Post.Application.DTOs.Orther
{
    public sealed record HashTagFacet : BaseFacet
    {

        public HashTagFacet() => Type = FacetType.Hashtag.ToString();

        public string Tag { get; set; }

    }
}
