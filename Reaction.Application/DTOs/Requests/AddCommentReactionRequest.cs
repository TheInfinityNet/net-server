using InfinityNetServer.Services.Reaction.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace InfinityNetServer.Application.Post.Presentation.DTOs.Requests
{
    public class AddCommentReactionRequest
    {
        public Guid ProfileId { get; set; } // Link to Profile service

        public ReactionType Type { get; set; }

        public Guid CommentId { get; set; }
    }
}