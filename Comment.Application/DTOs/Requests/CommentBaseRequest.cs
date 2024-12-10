using InfinityNetServer.BuildingBlocks.Application.DTOs.Others;
using InfinityNetServer.Services.Comment.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace InfinityNetServer.Services.Comment.Application.DTOs.Requests
{
    public record CommentBaseRequest
    {

        [Required(ErrorMessage = "Required.PostId")]
        public string PostId { get; set; }

        public string Type { get; set; } = CommentType.Text.ToString();

        public string FileMetadataId { get; set; }

        [Required(ErrorMessage = "Required.Content")]
        public CommentContent Content { get; set; }

    }
}
