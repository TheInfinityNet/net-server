using System.ComponentModel.DataAnnotations;

namespace InfinityNetServer.Services.Identity.Application.DTOs.Requests
{
    public sealed record SignInRequest {

        [Required(ErrorMessage = "null_email")]
        [EmailAddress(ErrorMessage = "invalid_email")]
        public string Email {  get; set; }

        [Required(ErrorMessage = "null_password")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "size_password")]
        public string Password { get; set; }
    }
}
