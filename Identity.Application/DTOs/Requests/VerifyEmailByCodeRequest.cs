using System.ComponentModel.DataAnnotations;

namespace InfinityNetServer.Services.Identity.Application.DTOs.Requests
{
    public sealed record VerifyEmailByCodeRequest
    {

        [Required(ErrorMessage = "Required.Email")]
        [EmailAddress(ErrorMessage = "EmailAddress.Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Required.Code")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "StringLength.Code")]
        public string Code { get; set; }

    }
}
