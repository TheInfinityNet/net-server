using System.ComponentModel.DataAnnotations;

namespace InfinityNetServer.Services.Identity.Application.DTOs.Requests
{
    public sealed record SignInRequest {

        [Required(ErrorMessage = "Required.Email")]
        [EmailAddress(ErrorMessage = "EmailAddress.Email")]
        public string Email {  get; set; }

        [Required(ErrorMessage = "Required.Password")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "StringLength.Password")]
        public string Password { get; set; }
    }
}
