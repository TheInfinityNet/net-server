using System;

namespace InfinityNetServer.Services.Reaction.Application.DTOs.Responses
{
    public sealed record PostReactionResponse : ReactionBaseResponse
    {
        public Guid PostId { get; set; }

    }
}
