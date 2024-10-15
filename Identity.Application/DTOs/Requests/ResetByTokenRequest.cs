using System.ComponentModel.DataAnnotations;

namespace InfinityNetServer.Services.Identity.Application.DTOs.Requests
{
    public sealed record ResetByTokenRequest 
    {

        [Required(ErrorMessage = "null_password")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "size_password")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "null_password")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "size_password")]
        public string ConfirmPassword { get; set; }

        public bool AcceptTerms { get; set; }

    }
}
