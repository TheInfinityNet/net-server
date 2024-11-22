using AutoMapper;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Others;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Profile;
using InfinityNetServer.Services.Post.Application.DTOs.Orther;
using InfinityNetServer.Services.Post.Application.DTOs.Responses;
using InfinityNetServer.Services.Post.Domain.Entities;
using System.Linq;

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
                dest.Owner = new PreviewProfileResponse { Id = src.OwnerId };
            });

        CreateMap<PostContent, TextContent>();

        CreateMap<BuildingBlocks.Domain.Entities.HashtagFacet, HashTagFacet>()
            .AfterMap((src, dest) => {
                dest.Index = new FacetIndex { Start = src.Start, End = src.End };
            });

        CreateMap<PostAudience, BasePostAudience>()
            .AfterMap((src, dest) =>
            {
                dest.Type = src.Type.ToString();
            });

        CreateMap<PostAudience, Application.DTOs.Orther.PostAudienceInclude>()
            .AfterMap((src, dest) =>
            {
                if (src.Type != Domain.Enums.PostAudienceType.Include)
                    dest = null;

                else dest.Include = src.Includes.Select(i => new BaseProfileResponse { Id = i.ProfileId }).ToList();
            });

        CreateMap<PostAudience, Application.DTOs.Orther.PostAudienceExclude>()
            .AfterMap((src, dest) =>
            {
                if (src.Type != Domain.Enums.PostAudienceType.Exclude)
                    dest = null;

                else dest.Exclude = src.Excludes.Select(i => new BaseProfileResponse { Id = i.ProfileId }).ToList();
            });

        CreateMap<PostAudience, PostAudienceCustom>()
            .AfterMap((src, dest) =>
            {
                if (src.Type != Domain.Enums.PostAudienceType.Custom)
                    dest = null;

                else
                {
                    dest.Include = src.Includes.Select(i => new BaseProfileResponse { Id = i.ProfileId }).ToList();
                    dest.Exclude = src.Excludes.Select(i => new BaseProfileResponse { Id = i.ProfileId }).ToList();
                }
            });

    }
}
