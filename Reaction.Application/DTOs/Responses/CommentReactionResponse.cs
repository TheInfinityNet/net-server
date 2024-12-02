using System;

namespace InfinityNetServer.Services.Reaction.Application.DTOs.Responses
{
    public sealed record CommentReactionResponse : ReactionBaseResponse
    {
        public Guid CommentId { get; set; }

    }
}
