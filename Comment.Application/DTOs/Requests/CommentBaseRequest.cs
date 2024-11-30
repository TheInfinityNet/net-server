using InfinityNetServer.BuildingBlocks.Application.DTOs.Others;
using System.ComponentModel.DataAnnotations;

namespace InfinityNetServer.Services.Comment.Application.DTOs.Requests
{
    public record CommentBaseRequest
    {

        [Required(ErrorMessage = "Required.Content")]
        public string PostId { get; set; }

        public string FileMetadataId { get; set; }

        [Required(ErrorMessage = "Required.Content")]
        public CommentContent Content { get; set; }

    }
}
