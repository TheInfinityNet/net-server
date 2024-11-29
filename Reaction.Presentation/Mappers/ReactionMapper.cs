using InfinityNetServer.Services.Reaction.Application.DTOs.Requests;
using InfinityNetServer.Services.Reaction.Domain.Entities;

namespace InfinityNetServer.Services.Reaction.Presentation.Mappers
{
    public class ReactionMapper : AutoMapper.Profile
    {
        public ReactionMapper()
        {
            CreateMap<PostReaction, AddPostReactionRequest>().ReverseMap();
            CreateMap<CommentReaction, AddCommentReactionRequest>().ReverseMap();
        }
    }
}