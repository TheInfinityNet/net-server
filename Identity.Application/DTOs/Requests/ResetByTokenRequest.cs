using System.ComponentModel.DataAnnotations;

namespace InfinityNetServer.Services.Identity.Application.DTOs.Requests
{
    public sealed record ResetByTokenRequest(

        [Required(ErrorMessage = "null_password")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "size_password")]
        string NewPassword,

        [Required(ErrorMessage = "null_password")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "size_password")]
        string ConfirmPassword,

        bool AcceptTerms

    );
}
