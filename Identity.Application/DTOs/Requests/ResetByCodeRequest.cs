using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace InfinityNetServer.Services.Identity.Application.DTOs.Requests
{
    public sealed record ResetByCodeRequest(

        [Required(ErrorMessage = "null_password")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "size_password")]
        string NewPassword,

        [Required(ErrorMessage = "null_password")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "size_password")]
        string ConfirmPassword,

        [Required(ErrorMessage = "null_code")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "size_code")]
        string Code,

        bool AcceptTerms

    );
}
