using InfinityNetServer.Services.Post.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Post.Application.DTOs.Responses
{
    public class PostPrivacyResponse
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public PostPrivacyType Type { get; set; }

        public List<Guid> PostPrivacyIncludes { get; set; } = new List<Guid>();
        public List<Guid> PostPrivacyExcludes { get; set; } = new List<Guid>();
    }
}
