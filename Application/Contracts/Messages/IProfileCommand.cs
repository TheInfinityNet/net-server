using System;

namespace InfinityNetServer.BuildingBlocks.Application.Contracts.Messages
{
    public interface IProfileCommand : IMessage
    {

        public Guid ProfileId { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}
