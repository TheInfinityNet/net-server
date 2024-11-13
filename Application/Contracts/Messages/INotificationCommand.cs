using InfinityNetServer.BuildingBlocks.Domain.Enums;
using System;

namespace InfinityNetServer.BuildingBlocks.Application.Contracts.Messages
{
    public interface INotificationCommand : IMessage
    {

        public NotificationType Type { get; set; }

        public Guid RelatedProfileId { get; set; }

        public string TriggeredBy { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}
