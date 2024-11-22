using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Comment.Application.DTOs.Requests
{
    public class GetChildCommentsRequest
    {
        public Guid ParentCommentId { get; set; }
    }
}
