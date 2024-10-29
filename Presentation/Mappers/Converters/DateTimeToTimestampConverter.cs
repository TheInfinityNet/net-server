using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using System;

namespace InfinityNetServer.BuildingBlocks.Presentation.Mappers.Converters
{
    public class DateTimeToTimestampConverter : ITypeConverter<DateTime, Timestamp>
    {
        public Timestamp Convert(DateTime source, Timestamp destination, ResolutionContext context)
        {
            // Đảm bảo DateTime được chuyển đổi sang UTC
            DateTime utcDateTime = source.Kind == DateTimeKind.Utc ? source : source.ToUniversalTime();
            return Timestamp.FromDateTime(utcDateTime);
        }
    }

}
