using AutoMapper;
using Google.Protobuf.WellKnownTypes;
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

        CreateMap<Application.Protos.UserProfileResponse, Application.DTOs.Responses.UserProfileResponse>()
            .AfterMap((src, dest) =>
            {
                dest.Gender = src.Gender.ToString(); // Chuyển enum sang string
                dest.Status = src.Status.ToString(); // Chuyển enum sang string
                dest.CreatedAt = dest.CreatedAt.ToLocalTime();
                dest.UpdatedAt = dest.UpdatedAt?.ToLocalTime();
                dest.DeletedAt = dest.DeletedAt?.ToLocalTime();
            });

        CreateMap<Application.Protos.AccountWithDefaultProfile, Application.DTOs.Others.AccountWithDefaultProfile>();

        CreateMap<Application.Protos.GroupMemberWithGroup, Application.DTOs.Others.GroupMemberWithGroup>();
    }
}
