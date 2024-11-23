using InfinityNetServer.BuildingBlocks.Domain.Enums;
using System;

namespace InfinityNetServer.BuildingBlocks.Application.Contracts.Messages
{
    public interface IMetadataEvent : IMessage
    {

        public Guid FileMetadataId { get; set; }

        public Guid OwnerId { get; set; }

        public FileOwnerType OwnerType { get; set; }

        public DateTime UpdatedAt { get; set; }

        public Guid UpdatedBy { get; set; }

    }
}
