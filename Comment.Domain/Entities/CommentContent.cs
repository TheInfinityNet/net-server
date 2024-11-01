using InfinityNetServer.BuildingBlocks.Domain.Entities;
using System.Collections.Generic;

namespace InfinityNetServer.Services.Comment.Domain.Entities
{
    public class CommentContent
    {

        public string Text { get; set; }

        public ICollection<TagFacet> TagFacets { get; set; } = [];

    }
}
