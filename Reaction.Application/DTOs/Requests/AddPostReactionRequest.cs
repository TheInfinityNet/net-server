using InfinityNetServer.BuildingBlocks.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using InfinityNetServer.BuildingBlocks.Domain.Enums;

namespace InfinityNetServer.Services.Reaction.Application.DTOs.Requests
{
    public class AddPostReactionRequest
    {
        public Guid ProfileId { get; set; } // Link to Profile service
        public ReactionType Type { get; set; }
        public Guid PostId { get; set; }
    }

}
