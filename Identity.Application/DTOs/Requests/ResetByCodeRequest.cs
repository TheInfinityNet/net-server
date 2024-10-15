using System.ComponentModel.DataAnnotations;

namespace InfinityNetServer.Services.Identity.Application.DTOs.Requests
{
    public sealed record ResetByCodeRequest 
    {

        [Required(ErrorMessage = "null_password")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "size_password")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "null_password")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "size_password")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "null_code")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "size_code")]
        public string Code { get; set; }

        public bool AcceptTerms { get; set; }

    }
}
