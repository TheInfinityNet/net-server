using InfinityNetServer.BuildingBlocks.Application.Contracts.Messages;
using InfinityNetServer.BuildingBlocks.Domain.Enums;
using System;

namespace InfinityNetServer.BuildingBlocks.Application.Contracts.Commands
{
    public static class DomainCommand
    {

        public sealed record PostNotificationCommand : INotificationCommand
        {

            public Guid Id { get; set; }

            public NotificationType Type { get; set; }

            public Guid RelatedProfileId { get; set; }

            public string TriggeredBy { get; set; }

            public Guid PostId { get; set; }

            public DateTime CreatedAt { get; set; }

        }

        public sealed record CommentNotificationCommand : INotificationCommand
        {

            public Guid Id { get; set; }

            public NotificationType Type { get; set; }

            public Guid RelatedProfileId { get; set; }

            public string TriggeredBy { get; set; }

            public Guid CommentId { get; set; }

            public DateTime CreatedAt { get; set; }

        }

        public sealed record FriendshipNotificationCommand : INotificationCommand
        {

            public Guid Id { get; set; }

            public NotificationType Type { get; set; }

            public Guid RelatedProfileId { get; set; }

            public string TriggeredBy { get; set; }

            public Guid FriendshipId { get; set; }

            public DateTime CreatedAt { get; set; }

        }

        public sealed record ProfileFollowNotificationCommand : INotificationCommand
        {

            public Guid Id { get; set; }

            public NotificationType Type { get; set; }

            public Guid RelatedProfileId { get; set; }

            public string TriggeredBy { get; set; }

            public Guid ProfileFollowId { get; set; }

            public DateTime CreatedAt { get; set; }

        }

    }
}
