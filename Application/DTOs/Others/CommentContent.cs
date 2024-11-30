using System.Collections.Generic;

namespace InfinityNetServer.BuildingBlocks.Application.DTOs.Others
{
    public sealed record CommentContent
    {

        public string Text { get; set; }

        public IList<TagFacet> TagFacets { get; set; }

    }
}
