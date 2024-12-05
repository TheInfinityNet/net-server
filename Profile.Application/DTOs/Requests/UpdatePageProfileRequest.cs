using InfinityNetServer.BuildingBlocks.Application.DTOs.Requests;
using System.ComponentModel.DataAnnotations;

namespace InfinityNetServer.Services.Profile.Application.DTOs.Requests
{
    public sealed record UpdatePageProfileRequest : BaseProfileRequest
    {

        [Required(ErrorMessage = "Required.Name")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "StringLength.LastName")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Required.Description")]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "StringLength.Description")]
        public string Description { get; set; }

    }

}
