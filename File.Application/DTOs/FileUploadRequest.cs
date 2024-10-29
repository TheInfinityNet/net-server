using System.ComponentModel.DataAnnotations;

namespace InfinityNetServer.Services.File.Application.DTOs
{
    public sealed record FileUploadRequest
    {
        [Required(ErrorMessage = "null_file_name")]
        public string FileName { get; set; }

        [Required(ErrorMessage = "null_content_type")]
        public string ContentType { get; set; }
    }
}
