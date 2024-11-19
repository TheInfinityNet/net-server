using InfinityNetServer.BuildingBlocks.Application.Contracts.Messages;
using InfinityNetServer.BuildingBlocks.Domain.Enums;
using System;

namespace InfinityNetServer.BuildingBlocks.Application.Contracts.Commands
{
    public static class DomainCommand
    {

        public sealed record CreatePostNotificationCommand : INotificationCommand
        {

            public Guid Id { get; set; } = Guid.NewGuid();

            public NotificationType Type { get; set; }

            public Guid TargetProfileId { get; set; }

            public string TriggeredBy { get; set; }

            public Guid PostId { get; set; }

            public DateTime CreatedAt { get; set; }

        }

        public sealed record CreateCommentNotificationCommand : INotificationCommand
        {

            public Guid Id { get; set; } = Guid.NewGuid();

            public NotificationType Type { get; set; }

            public Guid TargetProfileId { get; set; }

            public string TriggeredBy { get; set; }

            public Guid CommentId { get; set; }

            public DateTime CreatedAt { get; set; }

        }

        public sealed record CreateFriendshipNotificationCommand : INotificationCommand
        {

            public Guid Id { get; set; } = Guid.NewGuid();

            public NotificationType Type { get; set; }

            public Guid TargetProfileId { get; set; }

            public string TriggeredBy { get; set; }

            public Guid FriendshipId { get; set; }

            public DateTime CreatedAt { get; set; }

        }

        public sealed record CreateProfileFollowNotificationCommand : INotificationCommand
        {

            public Guid Id { get; set; } = Guid.NewGuid();

            public NotificationType Type { get; set; }

            public Guid TargetProfileId { get; set; }

            public string TriggeredBy { get; set; }

            public Guid ProfileFollowId { get; set; }

            public DateTime CreatedAt { get; set; }

        }

        public sealed record UpdateUserTimelineCommand : IUserTimelineCommand
        {

            public Guid ProfileId { get; set; }

            public Guid Id { get; set; } = Guid.NewGuid();

            public Guid PostId { get; set; }

            public DateTime CreatedAt { get; set; }

        }

    }
}
