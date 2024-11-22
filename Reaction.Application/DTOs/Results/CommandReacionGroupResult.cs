using InfinityNetServer.Services.Reaction.Domain.Enums;
using System;

namespace InfinityNetServer.Application.DTOs.Results
{
    public class CommandReacionGroupResult
    {
        public Guid CommentId { get; set; }
        public ReactionType Type { get; set; }
        public long Count { get; set; }
    }
}