using InfinityNetServer.BuildingBlocks.Application.DTOs.Others;
using System.Collections.Generic;

namespace InfinityNetServer.Services.Post.Application.DTOs.Orther
{
    public sealed record TextContent
    {

        public string Text { get; set; }

        public IList<TagFacet> TagFacets { get; set; }

        public IList<HashTagFacet> HashtagFacets { get; set; }

    }
}
