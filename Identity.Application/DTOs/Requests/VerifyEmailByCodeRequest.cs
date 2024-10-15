using System.ComponentModel.DataAnnotations;

namespace InfinityNetServer.Services.Identity.Application.DTOs.Requests
{
    public sealed record VerifyEmailByCodeRequest
    {

        [Required(ErrorMessage = "null_email")]
        [EmailAddress(ErrorMessage = "invalid_email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "null_code")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "size_code")]
        public string Code { get; set; }

    }
}
