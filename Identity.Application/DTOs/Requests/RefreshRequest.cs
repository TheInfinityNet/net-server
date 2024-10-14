using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace InfinityNetServer.Services.Identity.Application.DTOs.Requests
{
    [SwaggerSchema("DTO for refreshing a token")]
    public sealed record RefreshRequest
    {

        [Required(ErrorMessage = "null_token")]
        [MinLength(1, ErrorMessage = "blank_token")]
        [SwaggerSchema("The refresh token required for generating a new access token", Nullable = false)]
        public string RefreshToken { get; set; }

    }
}
