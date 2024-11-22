using InfinityNetServer.BuildingBlocks.Application.Exceptions;

namespace InfinityNetServer.Services.Notification.Application.Exceptions
{
    public sealed record NotificationError : BaseError
    {

        private NotificationError(ErrorType type, string code) : base(type, code) { }


    }

}
