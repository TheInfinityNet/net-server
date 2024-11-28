using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using InfinityNetServer.BuildingBlocks.Domain.Enums;

namespace InfinityNetServer.Services.Reaction.Application.DTOs.Requests
{
    public class AddCommentReactionRequest
    {
        public Guid ProfileId { get; set; } // Link to Profile service

        public ReactionType Type { get; set; }

        public Guid CommentId { get; set; }
    }
}