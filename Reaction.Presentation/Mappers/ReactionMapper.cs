using AutoMapper;
using InfinityNetServer.Services.Reaction.Domain.Entities;
using InfinityNetServer.Application.Post.Presentation.DTOs.Requests;

namespace InfinityNetServer.Presentation.Mappers
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