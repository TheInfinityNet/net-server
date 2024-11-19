using AutoMapper;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Profile;
using InfinityNetServer.Services.Post.Application.DTOs.Orther;
using InfinityNetServer.Services.Post.Application.DTOs.Responses;
using InfinityNetServer.Services.Post.Domain.Entities;

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

        CreateMap<Domain.Entities.Post, BasePostResponse>()
            .AfterMap((src, dest) => { 
                dest.Type = src.Type.ToString(); 
                dest.Owner = new BaseProfileResponse { Id = src.OwnerId };
            });

        CreateMap<PostContent, TextContent>();

        CreateMap<BuildingBlocks.Domain.Entities.TagFacet, TagFacet>()
            .AfterMap((src, dest) => { 
                dest.Type = src.Type.ToString(); 
                dest.Profile = new PreviewProfileResponse { Id = src.ProfileId };
                dest.Index = new FacetIndex { Start = src.Start, End = src.End };
            });

        CreateMap<BuildingBlocks.Domain.Entities.HashtagFacet, HashTagFacet>()
            .AfterMap((src, dest) => {
                dest.Type = src.Type.ToString();
                dest.Index = new FacetIndex { Start = src.Start, End = src.End };
            });

    }
}
