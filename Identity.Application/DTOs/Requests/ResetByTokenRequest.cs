using System.ComponentModel.DataAnnotations;

namespace InfinityNetServer.Services.Identity.Application.DTOs.Requests
{
    public sealed record ResetByTokenRequest
    {

        [Required(ErrorMessage = "Required.Password")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "StringLength.Password")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Required.PasswordConfirmation")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "StringLength.Password")]
        public string PasswordConfirmation { get; set; }

        public bool AcceptTerms { get; set; }

    }
}
