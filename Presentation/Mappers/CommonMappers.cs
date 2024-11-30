using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Others;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.File;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Profile;
using InfinityNetServer.BuildingBlocks.Domain.Enums;
using InfinityNetServer.BuildingBlocks.Presentation.Mappers.Converters;
using InfinityNetServer.Services.Profile.Application.DTOs.Responses;
using System;

namespace InfinityNetServer.BuildingBlocks.Presentation.Mappers;

public class CommonMappers : Profile
{
    public CommonMappers()
    {
        CreateMap<DateTime, Timestamp>().ConvertUsing<DateTimeToTimestampConverter>();

        CreateMap<Timestamp, DateTime>().ConvertUsing<TimestampToDateTimeConverter>();

        CreateMap<Timestamp, DateOnly>().ConvertUsing<TimestampToDateOnlyConverter>();

        CreateMap<DateOnly, Timestamp>().ConvertUsing<DateOnlyToTimestampConverter>();

        CreateMap<Application.Protos.ProfileResponse, BaseProfileResponse>()
            .AfterMap((src, dest) =>
            {
                dest.Type = src.Type.ToString(); // Chuyển enum sang string
                dest.CreatedAt = dest.CreatedAt.ToLocalTime();
                dest.UpdatedAt = dest.UpdatedAt?.ToLocalTime();
                dest.DeletedAt = dest.DeletedAt?.ToLocalTime();
                if (src.AvatarId.Equals(Guid.Empty.ToString()))
                {
                    dest.Avatar = new PhotoMetadataResponse
                    {
                        Id = Guid.Empty,
                        Name = "cover.jpg",
                        Width = 500,
                        Height = 500,
                        Size = 1000,
                        Type = FileMetadataType.Photo.ToString(),
                        Url = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRmCy16nhIbV3pI1qLYHMJKwbH2458oiC9EmA&s",
                        CreatedAt = DateTime.Now
                    };
                }
                else
                {
                    dest.Avatar = new PhotoMetadataResponse
                    {
                        Id = Guid.Parse(src.AvatarId),
                    };
                }

                if (src.CoverId.Equals(Guid.Empty.ToString()))
                {
                    dest.Cover = new PhotoMetadataResponse
                    {
                        Id = Guid.Empty,
                        Name = "cover.jpg",
                        Width = 500,
                        Height = 500,
                        Size = 1000,
                        Type = FileMetadataType.Photo.ToString(),
                        Url = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRmCy16nhIbV3pI1qLYHMJKwbH2458oiC9EmA&s",
                        CreatedAt = DateTime.Now
                    };
                }
                else
                {
                    dest.Cover = new PhotoMetadataResponse
                    {
                        Id = Guid.Parse(src.CoverId),
                    };
                }
            });

        CreateMap<Application.Protos.UserProfileResponse, UserProfileResponse>()
            .AfterMap((src, dest) =>
            {
                dest.Type = src.Type.ToString(); // Chuyển enum sang string
                dest.Gender = src.Gender.ToString(); // Chuyển enum sang string
                dest.Status = src.Status.ToString(); // Chuyển enum sang string
                dest.CreatedAt = dest.CreatedAt.ToLocalTime();
                dest.UpdatedAt = dest.UpdatedAt?.ToLocalTime();
                dest.DeletedAt = dest.DeletedAt?.ToLocalTime();
                if (src.AvatarId.Equals(Guid.Empty.ToString()))
                {
                    dest.Avatar = new PhotoMetadataResponse
                    {
                        Id = Guid.Empty,
                        Name = "cover.jpg",
                        Width = 500,
                        Height = 500,
                        Size = 1000,
                        Type = FileMetadataType.Photo.ToString(),
                        Url = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRmCy16nhIbV3pI1qLYHMJKwbH2458oiC9EmA&s",
                        CreatedAt = DateTime.Now
                    };
                }
                else
                {
                    dest.Avatar = new PhotoMetadataResponse
                    {
                        Id = Guid.Parse(src.AvatarId),
                    };
                }

                if (src.CoverId.Equals(Guid.Empty.ToString()))
                {
                    dest.Cover = new PhotoMetadataResponse
                    {
                        Id = Guid.Empty,
                        Name = "cover.jpg",
                        Width = 500,
                        Height = 500,
                        Size = 1000,
                        Type = FileMetadataType.Photo.ToString(),
                        Url = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRmCy16nhIbV3pI1qLYHMJKwbH2458oiC9EmA&s",
                        CreatedAt = DateTime.Now
                    };
                }
                else
                {
                    dest.Cover = new PhotoMetadataResponse
                    {
                        Id = Guid.Parse(src.CoverId),
                    };
                }
                dest.Name = dest.GenerateName();
            });

        CreateMap<BaseProfileResponse, PreviewProfileResponse>();

        CreateMap<Application.Protos.PhotoMetadataResponse, PhotoMetadataResponse>()
            .AfterMap((src, dest) =>
            {
                dest.Type = src.Type.ToString(); // Chuyển enum sang string
                dest.CreatedAt = dest.CreatedAt.ToLocalTime();
                dest.UpdatedAt = dest.UpdatedAt?.ToLocalTime();
                dest.DeletedAt = dest.DeletedAt?.ToLocalTime();

            });

        CreateMap<Application.Protos.VideoMetadataResponse, VideoMetadataResponse>()
            .AfterMap((src, dest) =>
            {
                dest.Type = src.Type.ToString(); // Chuyển enum sang string
                dest.CreatedAt = dest.CreatedAt.ToLocalTime();
                dest.UpdatedAt = dest.UpdatedAt?.ToLocalTime();
                dest.DeletedAt = dest.DeletedAt?.ToLocalTime();
            });

        CreateMap<Domain.Entities.TagFacet, TagFacet>()
            .AfterMap((src, dest) =>
            {
                dest.Type = src.Type.ToString();
                dest.Profile = new PreviewProfileResponse { Id = src.ProfileId };
                dest.Index = new FacetIndex { Start = src.Start, End = src.End };
            });

        CreateMap<Application.Protos.PreviewPostResponse, Application.DTOs.Responses.Post.PreviewPostResponse>()
            .AfterMap((src, dest) =>
            {
                if (src.FileMetadataId.Equals(Guid.Empty)) dest.FileMetadataId = null;
            });

        CreateMap<Application.Protos.PreviewCommentResponse, Application.DTOs.Responses.Comment.PreviewCommentResponse>()
            .AfterMap((src, dest) =>
            {
                if (src.FileMetadataId.Equals(Guid.Empty)) dest.FileMetadataId = null;
            });

        CreateMap<Application.Protos.TagFacetResponse, Application.DTOs.Responses.Comment.CommentPreviewResponse.TagFacetResponse>()
        .AfterMap((src, dest) =>
        {
            dest.Type = src.Type.ToString();
        });

        CreateMap<Application.Protos.ContentResponse, Application.DTOs.Responses.Comment.CommentPreviewResponse.ContentResponse>();

        CreateMap<Application.Protos.CommentPreviewResponse, Application.DTOs.Responses.Comment.CommentPreviewResponse>();

        CreateMap<Application.Protos.AccountWithDefaultProfile, AccountWithDefaultProfile>();

        CreateMap<Application.Protos.GroupMemberWithGroup, GroupMemberWithGroup>();

        CreateMap<Application.Protos.ProfileIdWithName, ProfileIdWithName>();

        CreateMap<Application.Protos.PreviewFileMetadata, PreviewFileMetadata>();

        CreateMap<Application.Protos.ProfileIdWithMutualCount, ProfileIdWithMutualCount>().ReverseMap();

        CreateMap<UserProfileResponse, FriendSuggestionResponse>()
            .ForMember(dest => dest.Status, opt => opt.Ignore());
           
        CreateMap<UserProfileResponse, BlockeeResponse>();
    }
}
