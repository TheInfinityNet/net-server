using System;

namespace InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.File
{
    public abstract record BaseMetadataResponse
    {

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public long Size { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        public string Url { get; set; }

    }
}
