using System.ComponentModel.DataAnnotations;

namespace InfinityNetServer.Services.Identity.Application.DTOs.Requests
{
    public sealed record RefreshRequest
    {

        [Required(ErrorMessage = "null_token")]
        [MinLength(1, ErrorMessage = "blank_token")]
        public string RefreshToken { get; set; }

    }
}
