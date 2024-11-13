using AutoMapper;

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

    }
}
