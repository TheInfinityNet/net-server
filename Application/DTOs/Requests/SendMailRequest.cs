using InfinityNetServer.BuildingBlocks.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace InfinityNetServer.BuildingBlocks.Application.DTOs.Requests
{
    public sealed record SendMailRequest
    {

        [Required(ErrorMessage = "null_email")]
        [MinLength(1, ErrorMessage = "blank_email")]
        [EmailAddress(ErrorMessage = "invalid_email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "null_verification_type")]
        public VerificationType Type { get; set; }

    }
}
