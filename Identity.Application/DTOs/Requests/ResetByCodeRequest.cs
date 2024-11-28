using System.ComponentModel.DataAnnotations;

namespace InfinityNetServer.Services.Identity.Application.DTOs.Requests
{
    public sealed record ResetByCodeRequest
    {

        [Required(ErrorMessage = "Required.Password")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "StringLength.Password")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Required.PasswordConfirmation")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "StringLength.Password")]
        public string PasswordConfirmation { get; set; }

        [Required(ErrorMessage = "Required.Code")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "StringLength.Code")]
        public string Code { get; set; }

        public bool AcceptTerms { get; set; }

    }
}
