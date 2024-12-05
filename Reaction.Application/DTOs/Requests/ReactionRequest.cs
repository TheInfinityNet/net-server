using InfinityNetServer.BuildingBlocks.Domain.Enums;

namespace InfinityNetServer.Services.Reaction.Application.DTOs.Requests
{
    public class ReactionRequest
    {

        public string Reaction { get; set; } = ReactionType.Like.ToString();

    }

}
