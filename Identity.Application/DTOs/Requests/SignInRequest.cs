using System.ComponentModel.DataAnnotations;

namespace InfinityNetServer.Services.Identity.Application.DTOs.Requests
{
    public sealed record SignInRequest(

        [Required(ErrorMessage = "null_email")]
        [EmailAddress(ErrorMessage = "invalid_email")]
        string Email,

        [Required(ErrorMessage = "null_password")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "size_password")]
        string Password
    );
}
