using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace InfinityNetServer.Services.Identity.Application.DTOs.Requests
{

    public sealed record RefreshTokenRequest
    {

        [Required(ErrorMessage = "Required.Token")]
        public string RefreshToken { get; set; }

    }
}
