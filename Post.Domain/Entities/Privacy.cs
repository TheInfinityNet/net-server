using System.Collections.Generic;
using System;

namespace InfinityNetServer.Services.Post.Domain.Entities
{
    public class Privacy
    {

        public bool IsPublic { get; set; } = true;

        public bool AllowComments { get; set; } = true;

        public List<Guid> VisibleToUserIds { get; set; } = [];

        public List<Guid> ExcludeUserIds { get; set; } = [];

    }
}
