using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using System;

namespace InfinityNetServer.BuildingBlocks.Presentation.Mappers.Converters
{
    public class TimestampToDateTimeConverter : ITypeConverter<Timestamp, DateTime>
    {
        public DateTime Convert(Timestamp source, DateTime destination, ResolutionContext context)
        {
            // Chuyển đổi Timestamp sang DateTime và đảm bảo là UTC
            DateTime dateTime = source.ToDateTime();

            // Kiểm tra và đảm bảo DateTime là UTC
            return dateTime.Kind == DateTimeKind.Utc ? dateTime : dateTime.ToUniversalTime();
        }
    }

}
