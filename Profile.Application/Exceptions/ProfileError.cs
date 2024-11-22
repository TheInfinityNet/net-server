using InfinityNetServer.BuildingBlocks.Application.Exceptions;

namespace InfinityNetServer.Services.Profile.Application.Exceptions
{
    public sealed record ProfileError : BaseError
    {

        private ProfileError(ErrorType type, string code) : base(type, code) { }

        public static readonly ProfileError INVALID_PROFILE_TYPE = new(ErrorType.ValidationError, $"{ErrorType.ValidationError}.InvalidProfileType");

    }

}
