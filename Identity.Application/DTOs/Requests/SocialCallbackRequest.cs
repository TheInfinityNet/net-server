using System.ComponentModel.DataAnnotations;

namespace InfinityNetServer.Services.Identity.Application.DTOs.Requests
{
    public sealed record SocialCallbackRequest
    {

        [Required(ErrorMessage = "Required.Code")]
        public string Code { get; set; }

        public string Provider { get; set; }

    }
}
