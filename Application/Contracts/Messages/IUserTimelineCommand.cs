using System;

namespace InfinityNetServer.BuildingBlocks.Application.Contracts.Messages
{
    public interface IUserTimelineCommand : IMessage
    {

        public Guid ProfileId { get; set; }

        public Guid PostId { get; set; }

    }
}
