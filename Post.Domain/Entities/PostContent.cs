using InfinityNetServer.BuildingBlocks.Domain.Entities;
using System.Collections.Generic;

namespace InfinityNetServer.Services.Post.Domain.Entities
{
    public class PostContent
    {

        public string Text { get; set; }

        public ICollection<TagFacet> TagFacets { get; set; } = [];

        public ICollection<HashtagFacet> HashtagFacets { get; set; } = [];

    }
}
