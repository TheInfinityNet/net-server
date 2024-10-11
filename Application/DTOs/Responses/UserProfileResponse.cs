using InfinityNetServer.BuildingBlocks.Domain.Enums;
using System;

namespace InfinityNetServer.BuildingBlocks.Application.DTOs.Responses
{
    public sealed record UserProfileResponse
    (
        Guid Id,
        string Email,
        string FirstName,
        string LastName,
        string MobileNumber,
        DateTime BirthDate,
        Gender Gender
    );
}
