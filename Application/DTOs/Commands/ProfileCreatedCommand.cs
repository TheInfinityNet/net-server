using InfinityNetServer.BuildingBlocks.Domain.Enums;
using System;

namespace InfinityNetServer.BuildingBlocks.Application.DTOs.Commands
{
    public sealed record ProfileCreatedCommand (

        string AccountId,

        string Username,

        string FirstName,

        string MiddleName,

        string LastName,

        string MobileNumber,

        DateTime Birthdate,

        Gender Gender

    ) : IIntegrationEvent;
}
