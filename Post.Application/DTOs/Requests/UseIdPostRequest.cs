using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Post.Application.DTOs.Requests
{
    public class UseIdPostRequest
    {
        [Required]
        public Guid Id { get; set; }
    }
}
