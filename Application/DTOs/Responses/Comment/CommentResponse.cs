using InfinityNetServer.BuildingBlocks.Application.DTOs.Others;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.File;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Profile;
using System;

namespace InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Comment
{
    public class CommentResponse
    {

        public Guid Id { get; set; }

        public PreviewProfileResponse Profile { get; set; }

        public CommentContent Content { get; set; }

        public PhotoMetadataResponse Photo { get; set; }

        public int? ReplyCount { get; set; }

        public string Reaction { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}
