using AutoMapper;

namespace InfinityNetServer.Services.Post.Presentation.Mappers;

public class PostMappers : Profile
{
    public PostMappers()
    {

        CreateMap<Domain.Entities.Post, BuildingBlocks.Application.Protos.PreviewPostResponse>()
            .AfterMap((src, dest) =>
            {
                dest.PreviewContent = src.Content.Text.Length > 50
                    ? src.Content.Text[..50]
                    : src.Content.Text;
            });

        CreateMap<Domain.Entities.Post, BuildingBlocks.Application.DTOs.Responses.Post.PreviewPostResponse>()
            .AfterMap((src, dest) =>
            {
                dest.PreviewContent = src.Content.Text.Length > 50
                    ? src.Content.Text[..50]
                    : src.Content.Text;
            });

    }
}
