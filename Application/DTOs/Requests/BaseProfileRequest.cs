using System.ComponentModel.DataAnnotations;

namespace InfinityNetServer.BuildingBlocks.Application.DTOs.Requests
{
    public abstract record BaseProfileRequest
    {

        public string Id { get; set; }

        [Required(ErrorMessage = "Required.MobileNumber")]
        [Phone(ErrorMessage = "Phone.MobileNumber")]
        public string MobileNumber { get; set; }

    }
}
