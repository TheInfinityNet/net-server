using InfinityNetServer.Services.Post.Domain.Entities;
using InfinityNetServer.Services.Post.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Post.Application.DTOs.Responses
{
    public class PostResponse
    {
        public Guid Id { get; set; }

        public PostContent Content { get; set; }

        public PostType Type { get; set; }

        public Guid? PresentationId { get; set; }

        public Guid? ParentId { get; set; }

        public Guid OwnerId { get; set; }

        public Guid? GroupId { get; set; }

        public Guid? FileMetadataId { get; set; }

        public ICollection<PostPrivacy> PostPrivacies { get; set; }

        public ICollection<PostResponse> SharedPosts { get; set; }

        public ICollection<PostResponse> SubPosts { get; set; }
    }
}
