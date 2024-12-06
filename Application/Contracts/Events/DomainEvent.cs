using InfinityNetServer.BuildingBlocks.Application.Contracts.Messages;
using InfinityNetServer.BuildingBlocks.Domain.Enums;
using System;

namespace InfinityNetServer.BuildingBlocks.Application.Contracts.Events
{
    public static class DomainEvent
    {

        public sealed record PhotoMetadataEvent : IMetadataCommand
        {

            public Guid Id { get; set; } = Guid.NewGuid();

            public Guid TempId { get; set; }

            public Guid FileMetadataId { get; set; }

            public Guid OwnerId { get; set; }

            public FileOwnerType OwnerType { get; set; }

            public DateTime UpdatedAt { get; set; } = DateTime.Now;

            public Guid UpdatedBy { get; set; }

        }

        public sealed record VideoMetadataEvent : IMetadataCommand
        {

            public Guid Id { get; set; } = Guid.NewGuid();

            public Guid TempId { get; set; }

            public Guid FileMetadataId { get; set; }

            public Guid OwnerId { get; set; }

            public FileOwnerType OwnerType { get; set; }

            public DateTime UpdatedAt { get; set; } = DateTime.Now;

            public Guid UpdatedBy { get; set; }

        }

    }
}
