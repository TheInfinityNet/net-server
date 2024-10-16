using AutoMapper;
using System;
using Google.Protobuf.WellKnownTypes;

namespace InfinityNetServer.BuildingBlocks.Presentation.Mappers.Converters
{
    public class DateOnlyToTimestampConverter : ITypeConverter<DateOnly, Timestamp>
    {
        public Timestamp Convert(DateOnly source, Timestamp destination, ResolutionContext context)
        {
            // Chuyển DateOnly thành DateTime với thời gian là 00:00:00 UTC
            DateTime dateTime = source.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
            return Timestamp.FromDateTime(dateTime);
        }
    }
}
