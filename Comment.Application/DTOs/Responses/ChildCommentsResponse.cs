using System.Collections.Generic;
using System;

namespace InfinityNetServer.Services.Comment.Application.DTOs.Responses
{
    public class ChildCommentResponse
    {
        public string Id { get; set; }
        public DateTime CreateAt { get; set; }
        public string ProfileId { get; set; }
        public SimpleProfileResponse Profile { get; set; }
        public ContentResponse Content { get; set; }
        public int ReplyCount { get; set; }

    }

    public class ContentResponse
    {
        public string Text { get; set; }
        public List<TagFacetResponse> TagFacets { get; set; }
    }

    public class TagFacetResponse
    {
        public string Type { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
        public Guid ProfileId { get; set; }
    }

    public class SimpleProfileResponse
    {
        public string Username { get; set; }
        public string AvatarId { get; set; }
    }

}
