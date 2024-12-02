using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Profile;
using InfinityNetServer.Services.Reaction.Application.DTOs.Requests;
using InfinityNetServer.Services.Reaction.Application.DTOs.Responses;
using InfinityNetServer.Services.Reaction.Domain.Entities;

namespace InfinityNetServer.Services.Reaction.Presentation.Mappers
{
    public class ReactionMappers : AutoMapper.Profile
    {
        public ReactionMappers()
        {
            CreateMap<PostReaction, AddPostReactionRequest>().ReverseMap();
            CreateMap<CommentReaction, AddCommentReactionRequest>().ReverseMap();

            // Entity -> DTO
            CreateMap<PostReaction, BuildingBlocks.Application.Protos.PreviewReaction>()
                .AfterMap((src, dest) => dest.OwnerId = src.PostId.ToString());

            CreateMap<CommentReaction, BuildingBlocks.Application.Protos.PreviewReaction>()
                .AfterMap((src, dest) => dest.OwnerId = src.CommentId.ToString());

            CreateMap<PostReaction, ReactionBaseResponse>()
                .AfterMap((src, dest) => {
                    dest.Profile = new PreviewProfileResponse { Id = src.ProfileId };
                    dest.Reaction = src.Type.ToString();
                });

            CreateMap<PostReaction, PostReactionResponse>()
                .AfterMap((src, dest) => {
                    dest.Profile = new PreviewProfileResponse { Id = src.ProfileId };
                    dest.Reaction = src.Type.ToString();
                });

            CreateMap<CommentReaction, CommentReactionResponse>()
                .AfterMap((src, dest) => {
                    dest.Profile = new PreviewProfileResponse { Id = src.ProfileId };
                    dest.Reaction = src.Type.ToString();
                });
        }
    }
}