using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using System;

namespace InfinityNetServer.BuildingBlocks.Presentation.Mappers.Converters
{
    public class TimestampToDateOnlyConverter : ITypeConverter<Timestamp, DateOnly>
    {
        public DateOnly Convert(Timestamp source, DateOnly destination, ResolutionContext context)
        {
            // Chuyển Timestamp thành DateTime và sau đó thành DateOnly
            DateTime dateTime = source.ToDateTime();
            return DateOnly.FromDateTime(dateTime.ToUniversalTime());
        }
    }

}
