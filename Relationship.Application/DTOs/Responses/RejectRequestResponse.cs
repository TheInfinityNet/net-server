using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Relationship.Application.DTOs.Responses
{
    public class RejectRequestResponse
    {
        public string UserId { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
    }
}
