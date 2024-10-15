using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using System;

namespace InfinityNetServer.BuildingBlocks.Presentation.Mappers;

public class CommonMappers : Profile
{
    public CommonMappers()
    {
        CreateMap<DateTime, Timestamp>().ConvertUsing<DateTimeToTimestampConverter>();

        CreateMap<Timestamp, DateTime>().ConvertUsing<TimestampToDateTimeConverter>();

        CreateMap<Application.Protos.UserProfileResponse, Application.DTOs.Responses.UserProfileResponse>()
            .AfterMap((src, dest) =>
            {
                dest.Gender = src.Gender.ToString(); // Chuyển enum sang string
            }); ;
    }
}
