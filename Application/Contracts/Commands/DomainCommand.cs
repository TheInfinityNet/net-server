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

        public sealed record CreatePostReactionNotificationCommand : INotificationCommand
        {
            public Guid Id { get; set; } = Guid.NewGuid();

            public NotificationType Type { get; set; }

            public Guid TargetProfileId { get; set; }

            public string TriggeredBy { get; set; }

            public Guid PostReactionId { get; set; }

            public Guid PostId { get; set; }

            public ReactionType ReactionType { get; set; }

            public DateTime CreatedAt { get; set; }
        }

        public sealed record CreateCommentReactionNotificationCommand : INotificationCommand
        {

            public Guid Id { get; set; } = Guid.NewGuid();

            public NotificationType Type { get; set; }

            public Guid TargetProfileId { get; set; }

            public string TriggeredBy { get; set; }

            public Guid CommentReactionId { get; set; }

            public Guid CommentId { get; set; }

            public ReactionType ReactionType { get; set; }

            public DateTime CreatedAt { get; set; }

        }

        public sealed record PushPostToTimelineCommand : IUserTimelineCommand
        {

            public Guid Id { get; set; } = Guid.NewGuid();

            public Guid ProfileId { get; set; }

            public Guid PostId { get; set; }

            public Guid? ParentPostId { get; set; }

            public DateTime CreatedAt { get; set; }

        }

        public sealed record PullPostFromTimelineCommand : IUserTimelineCommand
        {

            public Guid Id { get; set; } = Guid.NewGuid();

            public Guid ProfileId { get; set; }

            public Guid PostId { get; set; }

        }

        public sealed record SendMailWithCodeCommand : IMailCommand
        {

            public string ToMail { get; set; }

            public VerificationType Type { get; set; } = VerificationType.VerifyByCode;

            public string AcceptLanguage { get; set; }

            public string Code { get; set; }

            public Guid Id { get; set; }

            public DateTime CreatedAt { get; set; }

            public Guid? CreatedBy { get; set; }

        }

        public sealed record CreateUserProfileCommand : IProfileCommand
        {

            public Guid Id { get; set; } = Guid.NewGuid();

            public Guid ProfileId { get; set; }

            public DateTime CreatedAt { get; set; } = DateTime.Now;

            public Guid AccountId { get; set; }

            public string FirstName { get; set; }

            public string MiddleName { get; set; }

            public string LastName { get; set; }

            public string MobileNumber { get; set; }

            public DateOnly Birthdate { get; set; }

            public Gender Gender { get; set; }

        }

        public sealed record ActiveProfileCommand : IProfileCommand
        {

            public Guid Id { get; set; } = Guid.NewGuid();

            public Guid ProfileId { get; set; }

            public DateTime CreatedAt { get; set; } = DateTime.Now;

        }

    }
}
