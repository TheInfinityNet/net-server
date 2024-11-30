using AutoMapper;
using InfinityNetServer.Services.Comment.Application.DTOs.Responses;

namespace InfinityNetServer.Services.Comment.Presentation.Mappers;

public class CommentMappers : Profile
{
    public CommentMappers()
    {
        CreateMap<BuildingBlocks.Application.DTOs.Responses.Comment.CommentPreviewResponse.TagFacetResponse, BuildingBlocks.Application.Protos.TagFacetResponse>();

        CreateMap<BuildingBlocks.Application.DTOs.Responses.Comment.CommentPreviewResponse.ContentResponse, BuildingBlocks.Application.Protos.ContentResponse>();

        CreateMap<BuildingBlocks.Application.DTOs.Responses.Comment.CommentPreviewResponse, BuildingBlocks.Application.Protos.CommentPreviewResponse>();

        CreateMap<InfinityNetServer.Services.Comment.Domain.Entities.Comment, CommentCountResponse>()
            .ForMember(dest => dest.PostId, opt => opt.MapFrom(src => src.PostId)) // Lấy ID bài viết từ Comment
            .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(src => 1)) // Đếm mỗi comment là 1
            .AfterMap((src, dest) =>
            {

            });
    }
}
