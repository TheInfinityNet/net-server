using System.ComponentModel.DataAnnotations;

namespace InfinityNetServer.Services.Identity.Application.DTOs.Requests
{
    public sealed record VerifyEmailByCodeRequest(

        [Required(ErrorMessage = "null_email")]
        [EmailAddress(ErrorMessage = "invalid_email")]
        string Email,

        [Required(ErrorMessage = "null_code")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "size_code")]
        string Code

    );
}
