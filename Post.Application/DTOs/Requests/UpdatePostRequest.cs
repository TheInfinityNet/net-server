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
    public class UpdatePostRequest
    {
        [Required]
        public Guid Id { get; set; }

        public PostContent Content { get; set; }

        public PostType Type { get; set; }

        public Guid? PresentationId { get; set; }

        public Guid? ParentId { get; set; }

        public Guid OwnerId { get; set; }

        public Guid? GroupId { get; set; }

        public Guid? FileMetadataId { get; set; }

        public List<PostPrivacy> PostPrivacies { get; set; }
    }
}
