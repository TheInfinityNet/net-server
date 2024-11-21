using AutoMapper;
using Google.Protobuf.WellKnownTypes;
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
        // Mapping từ Entity sang gRPC Response
        CreateMap<Domain.Entities.Comment, BuildingBlocks.Application.Protos.CommentPreviewResponse>()
            .ForMember(dest => dest.CreateAt, opt =>
                opt.MapFrom(src => Timestamp.FromDateTime(src.CreatedAt.ToUniversalTime())))
            .ForMember(dest => dest.Content, opt =>
                opt.MapFrom(src => src.Content.Text))
            .ForMember(dest => dest.ProfileId, opt =>
                opt.MapFrom(src => src.ProfileId.ToString()))
            .ForMember(dest => dest.CommentId, opt =>
                opt.MapFrom(src => src.Id.ToString()))
            .ForMember(dest => dest.ReplyCount, opt =>
                opt.MapFrom(src => src.RepliesComments.Count));

        // Mapping từ DTO sang Protobuf
        CreateMap<InfinityNetServer.Services.Comment.Application.DTOs.Responses.CommentPreviewResponse, BuildingBlocks.Application.Protos.CommentPreviewResponse>()
            .ForMember(dest => dest.CreateAt, opt =>
                opt.MapFrom(src => Timestamp.FromDateTime(src.CreateAt.ToUniversalTime())));
    }
}
