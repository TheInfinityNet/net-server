using InfinityNetServer.BuildingBlocks.Domain.Enums;

namespace InfinityNetServer.Services.Reaction.Application.DTOs.Requests
{
    public class CreateReactionRequest
    {

        public string Type { get; set; } = ReactionType.Like.ToString();

    }

}
