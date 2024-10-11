using InfinityNetServer.BuildingBlocks.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using InfinityNetServer.BuildingBlocks.Application.DTOs;

namespace InfinityNetServer.BuildingBlocks.Application.DTOs.Requests
{
    public sealed record SendMailRequest
    (

        [Required(ErrorMessage = "null_email")]
        [MinLength(1, ErrorMessage = "blank_email")]
        [EmailAddress(ErrorMessage = "invalid_email")]
        string Email,

        [Required(ErrorMessage = "null_verification_type")]
        VerificationType Type

    ) : IIntegrationEvent;
}
