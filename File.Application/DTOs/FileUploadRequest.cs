using System.ComponentModel.DataAnnotations;

namespace InfinityNetServer.Services.File.Application.DTOs
{
    public sealed record FileUploadRequest
    {
        [Required(ErrorMessage = "Required.FileName")]
        public string FileName { get; set; }

        [Required(ErrorMessage = "Required.ContentType")]
        public string ContentType { get; set; }
    }
}
