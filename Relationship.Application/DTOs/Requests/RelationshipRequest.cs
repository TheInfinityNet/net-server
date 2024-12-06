using System.ComponentModel.DataAnnotations;

namespace InfinityNetServer.Services.Relationship.Application.DTOs.Requests
{
    public sealed record RelationshipRequest
    {

        [Required(ErrorMessage = "Required.UserId")]
        public string UserId { get; set; }

    }
}
