using InfinityNetServer.Services.Post.Domain.Entities;
using InfinityNetServer.Services.Post.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Post.Application.DTOs.Requests
{
    public class CreatePostRequest
    {
        [Required]
        public PostContent Content { get; set; }

        [Required]
        public PostType Type { get; set; }

        public Guid? PresentationId { get; set; }

        public Guid? ParentId { get; set; }

        [Required]
        public Guid OwnerId { get; set; }

        public Guid? GroupId { get; set; }

        public Guid? FileMetadataId { get; set; }

        public List<PostAudience> PostPrivacies { get; set; } = new List<PostAudience>();
    }
}
