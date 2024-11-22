using InfinityNetServer.Services.Post.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Post.Application.DTOs.Requests
{
    public class CreatePostPrivacyRequest
    {
        public Guid PostId { get; set; }
        public PostAudienceType Type { get; set; } = PostAudienceType.Public;
    }
}
