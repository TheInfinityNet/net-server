using InfinityNetServer.BuildingBlocks.Application.DTOs.Others;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Profile;
using InfinityNetServer.BuildingBlocks.Domain.Enums;
using InfinityNetServer.Services.Post.Application.DTOs.Orther;
using InfinityNetServer.Services.Post.Application.DTOs.Requests;
using InfinityNetServer.Services.Post.Application.DTOs.Responses;
using InfinityNetServer.Services.Post.Domain.Entities;
using InfinityNetServer.Services.Post.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InfinityNetServer.Services.Post.Presentation.Mappers;

public class PostMappers : AutoMapper.Profile
{
    public PostMappers()
    {
        // Entity -> DTO
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
            .AfterMap((src, dest) =>
            {
                dest.Owner = new PreviewProfileResponse { Id = src.OwnerId };
            });

        CreateMap<Domain.Entities.PostContent, Application.DTOs.Orther.PostContent>();

        CreateMap<BuildingBlocks.Domain.Entities.HashtagFacet, HashTagFacet>()
            .AfterMap((src, dest) =>
            {
                dest.Index = new FacetIndex { Start = src.Start, End = src.End };
            });

        CreateMap<PostAudience, BasePostAudience>()
            .AfterMap((src, dest) =>
            {
                dest.Type = src.Type.ToString();

                switch (src.Type) {
                    case PostAudienceType.Include:
                        dest.Include = src.Includes.Select(i => new PreviewProfileResponse { Id = i.ProfileId }).ToList();
                        break;

                    case PostAudienceType.Exclude:
                        dest.Exclude =  src.Excludes.Select(i => new PreviewProfileResponse { Id = i.ProfileId }).ToList();
                        break;

                    case PostAudienceType.Custom:
                        dest.Include = src.Includes.Select(i => new PreviewProfileResponse { Id = i.ProfileId }).ToList();
                        dest.Exclude = src.Excludes.Select(i => new PreviewProfileResponse { Id = i.ProfileId }).ToList();
                        break;
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

        // DTO -> Entity
        CreateMap<BaseFacet, BuildingBlocks.Domain.Entities.BaseFacet>()
            .AfterMap((src, dest) =>
            {
                dest.Start = src.Index.Start;
                dest.End = src.Index.End;
                dest.Type = Enum.Parse<FacetType>(src.Type);
            });

        CreateMap<HashTagFacet, BuildingBlocks.Domain.Entities.HashtagFacet>();

        CreateMap<Application.DTOs.Orther.PostContent, Domain.Entities.PostContent>();

        CreateMap<BasePostAudience, PostAudience>()
            .AfterMap((src, dest) =>
            {
                dest.Type = Enum.Parse<PostAudienceType>(src.Type);

                switch (dest.Type) {
                    case PostAudienceType.Include:
                        dest.Includes = src.Include.Select(i =>
                        new Domain.Entities.PostAudienceInclude { ProfileId = i.Id }).ToList();
                        break;

                    case PostAudienceType.Exclude:
                        dest.Excludes = src.Exclude.Select(i =>
                        new PostAudienceExclude { ProfileId = i.Id }).ToList();
                        break;

                    case PostAudienceType.Custom:
                        dest.Includes = src.Include.Select(i =>
                        new PostAudienceInclude { ProfileId = i.Id }).ToList();

                        dest.Excludes = src.Exclude.Select(i =>
                        new PostAudienceExclude { ProfileId = i.Id }).ToList();
                        break;
                }
            });

        CreateMap<CreatePostBaseRequest, Domain.Entities.Post>();

        CreateMap<CreateMediaPostRequest, Domain.Entities.Post>()
            .AfterMap((src, dest) =>
            {
                PostType type = Enum.Parse<PostType>(src.Type);
                switch (type)
                {
                    case PostType.Photo:
                        dest.FileMetadataId = Guid.Parse(src.PhotoId);
                        break;
                    case PostType.Video:
                        dest.FileMetadataId = Guid.Parse(src.VideoId);
                        break;
                }
            });

        CreateMap<CreateSharePostRequest, Domain.Entities.Post>()
            .AfterMap((src, dest) =>
            {
                dest.ParentId = Guid.Parse(src.ShareId);
            });
    }
}
