using AutoMapper;
using InfinityNetServer.BuildingBlocks.Application.Protos;
using InfinityNetServer.Services.Comment.Application.DTOs.Responses;
using System.Collections.Generic;

namespace InfinityNetServer.Services.Comment.Presentation.Mappers;

public class CommentMappers : Profile
{
    public CommentMappers()
    {

        CreateMap<Domain.Entities.Comment, BuildingBlocks.Application.Protos.PreviewCommentResponse>()
            .AfterMap((src, dest) =>
            {
                dest.PreviewContent = src.Content.Text[..50];
            });

        CreateMap<Domain.Entities.Comment, BuildingBlocks.Application.DTOs.Responses.Comment.PreviewCommentResponse>()
            .AfterMap((src, dest) =>
            {
                dest.PreviewContent = src.Content.Text[..50];
            });
        CreateMap<Domain.Entities.Comment, CommentPreviewResponse>()
            .ForMember(dest => dest.CommentId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.ProfileId, opt => opt.MapFrom(src => src.ProfileId))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content.Text))
            .ForMember(dest => dest.ReplyCount, opt => opt.MapFrom(src => src.RepliesComments.Count))
            .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => src.CreatedAt));
    }
}
