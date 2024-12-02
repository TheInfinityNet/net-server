using InfinityNetServer.BuildingBlocks.Application.DTOs.Others;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.File;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Profile;
using System;
using System.Collections.Generic;

namespace InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Comment
{
    public class CommentResponse
    {

        public Guid Id { get; set; }

        public PreviewProfileResponse Owner { get; set; }

        public Guid OwnerId { get; set; }

        public Guid PostId { get; set; }

        public CommentContent Content { get; set; }

        public PhotoMetadataResponse Photo { get; set; }

        public int? ReplyCount { get; set; }

        public string Reaction { get; set; }

        public IDictionary<string, int> ReactionCounts { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}
