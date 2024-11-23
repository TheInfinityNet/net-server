namespace InfinityNetServer.BuildingBlocks.Domain.Enums
{
    public enum NotificationType
    {

        TaggedInPost,

        FriendInvitation,

        FriendInvitationAccepted, // trigger trong relationship

        NewProfileFollower,

        NewFollowerPost, //

        NewGroupPost,

        PostReaction, // trigger trong reactions

        CommentReaction, // trigger trong reactions

        CommentToPost, 

        ReplyToComment,

        TaggedInComment,

        Miscellaneous //

    }
}
