using InfinityNetServer.BuildingBlocks.Domain.Enums;
using System;

namespace InfinityNetServer.BuildingBlocks.Application.Contracts.Messages
{
    public interface IMetadataEvent : IMessage
    {

        public string Name { get; set; }

        public Guid OwnerId { get; set; }

        public FileOwnerType OwnerType { get; set; }

        public FileMetadataType Type { get; set; }

    }
}
