﻿using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Comment;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.File;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Profile;
using InfinityNetServer.BuildingBlocks.Domain.Enums;
using InfinityNetServer.Services.Post.Application.DTOs.Orther;
using System;
using System.Collections.Generic;

namespace InfinityNetServer.Services.Post.Application.DTOs.Responses
{
    public record BasePostResponse
    {

        public Guid Id { get; set; }

        public string Type { get; set; }

        public PostContent Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Reaction { get; set; }

        public IDictionary<string, int> ReactionCounts { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        public PreviewProfileResponse Owner { get; set; }

        public BasePostAudience Audience { get; set; }

        public PhotoMetadataResponse Photo { get; set; }

        public VideoMetadataResponse Video { get; set; }

        public IList<BasePostResponse> Aggregates { get; set; }

        public BasePostResponse Share { get; set; }

        public IList<CommentResponse> PopularComments { get; set; }   

    }
}
