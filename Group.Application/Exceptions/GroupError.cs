using InfinityNetServer.BuildingBlocks.Application.Exceptions;

namespace InfinityNetServer.Services.Group.Application.Exceptions
{
    public sealed record GroupError : BaseError
    {

        private GroupError(ErrorType type, string code) : base(type, code) { }

    }

}
