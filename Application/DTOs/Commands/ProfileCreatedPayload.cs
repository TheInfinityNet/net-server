using InfinityNetServer.BuildingBlocks.Domain.Enums;
using System;

namespace InfinityNetServer.BuildingBlocks.Application.DTOs.Commands
{
    public sealed record ProfileCreatedPayload (

        string AccountId,

        string ProfileId,

        string Username,

        string FirstName,

        string MiddleName,

        string LastName,

        string MobileNumber,

        DateOnly Birthdate,

        Gender Gender

    );
}
