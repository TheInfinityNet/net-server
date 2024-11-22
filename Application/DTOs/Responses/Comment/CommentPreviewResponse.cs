using System;
using System.Collections.Generic;

namespace InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Comment
{
    public class CommentPreviewResponse
    {
        public Guid CommentId { get; set; }
        public Guid ProfileId { get; set; }
        public ContentResponse Content { get; set; }
        public int ReplyCount { get; set; }
        public DateTime CreateAt { get; set; }

        public CommentPreviewResponse() { }
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

        //public CommentPreviewResponse(Domain.Entities.Comment comment)
        //{
        //    CommentId = comment.Id;
        //    ProfileId = comment.ProfileId;
        //    Content = comment.Content;
        //    ReplyCount = comment.RepliesComments.Count;
        //    CreateAt = comment.CreatedAt;
        //}
    }
}
