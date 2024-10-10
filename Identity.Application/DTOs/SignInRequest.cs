using System.ComponentModel.DataAnnotations;

namespace InfinityNetServer.Services.Identity.Application.DTOs
{
    public sealed record SignInRequest
    {

        [Required(ErrorMessage = "null_email")]
        [MinLength(1, ErrorMessage = "blank_email")]
        [EmailAddress(ErrorMessage = "invalid_email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "null_password")]
        [MinLength(1, ErrorMessage = "blank_password")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "size_password")]
        public string Password { get; set; }

    }
}
