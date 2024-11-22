using AutoMapper;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Others;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Profile;
using InfinityNetServer.Services.Post.Application.DTOs.Orther;
using InfinityNetServer.Services.Post.Application.DTOs.Responses;
using InfinityNetServer.Services.Post.Domain.Entities;
using System.Collections.Generic;
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

            CreateMap<Domain.Entities.Post, BuildingBlocks.Application.Protos.PostResponse>()
                .AfterMap((src, dest) =>
                {
                    dest.Id = src.Id.ToString();
                    dest.Content = src.Content.Text;
                    dest.Type = src.Type.ToString();
                    dest.PresentationId = src.PresentationId.ToString();
                    dest.ParentId = src.ParentId.ToString();
                    dest.OwnerId = src.OwnerId.ToString();
                    dest.GroupId = src.GroupId.ToString();
                    dest.FileMetadataId = src.FileMetadataId.ToString();
                    dest.Audience = src.Audience.Type.ToString() ?? string.Empty;
                });

            CreateMap<IEnumerable<Domain.Entities.Post>, BuildingBlocks.Application.Protos.GetByOwnerIdResponse>()
                .AfterMap((src, dest) =>
                {
                    dest.Posts.AddRange(src.Select(post => new BuildingBlocks.Application.Protos.PostResponse
                    {
                        Id = post.Id.ToString(),
                        Content = post.Content.Text,
                        Type = post.Type.ToString(),
                        PresentationId = post.PresentationId.ToString(),
                        ParentId = post.ParentId.ToString(),
                        OwnerId = post.OwnerId.ToString(),
                        GroupId = post.GroupId.ToString(),
                        FileMetadataId = post.FileMetadataId.ToString(),
                        Audience = post.Audience?.Type.ToString() ?? string.Empty
                    }));
                });

                CreateMap<IEnumerable<Domain.Entities.Post>, BuildingBlocks.Application.Protos.GetByParentIdResponse>()
                        .AfterMap((src, dest) =>
                        {
                            dest.Posts.AddRange(src.Select(post => new BuildingBlocks.Application.Protos.PostResponse
                            {
                                Id = post.Id.ToString(),
                                Content = post.Content.Text,
                                Type = post.Type.ToString(),
                                PresentationId = post.PresentationId.ToString(),
                                ParentId = post.ParentId.ToString(),
                                OwnerId = post.OwnerId.ToString(),
                                GroupId = post.GroupId.ToString(),
                                FileMetadataId = post.FileMetadataId.ToString(),
                                Audience = post.Audience?.Type.ToString() ?? string.Empty
                            }));
                        });

                CreateMap<IEnumerable<Domain.Entities.Post>, BuildingBlocks.Application.Protos.GetByGroupIdResponse>()
                        .AfterMap((src, dest) =>
                        {
                            dest.Posts.AddRange(src.Select(post => new BuildingBlocks.Application.Protos.PostResponse
                            {
                                Id = post.Id.ToString(),
                                Content = post.Content.Text,
                                Type = post.Type.ToString(),
                                PresentationId = post.PresentationId.ToString(),
                                ParentId = post.ParentId.ToString(),
                                OwnerId = post.OwnerId.ToString(),
                                GroupId = post.GroupId.ToString(),
                                FileMetadataId = post.FileMetadataId.ToString(),
                                Audience = post.Audience.Type.ToString() ?? string.Empty
                            }));
                        });
    }
}
