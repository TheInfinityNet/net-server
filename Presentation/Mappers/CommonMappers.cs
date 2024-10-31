using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.File;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Profile;
using InfinityNetServer.BuildingBlocks.Domain.Enums;
using InfinityNetServer.BuildingBlocks.Presentation.Mappers.Converters;
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

        CreateMap<Application.Protos.UserProfileResponse, UserProfileResponse>()
            .AfterMap((src, dest) =>
            {
                dest.Type = src.Type.ToString(); // Chuyển enum sang string
                dest.Gender = src.Gender.ToString(); // Chuyển enum sang string
                dest.Status = src.Status.ToString(); // Chuyển enum sang string
                dest.CreatedAt = dest.CreatedAt.ToLocalTime();
                dest.UpdatedAt = dest.UpdatedAt?.ToLocalTime();
                dest.DeletedAt = dest.DeletedAt?.ToLocalTime();
                dest.Cover = new PhotoMetadataResponse
                {
                    Id = Guid.NewGuid().ToString(),
                    Filename = "cover.jpg",
                    Width = 500,
                    Height = 500,
                    Size = 1000,
                    Type = FileMetadataType.Photo.ToString(),
                    Url = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRmCy16nhIbV3pI1qLYHMJKwbH2458oiC9EmA&s",
                    CreatedAt = DateTime.Now
                };
                dest.Avatar = new PhotoMetadataResponse
                {
                    Id = Guid.NewGuid().ToString(),
                    Filename = "cover.jpg",
                    Width = 500,
                    Height = 500,
                    Size = 1000,
                    Type = FileMetadataType.Photo.ToString(),
                    Url = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRmCy16nhIbV3pI1qLYHMJKwbH2458oiC9EmA&s",
                    CreatedAt = DateTime.Now
                };
                dest.Name = dest.GenerateName();
            });

        CreateMap<Application.Protos.AccountWithDefaultProfile, Application.DTOs.Others.AccountWithDefaultProfile>();

        CreateMap<Application.Protos.GroupMemberWithGroup, Application.DTOs.Others.GroupMemberWithGroup>();
    }
}
