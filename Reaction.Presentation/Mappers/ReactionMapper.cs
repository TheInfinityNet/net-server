using AutoMapper;
using InfinityNetServer.Services.Reaction.Domain.Entities;
using InfinityNetServer.Services.Reaction.Application.DTOs.Requests;

namespace InfinityNetServer.Services.Reaction.Presentation.Mappers
{
    public class ReactionMapper : Profile
    {
        public ReactionMapper()
        {
            CreateMap<PostReaction, AddPostReactionRequest>().ReverseMap();
            CreateMap<CommentReaction, AddCommentReactionRequest>().ReverseMap();
        }
    }
}